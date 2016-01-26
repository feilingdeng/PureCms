using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Hosting;
using System.Web.Mvc;

namespace PureCms.Utilities
{
    public static partial class CommonHelper
    {

        /// <summary>
        /// Generate random digit code
        /// </summary>
        /// <param name="length">Length</param>
        /// <returns>Result string</returns>
        public static string GenerateRandomDigitCode(int length)
        {
            var random = new Random();
            string str = string.Empty;
            for (int i = 0; i < length; i++)
                str = String.Concat(str, random.Next(10).ToString());
            return str;
        }

        /// <summary>
        /// Returns an random interger number within a specified rage
        /// </summary>
        /// <param name="min">Minimum number</param>
        /// <param name="max">Maximum number</param>
        /// <returns>Result</returns>
        public static int GenerateRandomInteger(int min = 0, int max = 2147483647)
        {
            var randomNumberBuffer = new byte[10];
            new RNGCryptoServiceProvider().GetBytes(randomNumberBuffer);
            return new Random(BitConverter.ToInt32(randomNumberBuffer, 0)).Next(min, max);
        }

        /// <summary>
        /// Maps a virtual path to a physical disk path.
        /// </summary>
        /// <param name="path">The path to map. E.g. "~/bin"</param>
        /// <param name="findAppRoot">Specifies if the app root should be resolved when mapped directory does not exist</param>
        /// <returns>The physical path. E.g. "c:\inetpub\wwwroot\bin"</returns>
        /// <remarks>
        /// This method is able to resolve the web application root
        /// even when it's called during design-time (e.g. from EF design-time tools).
        /// </remarks>
        public static string MapPath(string path, bool findAppRoot = true)
        {
            Guard.ArgumentNotNull(() => path);

            if (HostingEnvironment.IsHosted)
            {
                // hosted
                return HostingEnvironment.MapPath(path);
            }
            else
            {
                // not hosted. For example, running in unit tests or EF tooling
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');

                var testPath = Path.Combine(baseDirectory, path);

                if (findAppRoot /* && !Directory.Exists(testPath)*/)
                {
                    // most likely we're in unit tests or design-mode (EF migration scaffolding)...
                    // find solution root directory first
                    var dir = FindSolutionRoot(baseDirectory);

                    // concat the web root
                    if (dir != null)
                    {
                        baseDirectory = Path.Combine(dir.FullName, "Presentation\\PureCms.Web");
                        testPath = Path.Combine(baseDirectory, path);
                    }
                }

                return testPath;
            }
        }

        public static bool IsDevEnvironment
        {
            get
            {
                if (!HostingEnvironment.IsHosted)
                    return true;

                if (HostingEnvironment.IsDevelopmentEnvironment)
                    return true;

                if (System.Diagnostics.Debugger.IsAttached)
                    return true;

                // if there's a 'SmartStore.NET.sln' in one of the parent folders,
                // then we're likely in a dev environment
                if (FindSolutionRoot(HostingEnvironment.MapPath("~/")) != null)
                    return true;

                return false;
            }
        }

        private static DirectoryInfo FindSolutionRoot(string currentDir)
        {
            var dir = Directory.GetParent(currentDir);
            while (true)
            {
                if (dir == null || IsSolutionRoot(dir))
                    break;

                dir = dir.Parent;
            }

            return dir;
        }

        private static bool IsSolutionRoot(DirectoryInfo dir)
        {
            return File.Exists(Path.Combine(dir.FullName, "PureCms.sln"));
        }

        /// <summary>
        /// Gets a setting from the application's <c>web.config</c> <c>appSettings</c> node
        /// </summary>
        /// <typeparam name="T">The type to convert the setting value to</typeparam>
        /// <param name="key">The key of the setting</param>
        /// <param name="defValue">The default value to return if the setting does not exist</param>
        /// <returns>The casted setting value</returns>
        public static string GetAppSetting(string key)
        {
            Guard.ArgumentNotEmpty(() => key);

            var setting = ConfigurationManager.AppSettings[key];

            if (setting == null)
            {
                return string.Empty;
            }

            return setting;
        }
        #region 数组操作

        /// <summary>
        /// 获得字符串在字符串数组中的位置
        /// </summary>
        public static int GetIndexInArray(string s, string[] array, bool ignoreCase)
        {
            if (string.IsNullOrEmpty(s) || array == null || array.Length == 0)
                return -1;

            int index = 0;
            string temp = null;

            if (ignoreCase)
                s = s.ToLower();

            foreach (string item in array)
            {
                if (ignoreCase)
                    temp = item.ToLower();
                else
                    temp = item;

                if (s == temp)
                    return index;
                else
                    index++;
            }

            return -1;
        }

        /// <summary>
        /// 获得字符串在字符串数组中的位置
        /// </summary>
        public static int GetIndexInArray(string s, string[] array)
        {
            return GetIndexInArray(s, array, false);
        }
        /// <summary>
        /// 判断字符串是否在字符串数组中
        /// </summary>
        public static bool IsInArray(string s, string[] array, bool ignoreCase)
        {
            return GetIndexInArray(s, array, ignoreCase) > -1;
        }

        /// <summary>
        /// 判断字符串是否在字符串数组中
        /// </summary>
        public static bool IsInArray(string s, string[] array)
        {
            return IsInArray(s, array, false);
        }

        /// <summary>
        /// 判断字符串是否在字符串中
        /// </summary>
        public static bool IsInArray(string s, string array, string splitStr, bool ignoreCase)
        {
            return IsInArray(s, array.SplitSafe(splitStr), ignoreCase);
        }

        /// <summary>
        /// 判断字符串是否在字符串中
        /// </summary>
        public static bool IsInArray(string s, string array, string splitStr)
        {
            return IsInArray(s, array.SplitSafe(splitStr), false);
        }

        /// <summary>
        /// 判断字符串是否在字符串中
        /// </summary>
        public static bool IsInArray(string s, string array)
        {
            return IsInArray(s, array.SplitSafe(","), false);
        }
        #endregion
    }
}
