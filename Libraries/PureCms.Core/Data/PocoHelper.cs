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
        public static Sql ParseSelectSql<T>(QueryDescriptor<T> q, Sql otherCondition = null, bool isCount = false) where T : BaseEntity
        {
            return ParseSelectSql<T>(PetaPoco.Database.PocoData.ForType(typeof(T)), q, otherCondition, isCount);
        }
        /// <summary>
        /// 生成查询语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="poco"></param>
        /// <param name="q"></param>
        /// <param name="otherCondition"></param>
        /// <param name="isCount"></param>
        /// <returns></returns>
        public static Sql ParseSelectSql<T>(PetaPoco.Database.PocoData poco, QueryDescriptor<T> q, Sql otherCondition = null, bool isCount = false) where T : BaseEntity
        {
            List<string> froms;
            var columns = PocoHelper.GetSelectColumns(poco, q.Columns, out froms, isCount);
            Sql query = PetaPoco.Sql.Builder.Append("SELECT " + columns);// + " FROM [" + TableName + "]");
            //from，由select中反推
            query.From(froms.ToArray());

            //过滤条件
            query.Append(PocoHelper.GetConditions<T>(q, otherCondition));

            //排序
            if (isCount == false)
            {
                query.Append(PocoHelper.GetOrderBy<T>(poco, q.SortingDescriptor));
            }

            return query;
        }
        /// <summary>
        /// 格式化列名为：[表名].[列名]
        /// </summary>
        /// <param name="poco"></param>
        /// <param name="name"></param>
        /// <param name="fromTable"></param>
        /// <returns></returns>
        public static string FormatColumn(PetaPoco.Database.PocoData poco, string name)
        {
            var fromTable = string.Empty;
            return FormatColumn(poco, name, out fromTable);
        }
        /// <summary>
        /// 格式化列名为：[表名].[列名]
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="name"></param>
        /// <param name="fromTable"></param>
        /// <returns></returns>
        public static string FormatColumn(Type entityType, string name)
        {
            var fromTable = string.Empty;
            return FormatColumn(entityType, name, out fromTable);
        }

        /// <summary>
        /// 格式化列名为：[表名].[列名]
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="name"></param>
        /// <param name="fromTable"></param>
        /// <returns></returns>
        public static string FormatColumn(Type entityType, string name, out string fromTable)
        {
            return FormatColumn(PetaPoco.Database.PocoData.ForType(entityType), name, out fromTable);
        }
        /// <summary>
        /// 格式化列名为：[表名].[列名]
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string FormatColumn(PetaPoco.Database.PocoData poco, string name, out string fromTable)
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
                fromTable = linkPoco.TableInfo.TableName;
            }
            else
            {
                name = "[" + poco.TableInfo.TableName + "].[" + name + "]";//主表字段
                fromTable = poco.TableInfo.TableName;
            }
            return name;
        }
        /// <summary>
        /// 获取查询列名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columns"></param>
        /// <param name="froms"></param>
        /// <param name="isCount"></param>
        /// <returns></returns>
        public static string GetSelectColumns<T>(List<string> columns, out List<string> froms, bool isCount = false) where T : BaseEntity
        {
            return GetSelectColumns(PetaPoco.Database.PocoData.ForType(typeof(T)), columns, out froms, isCount);
        }
        /// <summary>
        /// 获取查询列名
        /// </summary>
        /// <param name="poco"></param>
        /// <param name="columns"></param>
        /// <param name="isCount"></param>
        /// <returns></returns>
        public static string GetSelectColumns(PetaPoco.Database.PocoData poco, List<string> columns, out List<string> froms, bool isCount = false)
        {
            froms = new List<string>();
            froms.Add(poco.TableInfo.TableName);
            Type entityType = poco.type;
            if (columns == null)
            {
                columns = new List<string>();
            }
            if (isCount)
            {
                columns.Add("COUNT(1)");
            }
            else
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
                    var fromTable = string.Empty;
                    columns[itemIndex] = FormatColumn(poco, item, out fromTable);
                    if (!froms.Contains(fromTable))
                    {
                        froms.Add(fromTable);
                    }
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
        public static Sql GetConditions<T>(QueryDescriptor<T> q, Sql otherCondition = null) where T : class
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


        /// <summary>
        /// 获取排序语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="poco"></param>
        /// <param name="sortingDescriptor"></param>
        /// <returns></returns>
        public static Sql GetOrderBy<T>(PetaPoco.Database.PocoData poco, List<SortDescriptor<T>> sortingDescriptor) where T : BaseEntity
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
        /// <summary>
        /// 转换为数据库执行上下文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="poco"></param>
        /// <param name="q"></param>
        /// <param name="otherCondition"></param>
        /// <param name="isCount"></param>
        /// <returns></returns>
        public static ExecuteContext<T> ParseContext<T>(PetaPoco.Database.PocoData poco, QueryDescriptor<T> q, Sql otherCondition = null, bool isCount = false) where T : BaseEntity
        {
            Sql s = ParseSelectSql<T>(poco, q, otherCondition, isCount);
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
        /// 转换为数据库执行上下文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="poco"></param>
        /// <param name="q"></param>
        /// <param name="otherCondition"></param>
        /// <param name="isCount"></param>
        /// <returns></returns>
        public static ExecuteContext<T> ParseContext<T>(QueryDescriptor<T> q, Sql otherCondition = null, bool isCount = false) where T : BaseEntity
        {
            Sql s = ParseSelectSql<T>(PetaPoco.Database.PocoData.ForType(typeof(T)), q, otherCondition, isCount);
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
        /// 转换为数据库执行上下文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container"></param>
        /// <param name="poco"></param>
        /// <param name="q"></param>
        /// <param name="otherCondition"></param>
        /// <param name="isCount"></param>
        /// <returns></returns>
        public static ExecuteContext<T> ParseContext<T>(Func<PetaPoco.Database.PocoData, QueryDescriptor<T>, Sql, bool, Sql> container, PetaPoco.Database.PocoData poco, QueryDescriptor<T> q, Sql otherCondition = null, bool isCount = false) where T : BaseEntity
        {
            Sql s = container(poco, q, otherCondition, isCount);
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
        /// 生成更新语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityType"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public static Sql ParseUpdateSql<T>(Type entityType, UpdateContext<T> q) where T : BaseEntity
        {
            return ParseUpdateSql<T>(PetaPoco.Database.PocoData.ForType(typeof(T)), q);
        }
        /// <summary>
        /// 生成更新语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="poco"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public static Sql ParseUpdateSql<T>(PetaPoco.Database.PocoData poco, UpdateContext<T> q) where T : BaseEntity
        {
            Sql query = PetaPoco.Sql.Builder.Append("UPDATE [" + poco.TableInfo.TableName + "] SET ");
            string optName = string.Empty;
            foreach (var item in q.Sets)
            {
                query.Append(PocoHelper.FormatColumn(poco, item.Key) + "=@0", item.Value);
            }
            query.Append(PocoHelper.GetConditions(q.QueryText, q.Parameters));
            return query;
        }
    }
}
