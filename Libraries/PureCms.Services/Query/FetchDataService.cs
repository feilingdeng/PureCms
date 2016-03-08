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
            _queryExpression = queryExpression;
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
                filterList.Add(filter.FilterOperator == LogicalOperator.And ? "AND" : "OR");
            }
            bool flag = false;
            filterList.Add("(");
            foreach (var cd in filter.Conditions)
            {
                if (flag)
                {
                    filterList.Add(filter.FilterOperator == LogicalOperator.And ? "AND" : "OR");
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
                case ConditionOperator.Equal:
                    condition = string.Format("{0}='{1}'", attrName, value);
                    break;
                case ConditionOperator.EqualUserId:
                    condition = string.Format("{0}='{1}'", attrName, value);
                    break;
                case ConditionOperator.NotEqual:
                    condition = string.Format("{0}<>'{1}'", attrName, value);
                    break;
                case ConditionOperator.NotEqualUserId:
                    condition = string.Format("{0}<>'{1}'", attrName, value);
                    break;
                case ConditionOperator.NotEqualBusinessId:
                    condition = string.Format("{0}<>'{1}'", attrName, value);
                    break;
                case ConditionOperator.BeginsWith:
                    condition = string.Format("{0} LIKE '{1}%'", attrName, value);
                    break;
                case ConditionOperator.DoesNotBeginWith:
                    condition = string.Format("{0} NOT LIKE '{1}%'", attrName, value);
                    break;
                case ConditionOperator.DoesNotContain:
                    condition = string.Format("{0} NOT LIKE'%{1}%'", attrName, value);
                    break;
                case ConditionOperator.DoesNotEndWith:
                    condition = string.Format("{0} NOT LIKE '%{1}'", attrName, value);
                    break;
                case ConditionOperator.EndsWith:
                    condition = string.Format("{0} LIKE '%{1}'", attrName, value);
                    break;
                case ConditionOperator.GreaterEqual:
                    condition = string.Format("{0}>='{1}'", attrName, value);
                    break;
                case ConditionOperator.GreaterThan:
                    condition = string.Format("{0}>'{1}'", attrName, value);
                    break;
                case ConditionOperator.LessEqual:
                    condition = string.Format("{0}<='{1}'", attrName, value);
                    break;
                case ConditionOperator.LessThan:
                    condition = string.Format("{0}<'{1}'", attrName, value);
                    break;
                case ConditionOperator.Last7Days:
                    condition = string.Format("(DATEDIFF(DAY, {0}, GETDATE())>0 AND DATEDIFF(DAY, {0}, GETDATE())<=7)", attrName);
                    break;
                case ConditionOperator.LastMonth:
                    condition = string.Format("DATEDIFF(MONTH, {0}, GETDATE())=1", attrName);
                    break;
                case ConditionOperator.LastWeek:
                    condition = string.Format("DATEDIFF(WEEK, {0}, GETDATE())=1", attrName);
                    break;
                case ConditionOperator.LastXDays:
                    condition = string.Format("(DATEDIFF(DAY, {0}, GETDATE())>0 AND DATEDIFF(DAY, {0}, GETDATE())<={1})", attrName, value);
                    break;
                case ConditionOperator.LastXHours:
                    condition = string.Format("(DATEDIFF(HH, {0}, GETDATE())>0 AND DATEDIFF(HH, {0}, GETDATE())<={1})", attrName, value);
                    break;
                case ConditionOperator.LastXMonths:
                    condition = string.Format("(DATEDIFF(MONTH, {0}, GETDATE())>0 AND DATEDIFF(MONTH, {0}, GETDATE())<={1})", attrName, value);
                    break;
                case ConditionOperator.LastXWeeks:
                    condition = string.Format("(DATEDIFF(WEEK, {0}, GETDATE())>0 AND DATEDIFF(WEEK, {0}, GETDATE())<={1})", attrName, value);
                    break;
                case ConditionOperator.LastXYears:
                    condition = string.Format("(DATEDIFF(YEAR, {0}, GETDATE())>0 AND DATEDIFF(YEAR, {0}, GETDATE())<={1})", attrName, value);
                    break;
                case ConditionOperator.LastYear:
                    condition = string.Format("DATEDIFF(YEAR, {0}, GETDATE())=1", attrName);
                    break;
                case ConditionOperator.Next7Days:
                    condition = string.Format("(DATEDIFF(DAY, GETDATE(), {0})>0 AND DATEDIFF(DAY, GETDATE(), {0})<=7)", attrName);
                    break;
                case ConditionOperator.NextMonth:
                    condition = string.Format("DATEDIFF(MONTH, GETDATE(), {0})=1", attrName);
                    break;
                case ConditionOperator.NextWeek:
                    condition = string.Format("DATEDIFF(WEEK, GETDATE(), {0})=1", attrName);
                    break;
                case ConditionOperator.NextXDays:
                    condition = string.Format("(DATEDIFF(DAY, GETDATE(), {0})>0 AND DATEDIFF(DAY, GETDATE(), {0})<={1})", attrName, value);
                    break;
                case ConditionOperator.NextXHours:
                    condition = string.Format("(DATEDIFF(HOUR, GETDATE(), {0})>0 AND DATEDIFF(HOUR, GETDATE(), {0})<={1})", attrName, value);
                    break;
                case ConditionOperator.NextXMonths:
                    condition = string.Format("(DATEDIFF(MONTH, GETDATE(), {0})>0 AND DATEDIFF(MONTH, GETDATE(), {0})<={1})", attrName, value);
                    break;
                case ConditionOperator.NextXWeeks:
                    condition = string.Format("(DATEDIFF(WEEK, GETDATE(), {0})>0 AND DATEDIFF(WEEK, GETDATE(), {0})<={1})", attrName, value);
                    break;
                case ConditionOperator.NextXYears:
                    condition = string.Format("(DATEDIFF(YEAR, GETDATE(), {0})>0 AND DATEDIFF(YEAR, GETDATE(), {0})<={1})", attrName, value);
                    break;
                case ConditionOperator.NextYear:
                    condition = string.Format("DATEDIFF(YEAR, GETDATE(), {0})=1", attrName);
                    break;
                case ConditionOperator.Today:
                    condition = string.Format("DATEDIFF(DAY,{0},GETDATE())=0", attrName);
                    break;
                case ConditionOperator.NotBetween:
                    condition = string.Format("{0} NOT BETWEEN '{1}' AND '{2}'", attrName, conditionNode.Values[0], conditionNode.Values[1]);
                    break;
                case ConditionOperator.OlderThanXMonths:
                    condition = string.Format("DATEDIFF(MONTH, {0}, GETDATE())>={1}", attrName, value);
                    break;
                case ConditionOperator.On:
                    condition = string.Format("{0}='{1}'", attrName, value);
                    break;
                case ConditionOperator.NotOn:
                    condition = string.Format("{0}<>'{1}'", attrName, value);
                    break;
                case ConditionOperator.OnOrAfter:
                    condition = string.Format("DATEDIFF(DAY, '{1}', {0})>=0", attrName, value);
                    break;
                case ConditionOperator.OnOrBefore:
                    condition = string.Format("DATEDIFF(DAY, '{1}', {0})<=0", attrName, value);
                    break;
                case ConditionOperator.ThisMonth:
                    condition = string.Format("DATEDIFF(MONTH, {0}, GETDATE())=0", attrName);
                    break;
                case ConditionOperator.ThisWeek:
                    condition = string.Format("DATEDIFF(WEEK, {0}, GETDATE())=0", attrName);
                    break;
                case ConditionOperator.ThisYear:
                    condition = string.Format("DATEDIFF(YEAR, {0}, GETDATE())=0", attrName);
                    break;
                case ConditionOperator.Tomorrow:
                    condition = string.Format("DATEDIFF(MONTH, {0}, GETDATE())=-1", attrName);
                    break;
                case ConditionOperator.Yesterday:
                    condition = string.Format("DATEDIFF(MONTH, {0}, GETDATE())=1", attrName);
                    break;
                case ConditionOperator.NotIn:
                    condition = string.Format("{0} NOT IN({1})", attrName, conditionNode.Values.CollectionToString(",", "'"));
                    break;
                case ConditionOperator.In:
                    condition = string.Format("{0} IN({1})", attrName, conditionNode.Values.CollectionToString(",", "'"));
                    break;
                case ConditionOperator.Like:
                    condition = string.Format("{0} LIKE '%{1}%'", attrName, value);
                    break;
                case ConditionOperator.Contains:
                    condition = string.Format("{0} LIKE '%{1}%'", attrName, value);
                    break;
                case ConditionOperator.NotLike:
                    condition = string.Format("{0} NOT LIKE '%{1}%'", attrName, value);
                    break;
                case ConditionOperator.NotNull:
                    condition = string.Format("{0} IS NOT NULL", attrName);
                    break;
                case ConditionOperator.Null:
                    condition = string.Format("{0} IS NULL", attrName);
                    break;
                case ConditionOperator.EqualBusinessId:
                    condition = string.Format("{0}='{1}'", attrName, value);
                    break;
                default:
                    break;
            }

            return condition;
        }
        private string GetLinkType(JoinOperator joinOperator)
        {
            string join = "LEFT JOIN";
            switch (joinOperator)
            {
                case JoinOperator.Inner:
                    join = "INNER JOIN";
                    break;
                case JoinOperator.LeftOuter:
                    join = "LEFT JOIN";
                    break;
                case JoinOperator.Natural:
                    join = "INNER JOIN";
                    break;
            }
            return join;
        }
    }
}
