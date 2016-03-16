using PureCms.Core.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace PureCms.Core.Context
{
    public class ExpressionResolver
    {
        private List<QueryParameter> _arguments = new List<QueryParameter>();
        public List<QueryParameter> Arguments
        {
            get { return _arguments; }
        }

        private string _queryText;
        public string QueryText { get { return _queryText; } }
        private int _index = 0;
        /// <summary>
        /// 解析lamdba，生成Sql查询条件
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public string ResolveToSql(Expression expression)
        {
            var s = Resolve(expression);
            this._queryText += s;
            return s;
        }

        private object GetValue(Expression expression)
        {
            if (expression is ConstantExpression)
                return (expression as ConstantExpression).Value;
            else if (expression is UnaryExpression)
            {
                UnaryExpression unary = expression as UnaryExpression;
                LambdaExpression lambda = Expression.Lambda(unary.Operand);
                Delegate fn = lambda.Compile();
                return fn.DynamicInvoke(null);
            }
            else if (expression is MemberExpression)
            {
                return GetMemberValue(expression as MemberExpression);
            }
            else if (expression is MethodCallExpression)
            {
                return GetValue((expression as MethodCallExpression).Arguments[0]);
            }
            throw new Exception("无法获取值" + expression);
        }
        // 获取属性值
        private object GetMemberValue(MemberExpression memberExpression)
        {
            MemberInfo memberInfo;
            object obj;
            object result;

            if (memberExpression == null)
                throw new ArgumentNullException("memberExpression");


            if (memberExpression.Expression is ConstantExpression)
                obj = ((ConstantExpression)memberExpression.Expression).Value;
            else if (memberExpression.Expression is MemberExpression)
                obj = GetMemberValue((MemberExpression)memberExpression.Expression);
            else
                throw new NotSupportedException("不支持的表达式类型: "
                    + memberExpression.Expression.GetType().FullName);

            memberInfo = memberExpression.Member;
            if (memberInfo is PropertyInfo)
            {
                PropertyInfo property = (PropertyInfo)memberInfo;

                result = property.GetValue(obj, null);
            }
            else if (memberInfo is FieldInfo)
            {
                FieldInfo field = (FieldInfo)memberInfo;
                result = field.GetValue(obj);
            }
            else
            {
                throw new NotSupportedException("不支持的成员: "
                    + memberInfo.GetType().FullName);
            }
            return result;
        }

        private string Resolve(Expression expression)
        {
            if (expression is LambdaExpression)
            {
                LambdaExpression lambda = expression as LambdaExpression;
                expression = lambda.Body;
                return Resolve(expression);
            }
            else if (expression is BinaryExpression)//解析二元运算符
            {
                BinaryExpression binary = expression as BinaryExpression;
                if (binary.Left is MemberExpression)
                {
                    object value = GetValue(binary.Right);
                    return ResolveFunc(binary.Left, value, binary.NodeType);
                }
                else if (binary.Left is MethodCallExpression && (binary.Right is UnaryExpression || binary.Right is MemberExpression || binary.Right is ConstantExpression))
                {
                    object value = GetValue(binary.Right);
                    return ResolveLinqToObject(binary.Left, value, binary.NodeType);
                }
                else if (binary.Left is UnaryExpression)
                {
                    object value = GetValue(binary.Right);
                    return ResolveFunc(binary.Left, value, binary.NodeType);
                }
            }
            else if (expression is UnaryExpression)//解析一元运算符
            {
                UnaryExpression unary = expression as UnaryExpression;
                if (unary.Operand is MethodCallExpression)
                {
                    return ResolveLinqToObject(unary.Operand, false);
                }
                else if (unary.Operand is MemberExpression)
                {
                    return ResolveFunc(unary.Operand, false, ExpressionType.Equal);
                }
            }
            else if (expression is MethodCallExpression)//解析扩展方法
            {
                return ResolveLinqToObject(expression, true);
            }
            else if (expression is MemberExpression)//解析属性，如x.Deletion
            {
                return ResolveFunc(expression, true, ExpressionType.Equal);
            }
            var body = expression as BinaryExpression;
            if (body == null)
                throw new Exception("无法解析表达式：" + expression);
            var operatorName = GetOperatorName(body.NodeType);
            var Left = Resolve(body.Left);
            var Right = Resolve(body.Right);
            string result = string.Format("({0} {1} {2})", Left, operatorName, Right);
            return result;
        }

        /// <summary>
        /// 根据条件生成对应的sql查询操作符
        /// </summary>
        /// <param name="expressiontype"></param>
        /// <returns></returns>
        private string GetOperatorName(ExpressionType expressiontype)
        {
            switch (expressiontype)
            {
                case ExpressionType.And:
                    return "and";
                case ExpressionType.AndAlso:
                    return "and";
                case ExpressionType.Or:
                    return "or";
                case ExpressionType.OrElse:
                    return "or";
                case ExpressionType.Equal:
                    return "=";
                case ExpressionType.NotEqual:
                    return "<>";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                default:
                    throw new Exception(string.Format("不支持{0}此种运算符查找！" + expressiontype));
            }
        }


        private string ResolveFunc(Expression left, object value, ExpressionType expressiontype)
        {
            MemberExpression me = null;
            if (left is UnaryExpression)
            {
                me = (left as UnaryExpression).Operand as MemberExpression;
            }
            else {
                me = left as MemberExpression;
            }
            string name = me.Member.Name;
            string operatorName = GetOperatorName(expressiontype);
            return BuildQuery(me.Expression.Type, name, operatorName, value.ToString());
        }

        private string ResolveLinqToObject(Expression expression, object value, ExpressionType? expressiontype = null)
        {
            var methodCall = expression as MethodCallExpression;
            var methodName = methodCall.Method.Name;
            switch (methodName)
            {
                case "Like":
                    return Like(methodCall);
                case "In":
                    return In(methodCall, true);
                case "NotIn":
                    return In(methodCall, false);
                case "IsNull":
                    return IsNull(methodCall, true);
                case "IsNotNull":
                    return IsNull(methodCall, false);
                case "Count":
                    return Len(methodCall, value, expressiontype.Value);
                case "LongCount":
                    return Len(methodCall, value, expressiontype.Value);
                default:
                    //return InvokeFunc(methodCall);
                    throw new Exception(string.Format("不支持{0}方法的查找！", methodName));
            }
        }

        private string SetArgument(string name, object value)
        {
            //name = "@" + name;
            //string temp = name;
            //while (Argument.ContainsKey(temp))
            //{
            //    temp = name + index;
            //    index = index + 1;
            //}
            string temp = "@" + _index;
            _index = _index + 1;
            //_argument[temp] = value;
            this._arguments.Add(new QueryParameter(temp, value));
            return temp;
        }

        private string BuildQuery(Type entityType, string name, string op, string value)
        {
            string paramName = SetArgument(name, value);
            var fieldName = PocoHelper.FormatColumn(entityType, name);
            var i = fieldName.IndexOf("AS");
            string result = string.Format("({0} {1} {2})", i > 1 ? fieldName.Substring(0, i - 1) : fieldName, op, paramName);
            return result;
        }

        private string In(MethodCallExpression expression, object isTrue)
        {
            var newArray = ((NewArrayExpression)expression.Arguments[1]).Expressions;
            List<string> inParas = new List<string>();
            int i = 0;
            foreach (var n in newArray)
            {
                string name_para = "InParameter" + i;
                object value = null;
                if (n is MemberExpression)
                {
                    value = GetMemberValue(n as MemberExpression);
                }
                else if (n is ConstantExpression)
                {
                    value = ((ConstantExpression)n).Value;
                }
                else if (n is MethodCallExpression)
                {
                    value = GetMemberValue(GetMemberExpression((MethodCallExpression)n));// (((MethodCallExpression)n).Object as MemberExpression);
                }
                string Key = SetArgument(name_para, value);
                inParas.Add(Key);
                i++;
            }
            string name = string.Empty;
            MemberExpression me = GetMemberExpression(expression);
            name = GetMemberName(expression);
            string operatorName = Convert.ToBoolean(isTrue) ? "IN" : "NOT IN";
            string paraName = string.Join(",", inParas);
            string result = string.Format("{0} {1} ({2})", PocoHelper.FormatColumn(me.Expression.Type, name), operatorName, paraName);
            return result;
        }

        private string Like(MethodCallExpression expression)
        {
            object val = GetValue(expression.Arguments[1]);
            string value = string.Format("%{0}%", val);
            string name = string.Empty;
            MemberExpression me = GetMemberExpression(expression);
            name = GetMemberName(expression);
            string paraName = SetArgument(name, value);
            string result = string.Format("{0} LIKE {1}", PocoHelper.FormatColumn(me.Expression.Type, name), paraName);
            return result;
        }

        private string Len(MethodCallExpression expression, object value, ExpressionType expressiontype)
        {
            string name = string.Empty;
            MemberExpression me = GetMemberExpression(expression);
            name = GetMemberName(expression);
            string operatorName = GetOperatorName(expressiontype);
            string paraName = SetArgument(name, value.ToString());
            string result = string.Format("LEN({0}){1}{2}", PocoHelper.FormatColumn(me.Expression.Type, name), operatorName, paraName);
            return result;
        }

        private string IsNull(MethodCallExpression expression, bool isNull = true)
        {
            string name = string.Empty;
            MemberExpression me = GetMemberExpression(expression);
            name = GetMemberName(expression);
            string result = string.Format("{0} IS{1}NULL", PocoHelper.FormatColumn(me.Expression.Type, name), isNull ? " " : " NOT ");
            return result;
        }
        private string InvokeFunc(MethodCallExpression expression)
        {
            MemberExpression me = GetMemberExpression(expression);
            return string.Empty;
        }

        private string GetMemberName(MethodCallExpression expression)
        {
            string name = string.Empty;
            if (expression.Arguments[0] is UnaryExpression)
            {
                name = (((UnaryExpression)expression.Arguments[0]).Operand as MemberExpression).Member.Name;
            }
            else {
                name = ((MemberExpression)expression.Arguments[0]).Member.Name;
            }
            return name;
        }

        private MemberExpression GetMemberExpression(MethodCallExpression expression)
        {
            MemberExpression me = null;

            if (expression.Arguments[0] is MemberExpression)
            {
                me = expression.Arguments[0] as MemberExpression;
            }
            else if (expression.Arguments[0] is UnaryExpression)
            {
                me = ((UnaryExpression)expression.Arguments[0]).Operand as MemberExpression;
            }
            else if (expression.Object is MemberExpression)
            {
                me = expression.Object as MemberExpression;
            }

            return me;
        }
    }
}
