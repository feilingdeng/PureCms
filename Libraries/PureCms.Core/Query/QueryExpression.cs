using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PureCms.Core.Query
{
    public sealed class QueryExpression
    {
        private ColumnSet _columnSet;
        private string _entityName;
        private bool _distinct;
        private bool _noLock;
        private PagingInfo _pageInfo;
        private List<LinkEntity> _linkEntities;
        private FilterExpression _criteria;
        private List<OrderExpression> _orders;
        [DataMember]
        public bool Distinct
        {
            get
            {
                return this._distinct;
            }
            set
            {
                this._distinct = value;
            }
        }
        [DataMember(Order = 50)]
        public bool NoLock
        {
            get
            {
                return this._noLock;
            }
            set
            {
                this._noLock = value;
            }
        }
        [DataMember]
        public PagingInfo PageInfo
        {
            get
            {
                return this._pageInfo;
            }
            set
            {
                this._pageInfo = value;
            }
        }
        [DataMember]
        public List<LinkEntity> LinkEntities
        {
            get
            {
                if (this._linkEntities == null)
                {
                    this._linkEntities = new List<LinkEntity>();
                }
                return this._linkEntities;
            }
            private set
            {
                this._linkEntities = value;
            }
        }
        [DataMember]
        public FilterExpression Criteria
        {
            get
            {
                return this._criteria;
            }
            set
            {
                this._criteria = value;
            }
        }
        [DataMember]
        public List<OrderExpression> Orders
        {
            get
            {
                if (this._orders == null)
                {
                    this._orders = new List<OrderExpression>();
                }
                return this._orders;
            }
            private set
            {
                this._orders = value;
            }
        }
        [DataMember]
        public string EntityName
        {
            get
            {
                return this._entityName;
            }
            set
            {
                this._entityName = value;
            }
        }
        [DataMember]
        public ColumnSet ColumnSet
        {
            get
            {
                return this._columnSet;
            }
            set
            {
                this._columnSet = value;
            }
        }
        public QueryExpression() : this(null)
        {
        }
        public QueryExpression(string entityName)
        {
            this._entityName = entityName;
            this._criteria = new FilterExpression();
            this._pageInfo = new PagingInfo();
            this._columnSet = new ColumnSet();
        }
        public void AddOrder(string attributeName, OrderType orderType)
        {
            this.Orders.Add(new OrderExpression(attributeName, orderType));
        }
        public LinkEntity AddLink(string linkToEntityName, string linkFromAttributeName, string linkToAttributeName)
        {
            return this.AddLink(linkToEntityName, linkFromAttributeName, linkToAttributeName, JoinOperator.Inner);
        }
        public LinkEntity AddLink(string linkToEntityName, string linkFromAttributeName, string linkToAttributeName, JoinOperator joinOperator)
        {
            LinkEntity linkEntity = new LinkEntity(this.EntityName, linkToEntityName, linkFromAttributeName, linkToAttributeName, joinOperator);
            this.LinkEntities.Add(linkEntity);
            return linkEntity;
        }
    }
}
