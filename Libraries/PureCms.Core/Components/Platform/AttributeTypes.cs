using PureCms.Core.Query;
using System;
using System.Collections.Generic;

namespace PureCms.Core.Components.Platform
{
    public static class AttributeTypes
    {
        static AttributeTypes()
        {
            StringOperators = null;
            NumberOperators = null;
            DateTimeOperators = null;
            LookUpOperators = null;
            PickListOperators = null;
        }
        private static List<KeyValuePair<string, string>> _numberOperators = new List<KeyValuePair<string, string>>();
        public static List<KeyValuePair<string, string>> NumberOperators
        {
            get
            {
                return _numberOperators;
            }
            private set
            {
                _numberOperators.Add(new KeyValuePair<string, string>("等于", GetOperatorName(ConditionOperator.Equal)));
                _numberOperators.Add(new KeyValuePair<string, string>("不等于", GetOperatorName(ConditionOperator.NotEqual)));
                _numberOperators.Add(new KeyValuePair<string, string>("大于等于", GetOperatorName(ConditionOperator.GreaterEqual)));
                _numberOperators.Add(new KeyValuePair<string, string>("大于", GetOperatorName(ConditionOperator.GreaterThan)));
                _numberOperators.Add(new KeyValuePair<string, string>("小于等于", GetOperatorName(ConditionOperator.LessEqual)));
                _numberOperators.Add(new KeyValuePair<string, string>("小于", GetOperatorName(ConditionOperator.LessThan)));
                _numberOperators.Add(new KeyValuePair<string, string>("包含数据", GetOperatorName(ConditionOperator.NotNull)));
                _numberOperators.Add(new KeyValuePair<string, string>("不包含数据", GetOperatorName(ConditionOperator.Null)));
            }
        }
        private static List<KeyValuePair<string, string>> _stringOperators = new List<KeyValuePair<string, string>>();
        public static List<KeyValuePair<string, string>> StringOperators
        {
            get
            {
                return _stringOperators;
            }
            private set
            {
                _stringOperators.Add(new KeyValuePair<string, string>("等于", GetOperatorName(ConditionOperator.Equal)));
                _stringOperators.Add(new KeyValuePair<string, string>("不等于", GetOperatorName(ConditionOperator.NotEqual)));
                _stringOperators.Add(new KeyValuePair<string, string>("包含", GetOperatorName(ConditionOperator.Contains)));
                _stringOperators.Add(new KeyValuePair<string, string>("不包含", GetOperatorName(ConditionOperator.DoesNotContain)));
                _stringOperators.Add(new KeyValuePair<string, string>("开头等于", GetOperatorName(ConditionOperator.BeginsWith)));
                _stringOperators.Add(new KeyValuePair<string, string>("结尾等于", GetOperatorName(ConditionOperator.EndsWith)));
                _stringOperators.Add(new KeyValuePair<string, string>("开头不等于", GetOperatorName(ConditionOperator.DoesNotBeginWith)));
                _stringOperators.Add(new KeyValuePair<string, string>("结尾不等于", GetOperatorName(ConditionOperator.DoesNotEndWith)));
                _stringOperators.Add(new KeyValuePair<string, string>("包含数据", GetOperatorName(ConditionOperator.NotNull)));
                _stringOperators.Add(new KeyValuePair<string, string>("不包含数据", GetOperatorName(ConditionOperator.Null)));
            }
        }

        private static List<KeyValuePair<string, string>> _datetimeOperators = new List<KeyValuePair<string, string>>();
        public static List<KeyValuePair<string, string>> DateTimeOperators
        {
            get
            {
                return _datetimeOperators;
            }
            private set
            {
                _datetimeOperators.Add(new KeyValuePair<string, string>("等于", GetOperatorName(ConditionOperator.Equal)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("不等于", GetOperatorName(ConditionOperator.NotEqual)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("大于等于", GetOperatorName(ConditionOperator.GreaterEqual)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("大于", GetOperatorName(ConditionOperator.GreaterThan)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("小于等于", GetOperatorName(ConditionOperator.LessEqual)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("小于", GetOperatorName(ConditionOperator.LessThan)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("往前7天", GetOperatorName(ConditionOperator.Last7Days)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("上一周", GetOperatorName(ConditionOperator.LastWeek)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("上个月", GetOperatorName(ConditionOperator.LastMonth)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("上一年", GetOperatorName(ConditionOperator.LastYear)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("最近X小时", GetOperatorName(ConditionOperator.LastXHours)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("最近X天", GetOperatorName(ConditionOperator.LastXDays)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("最近X周", GetOperatorName(ConditionOperator.LastXWeeks)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("最近X个月", GetOperatorName(ConditionOperator.LastXMonths)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("最近X年", GetOperatorName(ConditionOperator.LastXYears)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("往后7天", GetOperatorName(ConditionOperator.Next7Days)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("下一周", GetOperatorName(ConditionOperator.NextWeek)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("下个月", GetOperatorName(ConditionOperator.NextMonth)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("明年", GetOperatorName(ConditionOperator.NextYear)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("往后X小时", GetOperatorName(ConditionOperator.NextXHours)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("往后X天", GetOperatorName(ConditionOperator.NextXDays)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("往后X个月", GetOperatorName(ConditionOperator.NextXMonths)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("往后X周", GetOperatorName(ConditionOperator.NextXWeeks)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("往后X年", GetOperatorName(ConditionOperator.NextXYears)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("X个月以前", GetOperatorName(ConditionOperator.OlderThanXMonths)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("晚于(包含当天)", GetOperatorName(ConditionOperator.OnOrAfter)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("早于(包含当天)", GetOperatorName(ConditionOperator.OnOrBefore)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("本周", GetOperatorName(ConditionOperator.ThisWeek)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("本月", GetOperatorName(ConditionOperator.ThisMonth)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("本年", GetOperatorName(ConditionOperator.ThisYear)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("今天", GetOperatorName(ConditionOperator.Today)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("明天", GetOperatorName(ConditionOperator.Tomorrow)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("昨天", GetOperatorName(ConditionOperator.Yesterday)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("包含数据", GetOperatorName(ConditionOperator.NotNull)));
                _datetimeOperators.Add(new KeyValuePair<string, string>("不包含数据", GetOperatorName(ConditionOperator.Null)));
            }
        }


        private static List<KeyValuePair<string, string>> _lookupOperators = new List<KeyValuePair<string, string>>();
        public static List<KeyValuePair<string, string>> LookUpOperators
        {
            get
            {
                return _lookupOperators;
            }
            private set
            {
                _lookupOperators.Add(new KeyValuePair<string, string>("等于", GetOperatorName(ConditionOperator.Equal)));
                _lookupOperators.Add(new KeyValuePair<string, string>("不等于", GetOperatorName(ConditionOperator.NotEqual)));
                _lookupOperators.Add(new KeyValuePair<string, string>("开头等于", GetOperatorName(ConditionOperator.BeginsWith)));
                _lookupOperators.Add(new KeyValuePair<string, string>("结尾等于", GetOperatorName(ConditionOperator.EndsWith)));
                _lookupOperators.Add(new KeyValuePair<string, string>("开头不等于", GetOperatorName(ConditionOperator.DoesNotBeginWith)));
                _lookupOperators.Add(new KeyValuePair<string, string>("结尾不等于", GetOperatorName(ConditionOperator.DoesNotEndWith)));
                _lookupOperators.Add(new KeyValuePair<string, string>("包含", GetOperatorName(ConditionOperator.Contains)));
                _lookupOperators.Add(new KeyValuePair<string, string>("不包含", GetOperatorName(ConditionOperator.DoesNotContain)));
                _lookupOperators.Add(new KeyValuePair<string, string>("包含数据", GetOperatorName(ConditionOperator.NotNull)));
                _lookupOperators.Add(new KeyValuePair<string, string>("不包含数据", GetOperatorName(ConditionOperator.Null)));
            }
        }

        private static List<KeyValuePair<string, string>> _pickListOperators = new List<KeyValuePair<string, string>>();
        public static List<KeyValuePair<string, string>> PickListOperators
        {
            get
            {
                return _pickListOperators;
            }
            private set
            {
                _pickListOperators.Add(new KeyValuePair<string, string>("等于", GetOperatorName(ConditionOperator.Equal)));
                _pickListOperators.Add(new KeyValuePair<string, string>("不等于", GetOperatorName(ConditionOperator.NotEqual)));
                _pickListOperators.Add(new KeyValuePair<string, string>("开头等于", GetOperatorName(ConditionOperator.BeginsWith)));
                _pickListOperators.Add(new KeyValuePair<string, string>("结尾等于", GetOperatorName(ConditionOperator.EndsWith)));
                _pickListOperators.Add(new KeyValuePair<string, string>("开头不等于", GetOperatorName(ConditionOperator.DoesNotBeginWith)));
                _pickListOperators.Add(new KeyValuePair<string, string>("结尾不等于", GetOperatorName(ConditionOperator.DoesNotEndWith)));
                _pickListOperators.Add(new KeyValuePair<string, string>("包含", GetOperatorName(ConditionOperator.Contains)));
                _pickListOperators.Add(new KeyValuePair<string, string>("不包含", GetOperatorName(ConditionOperator.DoesNotContain)));
                _pickListOperators.Add(new KeyValuePair<string, string>("包含数据", GetOperatorName(ConditionOperator.NotNull)));
                _pickListOperators.Add(new KeyValuePair<string, string>("不包含数据", GetOperatorName(ConditionOperator.Null)));
            }
        }

        private static string GetOperatorName(ConditionOperator op)
        {
            return Enum.GetName(typeof(ConditionOperator), op);
        }
    }
}
