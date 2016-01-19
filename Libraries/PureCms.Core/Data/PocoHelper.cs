using PetaPoco;
using PureCms.Core;
using PureCms.Core.Context;
using PureCms.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PureCms.Core.Data
{
    public class PocoHelper
    {
        /// <summary>
        /// 格式化列名为：[表名].[列名]
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string FormatColumn(Type entityType, string name)
        {
            return FormatColumn(PetaPoco.Database.PocoData.ForType(entityType), name);
        }
        /// <summary>
        /// 格式化列名为：[表名].[列名]
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string FormatColumn(PetaPoco.Database.PocoData poco, string name)
        {
            Type entityType = poco.type;
            BindingFlags flag = BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance;
            var p = entityType.GetProperty(name, flag);
            var attrs = p.GetCustomAttributes(typeof(LinkEntityAttribute), true);
            if (attrs.Length > 0)
            {
                var leAttr = (LinkEntityAttribute)attrs[0];
                var linkTarget = leAttr.Target;
                var linkPoco = new Database.PocoData(linkTarget);
                var fieldName = (leAttr.SourceFieldName.IsNotEmpty() ? leAttr.SourceFieldName : name);
                name = "[" + linkPoco.TableInfo.TableName + "].[" + fieldName + "]";//关联表字段
            }
            else
            {
                name = "[" + poco.TableInfo.TableName + "].[" + name + "]";//主表字段
            }
            return name;
        }
        /// <summary>
        /// 获取查询列名
        /// </summary>
        /// <param name="poco"></param>
        /// <param name="columns"></param>
        /// <param name="isCount"></param>
        /// <returns></returns>
        public static string GetSelectColumns(PetaPoco.Database.PocoData poco, List<string> columns, bool isCount = false)
        {
            Type entityType = poco.type;
            if (columns == null)
            {
                columns = new List<string>();
            }
            if (isCount)
            {
                columns.Add("COUNT(1)");
            }
            else //if (columns.Count > 0)
            {
                if (columns.Count == 0)
                {
                    columns.AddRange(poco.QueryColumns);
                }
                else if (!columns.Contains(poco.TableInfo.PrimaryKey))
                {
                    columns.Add(poco.TableInfo.PrimaryKey);
                }
                for (int i = 0; i < columns.Count; i++)
                {
                    var item = columns[i];
                    var itemIndex = columns.IndexOf(item);
                    columns[itemIndex] = FormatColumn(poco, item);
                }
            }
            return string.Join(",", columns);
        }
        /// <summary>
        /// 获取过滤条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q"></param>
        /// <param name="otherCondition"></param>
        /// <returns></returns>
        public static Sql GetConditions<T>(QueryDescriptor<T> q, Sql otherCondition = null) where T : BaseEntity
        {
            return GetConditions(q.QueryText, q.Parameters, otherCondition);
        }
        /// <summary>
        /// 获取过滤条件
        /// </summary>
        /// <param name="queryText"></param>
        /// <param name="p"></param>
        /// <param name="otherCondition"></param>
        /// <returns></returns>
        public static Sql GetConditions(string queryText, List<QueryParameter> p, Sql otherCondition = null)
        {
            //过滤条件
            Sql filter = PetaPoco.Sql.Builder;
            if (queryText.IsNotEmpty() && p.IsNotNullOrEmpty())
            {
                var values = p.Select(n => n.Value).ToArray();
                filter.Append("WHERE");
                filter.Append(queryText, values);
            }
            if (otherCondition != null && otherCondition.SQL.IsNotEmpty())
            {
                if (filter.SQL.IsEmpty())
                {
                    filter.Append("WHERE");
                }
                filter.Append(otherCondition);
            }
            return filter;
        }


        public static ExecuteContext<T> ParseContext<T>(Func<QueryDescriptor<T>, Sql, bool, Sql> container, QueryDescriptor<T> q, Sql otherCondition = null, bool isCount = false) where T : BaseEntity
        {
            Sql s = container(q, otherCondition, isCount);
            ExecuteContext<T> ctx = new ExecuteContext<T>()
            {
                ExecuteContainer = s
                ,
                PagingInfo = q.PagingDescriptor
                ,
                TopCount = q.TopCount
            };

            return ctx;
        }
        /// <summary>
        /// 获取排序语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="poco"></param>
        /// <param name="sortingDescriptor"></param>
        /// <returns></returns>
        public static Sql GetOrderBy<T>(PetaPoco.Database.PocoData poco, List<SortDescriptor<T>> sortingDescriptor) where T : class
        {
            Sql query = PetaPoco.Sql.Builder;
            //排序
            if (sortingDescriptor != null && sortingDescriptor.Count > 0)
            {
                List<string> ord = new List<string>();
                foreach (var item in sortingDescriptor)
                {
                    if (item.Field.IsNotEmpty())
                    {
                        ord.Add(poco.TableInfo.TableName + "." + item.Field + " " + item.GetDbDirectionName());
                    }
                }
                if (ord.Count > 0)
                {
                    query.Append(" ORDER BY ");
                    query.Append(string.Join(",", ord));
                }
            }

            return query;
        }
    }
}
