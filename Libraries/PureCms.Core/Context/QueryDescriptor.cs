using PureCms.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace PureCms.Core.Context
{

    public class QueryDescriptor<T>
        where T : new()//class
    {
        private ExpressionResolver _resolver = new ExpressionResolver();
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
        public QueryDescriptor<T> Take(int take)
        {
            _topCount = take;
            return this;
        }
        /// <summary>
        /// 设置分页信息
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public QueryDescriptor<T> Page(int currentPage, int pageSize)
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
            return this;
        }
        /// <summary>
        /// 设置排序信息
        /// </summary>
        /// <param name="sorts"></param>
        /// <returns></returns>
        public QueryDescriptor<T> Sort(params SortDescriptor<T>[] sorts)
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
            return this;
        }
        /// <summary>
        /// 设置排序信息
        /// </summary>
        /// <param name="sorts"></param>
        /// <returns></returns>
        public QueryDescriptor<T> Sort(params Func<SortDescriptor<T>, SortDescriptor<T>>[] sorts)
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
            return this;
        }
        /// <summary>
        /// 设置过滤条件
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        //public TChild Where(Expression<Func<TChild, object>> field, object value)
        //{
        //    var name = ExpressionHelper.GetPropertyName<TChild>(field);
        //    Type t = typeof(TChild);
        //    t.SetPropertyValue(GetConditionContainer(), name, value);

        //    return this as TChild;
        //}
        public QueryDescriptor<T> Where(FilterContainer<T> filter)
        {
            QueryText = filter.QueryText;
            Parameters = filter.Parameters;
            return this;
        }
        /// <summary>
        /// 设置过滤条件
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public QueryDescriptor<T> Where(Expression<Func<T, bool>> predicate)
        {
            _resolver.ResolveToSql(predicate);
            QueryText = _resolver.QueryText;
            Parameters = _resolver.Arguments;//.Argument.Select(x => new QueryParameter(x.Key,x.Value)).ToList();

            return this;
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
        }
        public QueryDescriptor<T> Select(params Expression<Func<T, object>>[] fields)
        {
            if (_columns == null)
            {
                _columns = new List<string>();
            }
            foreach (var item in fields)
            {
                if (item.Body is NewExpression)
                {
                    var m = (item.Body as NewExpression).Members;
                    _columns.AddRange(m.AsEnumerable().Select(f => f.Name));
                }
                else if (item.Body is MemberExpression)
                {
                    _columns.Add((item.Body as MemberExpression).Member.Name);
                }
                else if (item.Body is UnaryExpression)
                {
                    var me = (item.Body as UnaryExpression).Operand as MemberExpression;
                    _columns.Add(me.Member.Name);
                }
            }
            return this;
        }
    }

    public class FilterContainer<T> where T : new()//class
    {
        //private QueryTranslator _translator = new QueryTranslator();
        private StringBuilder _queryText = new StringBuilder();
        private List<QueryParameter> _parameters = new List<QueryParameter>();
        private ExpressionResolver _resolver = new ExpressionResolver();

        public List<QueryParameter> Parameters
        {
            get
            {
                return this._parameters;
            }
        }
        public string QueryText
        {
            get
            {
                return _queryText.ToString();
            }
        }
        public FilterContainer<T> And(Expression<Func<T, bool>> predicate)
        {
            if (_queryText.Length > 0)
            {
                _queryText.Append(" AND ");
            }
            //_queryText.Append(_translator.Translate(predicate));
            //_parameters = _translator.Parameters;
            _queryText.Append(_resolver.ResolveToSql(predicate));
            _parameters = _resolver.Arguments;//.Argument.Select(x => new QueryParameter(x.Key, x.Value)).ToList();
            return this;
        }
        public FilterContainer<T> Or(Expression<Func<T, bool>> predicate)
        {
            if (_queryText.Length > 0)
            {
                _queryText.Append(" OR ");
            }
            //_queryText.Append(_translator.Translate(predicate));
            //_parameters = _translator.Parameters;
            _queryText.Append(_resolver.ResolveToSql(predicate));
            _parameters = _resolver.Arguments;//.Argument.Select(x => new QueryParameter(x.Key, x.Value)).ToList();
            return this;
        }
    }
    /// <summary>
    /// 表达式参数
    /// </summary>
    public class QueryParameter
    {
        public QueryParameter() { }
        public QueryParameter(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }
        public string Name { get; set; }

        public object Value { get; set; }
    }
    //public class QueryTranslator : ExpressionVisitor
    //{
    //    private StringBuilder _queryText;

    //    private List<string> _parameters = new List<string>();
    //    private List<object> _values = new List<object>();
    //    //private List<QueryParameter> _params = new List<QueryParameter>();

    //    public List<QueryParameter> Parameters
    //    {
    //        get
    //        {
    //            List<QueryParameter> result = new List<QueryParameter>();
    //            int i = 0;
    //            foreach (var p in _parameters)
    //            {
    //                result.Add(new QueryParameter()
    //                {
    //                    Name = p
    //                    ,
    //                    Value = _values[i]
    //                });
    //                i++;
    //            }
    //            return result;
    //        }
    //    }

    //    public QueryTranslator()
    //    {
    //    }

    //    public string Translate(Expression expression)
    //    {
    //        this._queryText = new StringBuilder();
    //        this.Visit(expression);
    //        return this._queryText.ToString();
    //    }

    //    private void SetParamName(string key)
    //    {
    //        _parameters.Add(key + _parameters.Count);
    //        //_params.Add(new QueryParameter() { Name = key, Value = value });
    //    }

    //    private void SetParamValue(object value)
    //    {
    //        //var p = _params.Last();
    //        //p.Value = value;
    //        _values.Add(value);
    //    }

    //    private static Expression StripQuotes(Expression e)
    //    {
    //        while (e.NodeType == ExpressionType.Quote)
    //        {
    //            e = ((UnaryExpression)e).Operand;
    //        }
    //        return e;
    //    }

    //    protected override Expression VisitMethodCall(MethodCallExpression m)
    //    {
    //        if (m.Method.DeclaringType == typeof(MemberExtensions) && m.Method.Name == "Like")
    //        {
    //            this.Visit(m.Arguments[0]);
    //            //_queryText.Append(((MemberExpression)m.Arguments[0]).Member.Name);
    //            _queryText.Append(" LIKE '%");
    //            //_queryText.Append("@" + _values.Count);
    //            this.Visit(m.Arguments[1]);
    //            _queryText.Append("%' ");
    //            //_values.Add(((ConstantExpression)m.Arguments[1]).Value);
    //            return m;
    //        }
    //        if (m.Method.DeclaringType == typeof(MemberExtensions) && m.Method.Name == "In")
    //        {
    //            this.Visit(m.Arguments[0]);
    //            //_queryText.Append(((MemberExpression)m.Arguments[0]).Member.Name);
    //            _queryText.Append(" IN (");
    //            var newArray = ((NewArrayExpression)m.Arguments[1]).Expressions;
    //            var values = new List<object>();
    //            foreach (var n in newArray)
    //            {
    //                if (n is MemberExpression)
    //                {
    //                    var v = GetMemberValue(n as MemberExpression);
    //                    values.Add(v);
    //                }
    //                else if (n is ConstantExpression)
    //                {
    //                    values.Add(((ConstantExpression)n).Value);
    //                }
    //                else if (n is MethodCallExpression)
    //                {
    //                    var v = GetMemberValue(((MethodCallExpression)n).Object as MemberExpression);
    //                    values.Add(v);
    //                }
    //            }
    //            _queryText.Append("@" + _values.Count);
    //            _queryText.Append(") ");
    //            SetParamValue(values);
    //            return m;
    //        }
    //        if (m.Method.DeclaringType == typeof(MemberExtensions) && m.Method.Name == "IsNull")
    //        {
    //            if (m.Arguments[0] is MemberExpression)
    //            {
    //                this.Visit(m.Arguments[0]);
    //                //_queryText.Append(((MemberExpression)m.Arguments[0]).Member.Name);
    //            }
    //            else
    //            {
    //                this.Visit(m.Arguments[0]);
    //            }
    //            _queryText.Append(" IS NULL ");
    //            //_values.Add(null);
    //            SetParamValue(null);
    //            return m;
    //        }
    //        if (m.Method.DeclaringType == typeof(MemberExtensions) && m.Method.Name == "IsNotNull")
    //        {
    //            if (m.Arguments[0] is MemberExpression)
    //            {
    //                this.Visit(m.Arguments[0]);
    //                //_queryText.Append(((MemberExpression)m.Arguments[0]).Member.Name);
    //            }
    //            else
    //            {
    //                this.Visit(m.Arguments[0]);
    //            }
    //            _queryText.Append(" IS NOT NULL ");
    //            //_values.Add(null);
    //            SetParamValue(null);
    //            return m;
    //        }
    //        //update 方法
    //        //if (m.Method.DeclaringType == typeof(UpdateExtensions) && m.Method.Name == "Val")
    //        //{
    //        //    this.Visit(m.Arguments[0]);
    //        //    _queryText.Append("=");
    //        //    this.Visit(m.Arguments[1]);
    //        //    return m;
    //        //}
    //        throw new NotSupportedException(string.Format("方法{0}不支持", m.Method.Name));
    //    }

    //    protected override Expression VisitUnary(UnaryExpression u)
    //    {
    //        switch (u.NodeType)
    //        {
    //            case ExpressionType.Not:
    //                _queryText.Append(" NOT ");
    //                this.Visit(u.Operand);
    //                break;
    //            case ExpressionType.Convert:
    //                this.Visit(u.Operand);
    //                break;
    //            default:
    //                throw new NotSupportedException(string.Format("运算符{0}不支持", u.NodeType));
    //        }
    //        return u;
    //    }

    //    protected override Expression VisitBinary(BinaryExpression b)
    //    {
    //        _queryText.Append("(");
    //        this.Visit(b.Left);
    //        switch (b.NodeType)
    //        {
    //            case ExpressionType.And:
    //                _queryText.Append(" AND ");
    //                break;
    //            case ExpressionType.AndAlso:
    //                _queryText.Append(" AND ");
    //                break;
    //            case ExpressionType.Or:
    //                _queryText.Append(" OR ");
    //                break;
    //            case ExpressionType.OrElse:
    //                _queryText.Append(" OR ");
    //                break;
    //            case ExpressionType.Equal:
    //                _queryText.Append(" = ");
    //                break;
    //            case ExpressionType.NotEqual:
    //                _queryText.Append(" <> ");
    //                break;
    //            case ExpressionType.LessThan:
    //                _queryText.Append(" < ");
    //                break;
    //            case ExpressionType.LessThanOrEqual:
    //                _queryText.Append(" <= ");
    //                break;
    //            case ExpressionType.GreaterThan:
    //                _queryText.Append(" > ");
    //                break;
    //            case ExpressionType.GreaterThanOrEqual:
    //                _queryText.Append(" >= ");
    //                break;
    //            default:
    //                throw new NotSupportedException(string.Format("运算符{0}不支持", b.NodeType));
    //        }
    //        this.Visit(b.Right);
    //        _queryText.Append(")");
    //        return b;
    //    }

    //    protected override Expression VisitConstant(ConstantExpression c)
    //    {
    //        _queryText.Append("@" + _values.Count);
    //        SetParamValue(c.Value);
    //        return c;
    //    }

    //    protected override Expression VisitMember(MemberExpression m)
    //    {
    //        if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter)
    //        {
    //            _queryText.Append(m.Member.Name);
    //            SetParamName(m.Member.Name);
    //            return m;
    //        }
    //        else if (m.Expression != null && (m.Expression.NodeType == ExpressionType.MemberAccess || m.Expression.NodeType == ExpressionType.Constant))
    //        {
    //            var v = GetMemberValue(m);
    //            //_queryText.Append(v);
    //            _queryText.Append("@" + _values.Count);
    //            SetParamValue(v);
    //            //SetParam(m.Member.Name + Parameters.Count, v);
    //            return m;
    //        }
    //        throw new NotSupportedException(string.Format("成员{0}不支持", m.Member.Name));
    //    }


    //    // 获取属性值
    //    private Object GetMemberValue(MemberExpression memberExpression)
    //    {
    //        MemberInfo memberInfo;
    //        Object obj;
    //        Object result;

    //        if (memberExpression == null)
    //            throw new ArgumentNullException("memberExpression");


    //        if (memberExpression.Expression is ConstantExpression)
    //            obj = ((ConstantExpression)memberExpression.Expression).Value;
    //        else if (memberExpression.Expression is MemberExpression)
    //            obj = GetMemberValue((MemberExpression)memberExpression.Expression);
    //        else
    //            throw new NotSupportedException("Expression type not supported: "
    //                + memberExpression.Expression.GetType().FullName);

    //        memberInfo = memberExpression.Member;
    //        if (memberInfo is PropertyInfo)
    //        {
    //            PropertyInfo property = (PropertyInfo)memberInfo;

    //            result = property.GetValue(obj, null);
    //        }
    //        else if (memberInfo is FieldInfo)
    //        {
    //            FieldInfo field = (FieldInfo)memberInfo;
    //            result = field.GetValue(obj);
    //        }
    //        else
    //        {
    //            throw new NotSupportedException("Member type not supported: "
    //                + memberInfo.GetType().FullName);
    //        }
    //        return result;
    //    }
    //}

        /// <summary>
        /// 表达式扩展方法
        /// </summary>
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
}
