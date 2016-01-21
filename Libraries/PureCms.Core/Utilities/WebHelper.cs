using System;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using PureCms.Core.Data;
using PureCms.Core.Domain;

namespace PureCms.Utilities
{

    public partial class WebHelper
    {
		private static bool? s_optimizedCompilationsEnabled = null;
		private static AspNetHostingPermissionLevel? s_trustLevel = null;
		private static readonly Regex s_staticExts = new Regex(@"(.*?)\.(css|js|png|jpg|jpeg|gif|bmp|html|htm|xml|pdf|doc|xls|rar|zip|ico|eot|svg|ttf|woff|otf|axd|ashx|less)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		private static readonly Regex s_htmlPathPattern = new Regex(@"(?<=(?:href|src)=(?:""|'))(?!https?://)(?<url>[^(?:""|')]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);
		private static readonly Regex s_cssPathPattern = new Regex(@"url\('(?<url>.+)'\)", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);

        private bool? _isCurrentConnectionSecured;
		private bool? _appPathPossiblyAppended;
		private bool? _appPathPossiblyAppendedSsl;


        public static string GetUrlReferrer()
        {
            string referrerUrl = string.Empty;

            if (HttpContext.Current.Request != null &&
                HttpContext.Current.Request.UrlReferrer != null)
                referrerUrl = HttpContext.Current.Request.UrlReferrer.ToString();

            return referrerUrl;
        }

        public static string GetCurrentIpAddress()
        {
			string result = null;

            if (HttpContext.Current.Request != null)
                result = HttpContext.Current.Request.UserHostAddress;

			if (result == "::1")
				result = "127.0.0.1";

			return result.EmptyNull();
        }
        
        public static string GetThisPageUrl(bool includeQueryString = false)
        {
            return GetThisPageUrl(includeQueryString, false);
        }

        public static string GetThisPageUrl(bool includeQueryString, bool useSsl)
        {
            string url = string.Empty;
            if (HttpContext.Current.Request == null)
                return url;

            if (includeQueryString)
            {
                url = HttpContext.Current.Request.RawUrl;
            }
            else
            {
                if (HttpContext.Current.Request.Url != null)
				{
                    url = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path);
				}
            }

            return url.ToLowerInvariant();
        }
        
        public static string ServerVariables(string name)
        {
            string result = string.Empty;

            try
            {
                if (HttpContext.Current.Request != null)
				{
                    if (HttpContext.Current.Request.ServerVariables[name] != null)
					{
                        result = HttpContext.Current.Request.ServerVariables[name];
					}
				}
            }
            catch
            {
                result = string.Empty;
            }
            return result;
        }

        private string GetHostPart(string url)
        {
            var uri = new Uri(url);
            var host = uri.GetComponents(UriComponents.Scheme | UriComponents.Host, UriFormat.Unescaped);
            return host;
        }
        
        public static bool IsStaticResource(HttpRequest request)
        {
			return IsStaticResourceRequested(new HttpRequestWrapper(request));
        }

		public static bool IsStaticResourceRequested(HttpRequest request)
		{
			Guard.ArgumentNotNull(() => request);
			return s_staticExts.IsMatch(request.Path);
		}

		public static bool IsStaticResourceRequested(HttpRequestBase request)
		{
			// unit testable
			Guard.ArgumentNotNull(() => request);
			return s_staticExts.IsMatch(request.Path);
		}

        public static string MapPath(string path)
        {
			return CommonHelper.MapPath(path, false);
        }
        
        public static void RestartAppDomain(bool makeRedirect = false, string redirectUrl = "", bool aggressive = false)
        {
			HttpRuntime.UnloadAppDomain();

			if (aggressive)
			{
				TryWriteBinFolder();
			}
			else
			{
				// without this, MVC may fail resolving controllers for newly installed plugins after IIS restart
				Thread.Sleep(250);
			}

            // If setting up plugins requires an AppDomain restart, it's very unlikely the
            // current request can be processed correctly.  So, we redirect to the same URL, so that the
            // new request will come to the newly started AppDomain.
            if (makeRedirect)
            {
                if (HttpContext.Current.Request.RequestType == "GET")
				{
					if (String.IsNullOrEmpty(redirectUrl))
					{
						redirectUrl = GetThisPageUrl(true);
					}
                    HttpContext.Current.Response.Redirect(redirectUrl, true /*endResponse*/);
				}
				else
				{
					// Don't redirect posts...
                    HttpContext.Current.Response.ContentType = "text/html";
                    HttpContext.Current.Response.WriteFile("~/refresh.html");
                    HttpContext.Current.Response.End();
				}
            }
        }

        private bool TryWriteWebConfig()
        {
            try
            {
                // In medium trust, "UnloadAppDomain" is not supported. Touch web.config
                // to force an AppDomain restart.
                File.SetLastWriteTimeUtc(MapPath("~/web.config"), DateTime.UtcNow);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool TryWriteGlobalAsax()
        {
            try
            {
                //When a new plugin is dropped in the Plugins folder and is installed into SmartSTore.NET, 
                //even if the plugin has registered routes for its controllers, 
                //these routes will not be working as the MVC framework can't
                //find the new controller types in order to instantiate the requested controller. 
                //That's why you get these nasty errors 
                //i.e "Controller does not implement IController".
                //The solution is to touch the 'top-level' global.asax file
                File.SetLastWriteTimeUtc(MapPath("~/global.asax"), DateTime.UtcNow);
                return true;
            }
            catch
            {
                return false;
            }
        }

		private static bool TryWriteBinFolder()
		{
			try
			{
				var binMarker = MapPath("~/bin/HostRestart");
				Directory.CreateDirectory(binMarker);

				using (var stream = File.CreateText(Path.Combine(binMarker, "marker.txt")))
				{
					stream.WriteLine("Restart on '{0}'", DateTime.UtcNow);
					stream.Flush();
				}
				return true;
			}
			catch
			{
				return false;
			}
		}

		internal static bool OptimizedCompilationsEnabled
		{
			get
			{
				if (!s_optimizedCompilationsEnabled.HasValue)
				{
					var section = (CompilationSection)ConfigurationManager.GetSection("system.web/compilation");
					s_optimizedCompilationsEnabled = section.OptimizeCompilations;
				}

				return s_optimizedCompilationsEnabled.Value;
			}
		}

        public static bool IsRequestBeingRedirected
        {
            get
            {
                var response = HttpContext.Current.Response;
                return response.IsRequestBeingRedirected;   
            }
        }

        public static bool IsPostBeingDone
        {
            get
            {
                if (HttpContext.Current.Items["sm.IsPOSTBeingDone"] == null)
                    return false;
                return Convert.ToBoolean(HttpContext.Current.Items["sm.IsPOSTBeingDone"]);
            }
            set
            {
                HttpContext.Current.Items["sm.IsPOSTBeingDone"] = value;
            }
        }

		/// <summary>
		/// Finds the trust level of the running application (http://blogs.msdn.com/dmitryr/archive/2007/01/23/finding-out-the-current-trust-level-in-asp-net.aspx)
		/// </summary>
		/// <returns>The current trust level.</returns>
		public static AspNetHostingPermissionLevel GetTrustLevel()
		{
			if (!s_trustLevel.HasValue)
			{
				//set minimum
				s_trustLevel = AspNetHostingPermissionLevel.None;

				//determine maximum
				foreach (AspNetHostingPermissionLevel trustLevel in
						new AspNetHostingPermissionLevel[] {
                                AspNetHostingPermissionLevel.Unrestricted,
                                AspNetHostingPermissionLevel.High,
                                AspNetHostingPermissionLevel.Medium,
                                AspNetHostingPermissionLevel.Low,
                                AspNetHostingPermissionLevel.Minimal 
                            })
				{
					try
					{
						new AspNetHostingPermission(trustLevel).Demand();
						s_trustLevel = trustLevel;
						break; //we've set the highest permission we can
					}
					catch (System.Security.SecurityException)
					{
						continue;
					}
				}
			}
			return s_trustLevel.Value;
		}

		/// <summary>
		/// Prepends protocol and host to all (relative) urls in a html string
		/// </summary>
		/// <param name="html">The html string</param>
		/// <param name="request">Request object</param>
		/// <returns>The transformed result html</returns>
		/// <remarks>
		/// All html attributed named <c>src</c> and <c>href</c> are affected, also occurences of <c>url('path')</c> within embedded stylesheets.
		/// </remarks>
		public static string MakeAllUrlsAbsolute(string html, HttpRequestBase request)
		{
			Guard.ArgumentNotNull(() => request);

			if (request.Url == null)
			{
				return html;
			}

			return MakeAllUrlsAbsolute(html, request.Url.Scheme, request.Url.Authority);
		}

		/// <summary>
		/// Prepends protocol and host to all (relative) urls in a html string
		/// </summary>
		/// <param name="html">The html string</param>
		/// <param name="protocol">The protocol to prepend, e.g. <c>http</c></param>
		/// <param name="host">The host name to prepend, e.g. <c>www.mysite.com</c></param>
		/// <returns>The transformed result html</returns>
		/// <remarks>
		/// All html attributed named <c>src</c> and <c>href</c> are affected, also occurences of <c>url('path')</c> within embedded stylesheets.
		/// </remarks>
		public static string MakeAllUrlsAbsolute(string html, string protocol, string host)
		{
			Guard.ArgumentNotEmpty(() => html);
			Guard.ArgumentNotEmpty(() => protocol);
			Guard.ArgumentNotEmpty(() => host);

			string baseUrl = string.Format("{0}://{1}", protocol, host.TrimEnd('/'));

			MatchEvaluator evaluator = (match) =>
			{
				var url = match.Groups["url"].Value;
				return "{0}{1}".FormatCurrent(baseUrl, url.EnsureStartsWith("/"));
			};

			html = s_htmlPathPattern.Replace(html, evaluator);
			html = s_cssPathPattern.Replace(html, evaluator);

			return html;
		}

		/// <summary>
		/// Prepends protocol and host to the given (relative) url
		/// </summary>
		public static string GetAbsoluteUrl(string url, HttpRequestBase request)
		{
			Guard.ArgumentNotEmpty(() => url);
			Guard.ArgumentNotNull(() => request);

			if (request.Url == null)
			{
				return url;
			}

			if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
			{
				return url;
			}

			if (url.StartsWith("~"))
			{
				url = VirtualPathUtility.ToAbsolute(url);
			}

			url = String.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, url);
			return url;
		}


        /// <summary>
        /// 是否get请求
        /// </summary>
        /// <returns></returns>
        public static bool IsGetRequest()
        {
            return HttpContext.Current.Request.RequestType == "GET";
        }

        /// <summary>
        /// 是否post请求
        /// </summary>
        /// <returns></returns>
        public static bool IsPostRequest()
        {
            return HttpContext.Current.Request.RequestType == "POST";
        }
        /// <summary>
        /// 是否Ajax请求
        /// </summary>
        /// <returns></returns>
        public static bool IsAjaxRequest()
        {
            return HttpContext.Current.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}
