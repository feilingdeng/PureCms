﻿using Newtonsoft.Json;
using PureCms.Core.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PureCms.Core
{
    public static class ObjectExtensions
    {

        #region  序列化

        /// <summary>
        /// XML序列化
        /// </summary>
        /// <param name="obj">序列对象</param>
        /// <param name="filePath">XML文件路径</param>
        /// <returns>是否成功</returns>
        public static bool SerializeToXml(this object source, string filePath)
        {
            bool result = false;

            FileStream fs = null;
            try
            {
                fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(source.GetType());
                serializer.Serialize(fs, source);
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return result;

        }

        /// <summary>
        /// XML反序列化
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="filePath">XML文件路径</param>
        /// <returns>序列对象</returns>
        public static void DeserializeFromXML<T>(this object target, out T result, string filePath) where T : class
        {
            Type type = typeof(T);
            FileStream fs = null;
            try
            {
                fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(type);
                result = serializer.Deserialize(fs) as T;
                //return target as T;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }
        /// <summary>
        /// json序列化
        /// </summary>
        /// <param name="obj">序列对象</param>
        /// <returns>是否成功</returns>
        public static string SerializeToJson(this object source, bool nameLower = true)
        {
            JsonSerializerSettings jss = new JsonSerializerSettings();
            if (nameLower)
            {
                jss.ContractResolver = new LowercaseContractResolver();
            }
            var t = JsonConvert.SerializeObjectAsync(source, Formatting.Indented, jss);
            return t.Result;
        }
        /// <summary>
        /// JSON反序列化
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="data">json</param>
        /// <returns>序列对象</returns>
        public static void DeserializeFromJson<T>(this object target, out T result, string data) where T : class
        {
            var t = Newtonsoft.Json.JsonConvert.DeserializeObjectAsync<T>(data);
            result = t.Result;
        }

        #endregion
    }
}