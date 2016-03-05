using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PureCms.Core.Query
{
    public sealed class ConditionExpression
    {
        private string _attributeName;
        private ConditionOperator _conditionOperator;
        private List<object> _values;
        [DataMember]
        public string AttributeName
        {
            get
            {
                return this._attributeName;
            }
            set
            {
                this._attributeName = value;
            }
        }
        [DataMember]
        public ConditionOperator Operator
        {
            get
            {
                return this._conditionOperator;
            }
            set
            {
                this._conditionOperator = value;
            }
        }
        [DataMember]
        public List<object> Values
        {
            get
            {
                if (this._values == null)
                {
                    this._values = new List<object>();
                }
                return this._values;
            }
            private set
            {
                this._values = value;
            }
        }
        public ConditionExpression()
        {
        }
        public ConditionExpression(string attributeName, ConditionOperator conditionOperator, params object[] values)
        {
            this._attributeName = attributeName;
            this._conditionOperator = conditionOperator;
            if (values != null)
            {
                this._values = new List<object>(values);
            }
        }
        public ConditionExpression(string attributeName, ConditionOperator conditionOperator, object value) : this(attributeName, conditionOperator, new object[]
        {
            value
        })
        {
        }
        public ConditionExpression(string attributeName, ConditionOperator conditionOperator) : this(attributeName, conditionOperator, new object[0])
        {
        }
        public ConditionExpression(string attributeName, ConditionOperator conditionOperator, ICollection values)
        {
            this._attributeName = attributeName;
            this._conditionOperator = conditionOperator;
            if (values != null)
            {
                this._values = new List<object>();
                foreach (object current in values)
                {
                    this._values.Add(current);
                }
            }
        }
    }
}
