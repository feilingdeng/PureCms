using PureCms.Core;
using PureCms.Core.Domain.User;
using PureCms.Services.Configuration;
using PureCms.Services.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureCms.Web.Framework
{
    /// <summary>
    /// 商城前台工作上下文类
    /// </summary>
    public class WebWorkContext
    {
        //public MallConfigInfo MallConfig = BMAConfig.MallConfig;//平台配置信息

        public bool IsHttpAjax;//当前请求是否为ajax请求

        public string IP;//用户ip

        public int RegionId;//区域id

        public string Url;//当前url

        public string UrlReferrer;//上一次访问的url

        public UserRankInfo UserRankInfo;//用户等级信息

        public CurrentUser CurrentUser = null;//用户信息

        public string Controller;//控制器

        public string Action;//动作方法

        public string PageKey;//页面标示符

        public int OnlineUserCount = 0;//在线总人数

        public int OnlineMemberCount = 0;//在线会员数

        public int OnlineGuestCount = 0;//在线游客数

        public string SearchWord;//搜索词

        public int CartProductCount = 0;//进货车中商品数量

        public DateTime StartExecuteTime;//页面开始执行时间

        public double ExecuteTime;//页面执行时间

        public int ExecuteCount = 0;//执行的sql语句数目

        public string ExecuteDetail;//执行的sql语句细节
        public string ImageCDN;
        public string CSSCDN;
        public string ScriptCDN;

        public string Version = PureCmsVersion.VERSION;//版本

        public string Copyright = PureCmsVersion.VERSION;//版权

        public bool IsSignIn {
            get
            {
                return this.CurrentUser != null;
            }
        }

    }
}
