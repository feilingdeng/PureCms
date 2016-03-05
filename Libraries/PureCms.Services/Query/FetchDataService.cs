using PureCms.Core.Query;
using System.Collections.Generic;
using System.Text;

namespace PureCms.Services.Query
{
    public class FetchDataService
    {
        public List<KeyValuePair<string, object>> EntityList;
        public List<KeyValuePair<string, object>> AttributeList;
        QueryExpression _queryExpression = new QueryExpression();

        public FetchDataService() {
            _queryExpression.EntityName = "users";
            _queryExpression.Distinct = false;
            _queryExpression.NoLock = true;
            _queryExpression.AddOrder("createdon", OrderType.Descending);
            _queryExpression.AddOrder("username", OrderType.Ascending);
            _queryExpression.PageInfo = new PagingInfo() { PageNumber = 1, ReturnTotalRecordCount = true };
            _queryExpression.ColumnSet = new ColumnSet("userid", "username", "createdon");
            FilterExpression filter = new FilterExpression(LogicalOperator.Or);
            filter.AddCondition("username", ConditionOperator.Like, "u");
            filter.AddCondition("gender", ConditionOperator.In, new object[] { 1, 2 });
            //filter.AddFilter(LogicalOperator.Or);
            //filter.AddCondition("isdeleted", ConditionOperator.Equal, true);

            FilterExpression filter2 = new FilterExpression(LogicalOperator.Or);
            filter2.AddCondition("isdeleted", ConditionOperator.Equal, 1);
            filter2.AddCondition("isactive", ConditionOperator.Equal, 0);
            filter.AddFilter(filter2);

            _queryExpression.Criteria = filter;

            LinkEntity roleEntity = _queryExpression.AddLink("roles", "roleid", "roleid");
            roleEntity.FromEntityAlias = "users0";
            roleEntity.EntityAlias = "roles1";
            roleEntity.Columns = new ColumnSet("name");
            filter = new FilterExpression(LogicalOperator.And);
            filter.AddCondition("name", ConditionOperator.Like, "经理");
            roleEntity.LinkCriteria = filter;
        }
        public string ToJsonString(QueryExpression queryExpression)
        {
            return queryExpression.SerializeToJson();
        }
        public QueryExpression ToQueryExpression(string jsonConfig)
        {
            QueryExpression result = new QueryExpression();
            return result.DeserializeFromJson(jsonConfig);
        }
        public string ToSqlString(QueryExpression queryExpression)
        {
            StringBuilder sqlString = new StringBuilder();
            List<string> tableList = new List<string>();
            List<string> attrList = new List<string>();
            List<string> filterList = new List<string>();
            List<string> orderList = new List<string>();
            string mainEntityAlias = _queryExpression.EntityName + tableList.Count;
            //tables
            tableList.Add(_queryExpression.EntityName + " as " + mainEntityAlias);
            //columns
            foreach (var column in _queryExpression.ColumnSet.Columns)
            {
                attrList.Add(mainEntityAlias + "." + column);
            }
            //filters
            ParseFilter(_queryExpression.Criteria, mainEntityAlias, ref filterList);
            //link entities
            foreach (var le in _queryExpression.LinkEntities)
            {
                ParseLinkEntity(le, ref tableList, ref attrList, ref filterList);
            }
            //orders
            foreach (var ord in _queryExpression.Orders)
            {
                orderList.Add(mainEntityAlias + "." + ord.AttributeName + " " + (ord.OrderType == OrderType.Descending ? " desc" : ""));
            }
            string tableStr = string.Join(" ", tableList);
            string attrStr = string.Join(",", attrList);
            string filterStr = string.Join(" ", filterList);
            string orderStr = string.Join(",", orderList);
            sqlString.AppendFormat("select {0} from {1} where {2} order by {3} ", attrStr, tableStr, filterStr, orderStr);
            return sqlString.ToString();
        }
        private void ParseLinkEntity(LinkEntity linkEntity, ref List<string> tableList, ref List<string> attrList, ref List<string> filterList)
        {
            string entityAlias = linkEntity.EntityAlias;
            foreach (var column in linkEntity.Columns.Columns)
            {
                attrList.Add(entityAlias + "." + column);
            }
            string tb = GetLinkType(linkEntity.JoinOperator) + " " + linkEntity.LinkToEntityName + " as " + entityAlias
                + " on " + entityAlias + "." + linkEntity.LinkToAttributeName + " = " + linkEntity.FromEntityAlias + "." + linkEntity.LinkFromAttributeName;
            tableList.Add(tb);

            ParseFilter(linkEntity.LinkCriteria, entityAlias, ref filterList);

            if (linkEntity.LinkEntities.IsNotNullOrEmpty())
            {
                foreach (var le in linkEntity.LinkEntities)
                {
                    ParseLinkEntity(le, ref tableList, ref attrList, ref filterList);
                }
            }
        }
        private void ParseFilter(FilterExpression filter, string entityAlias, ref List<string> filterList)
        {
            if (filterList.IsNotNullOrEmpty())
            {
                filterList.Add(filter.FilterOperator == LogicalOperator.And ? "and" : "or");
            }
            bool flag = false;
            filterList.Add("(");
            foreach (var cd in filter.Conditions)
            {
                if (flag)
                {
                    filterList.Add(filter.FilterOperator == LogicalOperator.And ? "and" : "or");
                }
                filterList.Add(MakeCondition(entityAlias, cd));
                flag = true;
            }
            if (filter.Filters.IsNotNullOrEmpty())
            {
                foreach (var item in filter.Filters)
                {
                    ParseFilter(item, entityAlias, ref filterList);
                }
            }
            filterList.Add(")");
        }
        private string MakeCondition(string entityAliaName, ConditionExpression conditionNode)
        {
            string condition = string.Empty;
            string attrName = entityAliaName + "." + conditionNode.AttributeName;
            string value = (conditionNode.Values != null) ? string.Join(",", conditionNode.Values) : string.Empty;

            switch (conditionNode.Operator)
            {
                case ConditionOperator.In:
                    var valueNodes = conditionNode.Values;
                    List<string> values = new List<string>();
                    foreach (var item in valueNodes)
                    {
                        values.Add("'" + item + "'");
                    }
                    string value2 = string.Join(",", values);
                    condition = attrName + " in(" + value2 + ")";
                    break;
                case ConditionOperator.Like:
                    condition = attrName + " like '%" + value + "%'";
                    break;
                case ConditionOperator.Equal:
                    condition = attrName + " = '" + value + "'";
                    break;
                case ConditionOperator.EqualUserId:
                    condition = attrName + " = '" + value + "'";
                    break;
                case ConditionOperator.Today:
                    condition = "datediff(d," + attrName + ",getdate())=0";
                    break;
                default:
                    break;
            }

            return condition;
        }
        private string GetLinkType(JoinOperator joinOperator)
        {
            string join = "left join";
            switch (joinOperator)
            {
                case JoinOperator.Inner:
                    join = "inner join";
                    break;
                case JoinOperator.LeftOuter:
                    join = "left join";
                    break;
                case JoinOperator.Natural:
                    join = "inner join";
                    break;
            }
            return join;
        }
    }
}
