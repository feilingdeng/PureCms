using PureCms.Core.Domain.User;
using PureCms.Services.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using PureCms.Core.Session;

namespace PureCms.Services.Security
{
    /// <summary>
    /// Authentication service
    /// </summary>
    public partial class FormsAuthenticationService
    {
        private readonly HttpContextBase _httpContext;
        private readonly UserService _userService;
        private readonly TimeSpan _expirationTimeSpan;
        private readonly ISession _session;

        private CurrentUser _cachedUser;

        /// <summary>
        /// Ctor
        /// </summary>
        public FormsAuthenticationService(HttpContextBase httpContext)
        {
            this._httpContext = httpContext;
            this._expirationTimeSpan = FormsAuthentication.Timeout;
            this._session = new AspNetSession(httpContext);
            this._userService = new UserService();
        }

        public string LoginUrl
        {
            get
            {
                return FormsAuthentication.LoginUrl;
            }
        }


        public virtual void SignIn(UserInfo user, bool createPersistentCookie = true)
        {
            var now = DateTime.UtcNow.ToLocalTime();

            var ticket = new FormsAuthenticationTicket(
                1 /*version*/,
                user.LoginName,
                now,
                now.Add(_expirationTimeSpan),
                createPersistentCookie,
                user.LoginName,
                FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            cookie.HttpOnly = true;
            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            _httpContext.Response.Cookies.Add(cookie);
            _cachedUser = new CurrentUser();
            _cachedUser.LoginName = user.LoginName;
            _cachedUser.RoleId = user.RoleId;
            _cachedUser.UserId = user.UserId;
            _cachedUser.UserName = user.UserName;
            _cachedUser.Privileges = user.Privileges;
            _cachedUser.IsSuperAdmin = user.LoginName.ToLower() == "purecms";

            _session.Set(CurrentUser.SESSION_KEY, _cachedUser, null);
        }

        public virtual void SignOut()
        {
            _cachedUser = null;
            FormsAuthentication.SignOut();
            _session.Remove(CurrentUser.SESSION_KEY);
        }

        public virtual CurrentUser GetAuthenticatedUser()
        {
            if (_cachedUser != null)
                return _cachedUser;

            if (_httpContext == null ||
                _httpContext.Request == null ||
                !_httpContext.Request.IsAuthenticated ||
                !(_httpContext.User.Identity is FormsIdentity))
            {
                return null;
            }

            var formsIdentity = (FormsIdentity)_httpContext.User.Identity;
            _cachedUser = GetAuthenticatedUserFromTicket(formsIdentity.Ticket);
            //if (user != null && user.IsActive && !user.IsDeleted)
            //{
            //    _cachedUser = new CurrentUser();
            //    _cachedUser.RoleId = user.RoleId;
            //    _cachedUser.UserId = user.UserId;
            //    _cachedUser.UserName = user.UserName;
            //    _cachedUser.IsSuperAdmin = user.LoginName.ToLower() == "purecms";
            //    _cachedUser.Privileges = user.Privileges;
            //}
            return _cachedUser;
        }

        public virtual CurrentUser GetAuthenticatedUserFromTicket(FormsAuthenticationTicket ticket)
        {
            if (ticket == null)
                throw new ArgumentNullException("ticket");

            var loginname = ticket.UserData;

            if (String.IsNullOrWhiteSpace(loginname))
                return null;

            var user = _session.Get<CurrentUser>(CurrentUser.SESSION_KEY);//_userService.GetUserByLoginName(loginname);

            if (user != null && user.LoginName.IsCaseInsensitiveEqual(loginname))
            {
                return user;
            }
            else
            {
                SignOut();
            }

            return null;
        }
    }
}
