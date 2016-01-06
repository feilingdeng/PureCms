using PureCms.Utilities;
using System;
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
        public TChild Conditions(Expression<Func<T, bool>> predicate)
        {
            var translator = new QueryTranslator();
            var a = translator.Translate(predicate);
            
            return this as TChild;
        }

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
            //fields.Select(e => (PropertyPathMarker)e);
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
                    _columns.Add(((MemberExpression)item.Body).Member.Name); //(ExpressionHelper.GetPropertyName<T>(item));
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


    public class QueryTranslator : ExpressionVisitor
    {
        StringBuilder sb;

        public QueryTranslator()
        {
        }

        public string Translate(Expression expression)
        {
            this.sb = new StringBuilder();
            this.Visit(expression);
            return this.sb.ToString();
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
            //m.Method.DeclaringType == typeof(QueryExtensions) && 
            //if (m.Method.Name == "Where")
            //{
            //    sb.Append("SELECT * FROM ");
            //    this.Visit(m.Arguments[0]);
            //    sb.Append(" AS T WHERE ");
            //    LambdaExpression lambda = (LambdaExpression)StripQuotes(m.Arguments[1]);
            //    this.Visit(lambda.Body);
            //    return m;
            //}
            if (m.Method.DeclaringType == typeof(String) && m.Method.Name == "Contains")
            {
                this.Visit(m.Object);
                sb.Append(" like '%");
                this.Visit(m.Arguments[0]);
                sb.Append("%' ");
                return m;
            }
            throw new NotSupportedException(string.Format("方法{0}不支持", m.Method.Name));
        }

        protected override Expression VisitUnary(UnaryExpression u)
        {
            switch (u.NodeType)
            {
                case ExpressionType.Not:
                    sb.Append(" NOT ");
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
            sb.Append("(");
            this.Visit(b.Left);
            switch (b.NodeType)
            {
                case ExpressionType.And:
                    sb.Append(" AND ");
                    break;
                case ExpressionType.AndAlso:
                    sb.Append(" AND ");
                    break;
                case ExpressionType.Or:
                    sb.Append(" OR ");
                    break;
                case ExpressionType.OrElse:
                    sb.Append(" OR ");
                    break;
                case ExpressionType.Equal:
                    sb.Append(" = ");
                    break;
                case ExpressionType.NotEqual:
                    sb.Append(" <> ");
                    break;
                case ExpressionType.LessThan:
                    sb.Append(" < ");
                    break;
                case ExpressionType.LessThanOrEqual:
                    sb.Append(" <= ");
                    break;
                case ExpressionType.GreaterThan:
                    sb.Append(" > ");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    sb.Append(" >= ");
                    break;
                default:
                    throw new NotSupportedException(string.Format("运算符{0}不支持", b.NodeType));
            }
            this.Visit(b.Right);
            sb.Append(")");
            return b;
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            if (c.Value is IQueryable)
            {
                IQueryable q = c.Value as IQueryable;
                sb.Append(q.ElementType.Name);
            }
            else if (c.Value == null)
            {
                sb.Append("NULL");
            }
            else
            {
                switch (Type.GetTypeCode(c.Value.GetType()))
                {
                    case TypeCode.Boolean:
                        sb.Append(((bool)c.Value) ? 1 : 0);
                        break;
                    case TypeCode.String:
                        sb.Append("'");
                        sb.Append(c.Value);
                        sb.Append("'");
                        break;
                    case TypeCode.DateTime:
                        sb.Append("'");
                        sb.Append(c.Value);
                        sb.Append("'");
                        break;
                    case TypeCode.Object:
                        throw new NotSupportedException(string.Format("常量{0}不支持", c.Value));
                    default:
                        sb.Append(c.Value);
                        break;
                }
            }
            return c;
        }

        protected override Expression VisitMember(MemberExpression m)
        {
            if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter)
            {
                sb.Append(m.Member.Name);
                return m;
            }
            else if (m.Expression != null && m.Expression.NodeType == ExpressionType.MemberAccess) {
                var v = GetMemberValue(m);
                sb.Append(v);
                //this.Visit(m.Expression);
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
            if(result is String || result is DateTime)
            {
                result = ("'" + result.ToString() + "'");
            }
            if (result is Boolean)
            {
                result = (bool.Parse(result.ToString()) ? 1 : 0);
            }
            return result;
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
