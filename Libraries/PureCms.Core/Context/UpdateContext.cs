using PureCms.Core.Domain;
using PureCms.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PureCms.Core.Context
{
    /// <summary>
    /// 更新上下文
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UpdateContext<T> where T : BaseEntity
    {
        private ResolveExpression _resolver = new ResolveExpression();
        private string _queryText = string.Empty;
        public string QueryText { get { return this._queryText; } }
        private List<QueryParameter> _parameters = new List<QueryParameter>();
        public List<QueryParameter> Parameters { get { return this._parameters; } }
        private List<KeyValuePair<string, object>> _sets = new List<KeyValuePair<string, object>>();
        public List<KeyValuePair<string, object>> Sets
        {
            get
            {
                return _sets;
            }
        }

        public UpdateContext<T> Where(Expression<Func<T, bool>> predicate)
        {
            _resolver.ResolveToSql(predicate);
            this._queryText = _resolver.QueryText;
            this._parameters = _resolver.Argument.Select(x => new QueryParameter(x.Key, x.Value)).ToList();
            return this;
        }

        public UpdateContext<T> Set(Expression<Func<T, object>> fieldPath, object value)
        {
            var field = ExpressionHelper.GetPropertyName<T>(fieldPath);
            _sets.Add(new KeyValuePair<string, object>(field, value));
            return this;
        }
    }

    public static class UpdateExtensions
    {
        public const string Tips = "此方法只能用于本框架";

        public static void Val(this object member, object value)
        {
            throw new Exception(Tips);
        }
    }
}
