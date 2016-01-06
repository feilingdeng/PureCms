using PetaPoco;
using PureCms.Core;
using PureCms.Core.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PureCms.Data
{
    public class ContextHelper
    {
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
                    BindingFlags flag = BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance;
                    var p = entityType.GetProperty(item, flag);
                    var attrs = p.GetCustomAttributes(typeof(LinkEntityAttribute), true);
                    if (attrs.Length > 0)
                    {
                        var leAttr = (LinkEntityAttribute)attrs[0];
                        var linkTarget = leAttr.Target;
                        var linkPoco = new Database.PocoData(linkTarget);
                        var fieldName = (leAttr.SourceFieldName.IsNotEmpty() ? leAttr.SourceFieldName : item);
                        columns[itemIndex] = linkPoco.TableInfo.TableName + "." + fieldName;//关联表字段
                    }
                    else
                    {
                        columns[itemIndex] = poco.TableInfo.TableName + "." + item;//主表字段
                    }
                }
            }
            return string.Join(",", columns);
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
