using PureCms.Core.Domain;
using PureCms.Utilities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PureCms.Core.Context
{
    /// <summary>
    /// 更新上下文
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UpdateContext<T,TQ> where T : BaseEntity where TQ : class,new()
    {
        private List<KeyValuePair<string, object>> _sets = new List<KeyValuePair<string, object>>();
        public List<KeyValuePair<string, object>> Sets
        {
            get
            {
                return _sets;
            }
            set
            {
                _sets = value;
            }
        }
        private TQ _q = new TQ();
        public TQ QueryContext
        {
            get { return _q; }
            set { _q = value; }
        }
        public UpdateContext<T, TQ> Filter(Expression<Func<TQ, object>> q)
        {
            q.Compile().Invoke(QueryContext);
            return this;
        }

        public UpdateContext<T,TQ> Set(Expression<Func<T, object>> fieldPath, object value)
        {
            var field = ExpressionHelper.GetPropertyName<T>(fieldPath);
            _sets.Add(new KeyValuePair<string, object>(field, value));
            return this;
        }
        public UpdateContext<T,TQ> Set(params Action<T>[] modifier)
        {
            foreach (var item in modifier)
            {
            //var field = ExpressionHelper.GetPropertyName<T>(item);
            //_sets.Add(new KeyValuePair<string, object>(field, value));
                
            }
            return this;
        }
    }
}
