using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PureCms.Core.Query
{
    public sealed class FilterExpression
    {
        private LogicalOperator _filterOperator;
        private List<ConditionExpression> _conditions;
        private List<FilterExpression> _filters;
        [DataMember]
        public LogicalOperator FilterOperator
        {
            get
            {
                return this._filterOperator;
            }
            set
            {
                this._filterOperator = value;
            }
        }
        [DataMember]
        public List<ConditionExpression> Conditions
        {
            get
            {
                if (this._conditions == null)
                {
                    this._conditions = new List<ConditionExpression>();
                }
                return this._conditions;
            }
            private set
            {
                this._conditions = value;
            }
        }
        [DataMember]
        public List<FilterExpression> Filters
        {
            get
            {
                if (this._filters == null)
                {
                    this._filters = new List<FilterExpression>();
                }
                return this._filters;
            }
            private set
            {
                this._filters = value;
            }
        }
        public FilterExpression()
        {
        }
        public FilterExpression(LogicalOperator filterOperator)
        {
            this.FilterOperator = filterOperator;
        }
        public void AddCondition(string attributeName, ConditionOperator conditionOperator, params object[] values)
        {
            this.Conditions.Add(new ConditionExpression(attributeName, conditionOperator, values));
        }
        public void AddCondition(ConditionExpression condition)
        {
            this.Conditions.Add(condition);
        }
        public FilterExpression AddFilter(LogicalOperator logicalOperator)
        {
            FilterExpression filterExpression = new FilterExpression();
            filterExpression.FilterOperator = logicalOperator;
            this.Filters.Add(filterExpression);
            return filterExpression;
        }
        public void AddFilter(FilterExpression childFilter)
        {
            if (childFilter != null)
            {
                this.Filters.Add(childFilter);
            }
        }
    }
}
