using System.Runtime.Serialization;

namespace PureCms.Core.Query
{
    public sealed class OrderExpression
    {
        private string _attributeName;
        private OrderType _orderType;
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
        public OrderType OrderType
        {
            get
            {
                return this._orderType;
            }
            set
            {
                this._orderType = value;
            }
        }
        public OrderExpression()
        {
        }
        public OrderExpression(string attributeName, OrderType orderType)
        {
            this._attributeName = attributeName;
            this._orderType = orderType;
        }
    }
}
