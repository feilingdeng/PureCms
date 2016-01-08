using PureCms.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace PureCms.Core.Context
{

    public abstract class QueryDescriptor<T, TChild>
        where T : class
        where TChild : class, new()
    {
        /// <summary>
        /// 设置接收条件值的对象
        /// </summary>
        /// <returns></returns>
        public abstract object GetConditionContainer();
        private int _topCount;
        /// <summary>
        /// 查询前N条记录
        /// </summary>
        public int TopCount { get { return _topCount; } set { this._topCount = value; } }

        private PageDescriptor _pagingDescriptor;
        /// <summary>
        /// 分页信息
        /// </summary>
        public PageDescriptor PagingDescriptor
        {
            get
            {
                return _pagingDescriptor;
            }
            set
            {
                this._pagingDescriptor = value;
            }
        }

        private List<SortDescriptor<T>> _sortingDescriptor;
        /// <summary>
        /// 排序信息
        /// </summary>
        public List<SortDescriptor<T>> SortingDescriptor
        {
            get
            {
                return _sortingDescriptor;
            }
            set
            {
                this._sortingDescriptor = value;
            }
        }
        /// <summary>
        /// 设置读取记录数
        /// </summary>
        /// <param name="take"></param>
        /// <returns></returns>
        public TChild Take(int take)
        {
            _topCount = take;
            return this as TChild;
        }
        /// <summary>
        /// 设置分页信息
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public TChild Page(int currentPage, int pageSize)
        {
            if (_pagingDescriptor == null)
            {
                _pagingDescriptor = new PageDescriptor(currentPage, pageSize);
            }
            else
            {
                _pagingDescriptor.PageNumber = currentPage;
                _pagingDescriptor.PageSize = pageSize;
            }
            return this as TChild;
        }
        /// <summary>
        /// 设置排序信息
        /// </summary>
        /// <param name="sorts"></param>
        /// <returns></returns>
        public TChild Sort(params SortDescriptor<T>[] sorts)
        {
            if (this._sortingDescriptor == null)
            {
                this._sortingDescriptor = new List<SortDescriptor<T>>();
            }
            foreach (var item in sorts)
            {
                if (!this._sortingDescriptor.Contains(item))
                {
                    this._sortingDescriptor.Add(item);
                }
            }
            return this as TChild;
        }
        /// <summary>
        /// 设置排序信息
        /// </summary>
        /// <param name="sorts"></param>
        /// <returns></returns>
        public TChild Sort(params Func<SortDescriptor<T>, SortDescriptor<T>>[] sorts)
        {
            if (this._sortingDescriptor == null)
            {
                this._sortingDescriptor = new List<SortDescriptor<T>>();
            }
            foreach (var item in sorts)
            {
                var sd = item(new SortDescriptor<T>());

                if (!this._sortingDescriptor.Contains(sd))
                {
                    this._sortingDescriptor.Add(sd);
                }
            }
            return this as TChild;
        }
        /// <summary>
        /// 设置过滤条件
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public TChild Where(Expression<Func<TChild, object>> field, object value)
        {
            var name = ExpressionHelper.GetPropertyName<TChild>(field);
            Type t = typeof(TChild);
            t.SetPropertyValue(GetConditionContainer(), name, value);
            
            return this as TChild;
        }
        public TChild Where(Expression<Func<FilterContainer<T>, object>> filter)
        {

            return this as TChild;
        }
        public TChild Where(FilterContainer<T> filter)
        {

            return this as TChild;
        }
        /// <summary>
        /// 设置过滤条件
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public TChild Where(Expression<Func<T, bool>> predicate)
        {
            var translator = new QueryTranslator();
            QueryText = translator.Translate(predicate);
            Parameters = translator.Parameters;
            
            return this as TChild;
        }
        public string QueryText { get; set; }
        public List<QueryParameter> Parameters { get; set; }

        private List<string> _columns;
        public List<string> Columns
        {
            get
            {
                return _columns;
            }
            set
            {
                _columns = value;
            }
        }
        public TChild Select(params Expression<Func<T, object>>[] fields)
        {
            if (_columns == null)
            {
                _columns = new List<string>();
            }
            foreach (var item in fields)
            {
                if (item.Body is NewExpression)
                {
                    var m = ((NewExpression)item.Body).Members;
                    _columns.AddRange(m.AsEnumerable().Select(f=>f.Name));
                }
                else if(item.Body is MemberExpression) {
                    _columns.Add(((MemberExpression)item.Body).Member.Name); 
                }
            }
            return this as TChild;
        }
    }

    public class FilterContainer<T> where T : class
    {
        private QueryTranslator _translator = new QueryTranslator();
        private StringBuilder _queryText = new StringBuilder();
        private List<KeyValuePair<string, object>> _parameters = new List<KeyValuePair<string, object>>();
        public string QueryText
        {
            get
            {
                return _queryText.ToString();
            }
        }
        public FilterContainer<T> And(Expression<Func<T, bool>> predicate)
        {
            if(_queryText.Length > 0)
            {
                _queryText.Append(" AND ");
            }
            _queryText.Append(_translator.Translate(predicate));
            return this;
        }
        public FilterContainer<T> Or(Expression<Func<T, bool>> predicate)
        {
            if (_queryText.Length > 0)
            {
                _queryText.Append(" OR ");
            }
            _queryText.Append(_translator.Translate(predicate));
            return this;
        }
    }

    public class QueryParameter
    {
        public string Name { get; set; }

        public object Value { get; set; }
    }
    public class QueryTranslator : ExpressionVisitor
    {
        private StringBuilder _queryText;

        private List<string> _parameters = new List<string>();
        private List<object> _values = new List<object>();

        public List<QueryParameter> Parameters
        {
            get
            {
                List<QueryParameter> result = new List<QueryParameter>();
                int i = 0;
                foreach (var p in _parameters)
                {
                    result.Add(new QueryParameter() {
                        Name = p
                        ,Value = _values[i]
                    });
                    i++;
                }
                return result;
            }
        }

        public QueryTranslator()
        {
        }

        public string Translate(Expression expression)
        {
            this._queryText = new StringBuilder();
            this.Visit(expression);
            return this._queryText.ToString();
        }

        private void SetParam(string key, object value)
        {
            _parameters.Add(key);
        }

        private static Expression StripQuotes(Expression e)
        {
            while (e.NodeType == ExpressionType.Quote)
            {
                e = ((UnaryExpression)e).Operand;
            }
            return e;
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            //if (m.Method.DeclaringType == typeof(QueryExtensions) && m.Method.Name == "Where")
            //{
            //    sb.Append("SELECT * FROM ");
            //    this.Visit(m.Arguments[0]);
            //    sb.Append(" AS T WHERE ");
            //    LambdaExpression lambda = (LambdaExpression)StripQuotes(m.Arguments[1]);
            //    this.Visit(lambda.Body);
            //    return m;
            //}
            //if (m.Method.DeclaringType == typeof(string) && m.Method.Name == "Contains")
            //{
            //    this.Visit(m.Object);
            //    _queryText.Append(" LIKE '%");
            //    this.Visit(m.Arguments[0]);
            //    _queryText.Append("%' ");
            //    return m;
            //}
            if (m.Method.DeclaringType == typeof(MemberExtensions) && m.Method.Name == "Like")
            {
                _queryText.Append(((MemberExpression)m.Arguments[0]).Member.Name);
                _queryText.Append(" LIKE '%");
                _queryText.Append("@" + _values.Count);
                _queryText.Append("%' ");
                _values.Add(((ConstantExpression)m.Arguments[1]).Value);
                return m;
            }
            if (m.Method.DeclaringType == typeof(MemberExtensions) && m.Method.Name == "In")
            {
                _queryText.Append(((MemberExpression)m.Arguments[0]).Member.Name);
                _queryText.Append(" IN (");
                var newArray = ((NewArrayExpression)m.Arguments[1]).Expressions;
                var values = new List<object>();
                foreach (var n in newArray)
                {
                    if (n is MemberExpression)
                    {
                        var v = GetMemberValue(n as MemberExpression);
                        values.Add(v);
                    }
                    else if(n is ConstantExpression)
                    {
                        values.Add(((ConstantExpression)n).Value);
                    }
                }
                _queryText.Append("@" + _values.Count);
                _queryText.Append(") ");
                _values.Add(values);
                return m;
            }
            if (m.Method.DeclaringType == typeof(MemberExtensions) && m.Method.Name == "IsNull")
            {
                if (m.Arguments[0] is MemberExpression)
                {
                    _queryText.Append(((MemberExpression)m.Arguments[0]).Member.Name);
                }
                else
                {
                    this.Visit(m.Arguments[0]);
                }
                _queryText.Append(" IS NULL ");
                return m;
            }
            if (m.Method.DeclaringType == typeof(MemberExtensions) && m.Method.Name == "IsNotNull")
            {
                if (m.Arguments[0] is MemberExpression)
                {
                    _queryText.Append(((MemberExpression)m.Arguments[0]).Member.Name);
                }
                else
                {
                    this.Visit(m.Arguments[0]);
                }
                _queryText.Append(" IS NOT NULL ");
                return m;
            }
            throw new NotSupportedException(string.Format("方法{0}不支持", m.Method.Name));
        }

        protected override Expression VisitUnary(UnaryExpression u)
        {
            switch (u.NodeType)
            {
                case ExpressionType.Not:
                    _queryText.Append(" NOT ");
                    this.Visit(u.Operand);
                    break;
                case ExpressionType.Convert:
                    this.Visit(u.Operand);
                    break;
                default:
                    throw new NotSupportedException(string.Format("运算符{0}不支持", u.NodeType));
            }
            return u;
        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            _queryText.Append("(");
            this.Visit(b.Left);
            switch (b.NodeType)
            {
                case ExpressionType.And:
                    _queryText.Append(" AND ");
                    break;
                case ExpressionType.AndAlso:
                    _queryText.Append(" AND ");
                    break;
                case ExpressionType.Or:
                    _queryText.Append(" OR ");
                    break;
                case ExpressionType.OrElse:
                    _queryText.Append(" OR ");
                    break;
                case ExpressionType.Equal:
                    _queryText.Append(" = ");
                    break;
                case ExpressionType.NotEqual:
                    _queryText.Append(" <> ");
                    break;
                case ExpressionType.LessThan:
                    _queryText.Append(" < ");
                    break;
                case ExpressionType.LessThanOrEqual:
                    _queryText.Append(" <= ");
                    break;
                case ExpressionType.GreaterThan:
                    _queryText.Append(" > ");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    _queryText.Append(" >= ");
                    break;
                default:
                    throw new NotSupportedException(string.Format("运算符{0}不支持", b.NodeType));
            }
            this.Visit(b.Right);
            _queryText.Append(")");
            return b;
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            if (c.Value is IQueryable)
            {
                IQueryable q = c.Value as IQueryable;
                _queryText.Append(q.ElementType.Name);
            }
            else if (c.Value == null)
            {
                _queryText.Append("NULL");
            }
            else
            {
                //switch (Type.GetTypeCode(c.Value.GetType()))
                //{
                //    case TypeCode.Boolean:
                //        _queryText.Append(((bool)c.Value) ? 1 : 0);
                //        break;
                //    case TypeCode.String:
                //        _queryText.Append("'");
                //        _queryText.Append(c.Value);
                //        _queryText.Append("'");
                //        break;
                //    case TypeCode.DateTime:
                //        _queryText.Append("'");
                //        _queryText.Append(c.Value);
                //        _queryText.Append("'");
                //        break;
                //    case TypeCode.Object:
                //        throw new NotSupportedException(string.Format("常量{0}不支持", c.Value));
                //    default:
                //        _queryText.Append(c.Value);
                //        break;
                //}

                _queryText.Append("@"+_values.Count);
                _values.Add(c.Value);
            }
            return c;
        }

        protected override Expression VisitMember(MemberExpression m)
        {
            if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter)
            {
                _queryText.Append(m.Member.Name);
                SetParam(m.Member.Name + _parameters.Count, null);
                return m;
            }
            else if (m.Expression != null && m.Expression.NodeType == ExpressionType.MemberAccess)
            {
                var v = GetMemberValue(m);
                //_queryText.Append(v);
                _queryText.Append("@" + _values.Count);
                _values.Add(v);
                //SetParam(m.Member.Name + Parameters.Count, v);
                return m;
            }
            throw new NotSupportedException(string.Format("成员{0}不支持", m.Member.Name));
        }


        // 获取属性值
        private Object GetMemberValue(MemberExpression memberExpression)
        {
            MemberInfo memberInfo;
            Object obj;
            Object result;

            if (memberExpression == null)
                throw new ArgumentNullException("memberExpression");


            if (memberExpression.Expression is ConstantExpression)
                obj = ((ConstantExpression)memberExpression.Expression).Value;
            else if (memberExpression.Expression is MemberExpression)
                obj = GetMemberValue((MemberExpression)memberExpression.Expression);
            else
                throw new NotSupportedException("Expression type not supported: "
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
                throw new NotSupportedException("Member type not supported: "
                    + memberInfo.GetType().FullName);
            }
            //if(result is String || result is DateTime)
            //{
            //    result = ("'" + result.ToString() + "'");
            //}
            //if (result is Boolean)
            //{
            //    result = (bool.Parse(result.ToString()) ? 1 : 0);
            //}
            return result;
        }
    }

    public static class MemberExtensions
    {
        public const string Tips = "此扩展方法只能用于本框架";
        /// <summary>
        /// IN
        /// </summary>
        public static bool In(this object member, IEnumerable<object> values)
        {
            throw new Exception(Tips);
        }
        /// <summary>
        /// IN
        /// </summary>
        public static bool In(this object member, params object[] values)
        {
            throw new Exception(Tips);
        }
        /// <summary>
        /// IN
        /// </summary>
        public static bool NotIn(this object member, IEnumerable<object> values)
        {
            throw new Exception(Tips);
        }
        /// <summary>
        /// IN
        /// </summary>
        public static bool NotIn(this object member, params object[] values)
        {
            throw new Exception(Tips);
        }
        /// <summary>
        /// LIKE
        /// </summary>
        public static bool Like(this string member, string values)
        {
            throw new Exception(Tips);
        }
        /// <summary>
        /// IS NULL
        /// </summary>
        public static bool IsNull(this object member)
        {
            throw new Exception(Tips);
        }
        /// <summary>
        /// IS NOT NULL
        /// </summary>
        public static bool IsNotNull(this object member)
        {
            throw new Exception(Tips);
        }
    }

    public static class QueryExtensions
    {
        public static string Where<TSource>(this IQueryable<TSource> source,
            Expression<Func<TSource, bool>> predicate)
        {
            var expression = Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod())
            .MakeGenericMethod(new Type[] { typeof(TSource) }),
            new Expression[] { source.Expression, Expression.Quote(predicate) });

            var translator = new QueryTranslator();
            return translator.Translate(expression);
        }
    }
}
