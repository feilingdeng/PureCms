﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
using System.Data;
using System.Data.SqlClient;

namespace PureCms.Core.Data
{
    public class ColumnMapper : PetaPoco.IMapper
    {
        //PetaPoco.Database.Mapper = new ColumnMapper();
        public void GetTableInfo(Type t, TableInfo ti)
        {
            //PetaPoco.Database.PocoData.ForType(typeof(T));
        }
        public bool MapPropertyToColumn(System.Reflection.PropertyInfo pi, ref string columnName, ref bool resultColumn)
        {
            // Do default property mapping
            return true;
        }


        public Func<object, object> GetFromDbConverter(System.Reflection.PropertyInfo pi, Type SourceType)
        {
            return null;
        }

        public Func<object, object> GetToDbConverter(Type SourceType)
        {
            Func<object, object> fn = new Func<object, object>(delegate(object item)
            {
                IDbDataParameter p = new SqlParameter();
                //p.Size = 5;
                if (item == null)
                {
                    p.Value = DBNull.Value;
                }
                else
                {
                    var t = item.GetType();
                    if (t.IsEnum)		// PostgreSQL .NET driver wont cast enum to int
                    {
                        p.Value = (int)item;
                    }
                    else if (t == typeof(Guid))
                    {
                        p.Value = item.ToString();
                        p.DbType = DbType.String;
                        p.Size = 40;
                    }
                    else if (t == typeof(string))
                    {
                        p.Size = Math.Max((item as string).Length + 1, 4000);		// Help query plan caching by using common size
                        p.Value = item;
                    }
                    else if (t == typeof(AnsiString))
                    {
                        // Thanks @DataChomp for pointing out the SQL Server indexing performance hit of using wrong string type on varchar
                        p.Size = Math.Max((item as AnsiString).Value.Length + 1, 4000);
                        p.Value = (item as AnsiString).Value;
                        p.DbType = DbType.AnsiString;
                    }
                    else if (t == typeof(bool))
                    {
                        p.Value = ((bool)item) ? 1 : 0;
                    }
                    else if (item.GetType().Name == "SqlGeography") //SqlGeography is a CLR Type
                    {
                        p.GetType().GetProperty("UdtTypeName").SetValue(p, "geography", null); //geography is the equivalent SQL Server Type
                        p.Value = item;
                    }

                    else if (item.GetType().Name == "SqlGeometry") //SqlGeometry is a CLR Type
                    {
                        p.GetType().GetProperty("UdtTypeName").SetValue(p, "geometry", null); //geography is the equivalent SQL Server Type
                        p.Value = item;
                    }
                    else
                    {
                        p.Value = item;
                    }
                }

                return p;
            });
            return fn;
        }
    }
}