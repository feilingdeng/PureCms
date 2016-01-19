using PureCms.Core.Utilities;
using PureCms.Services.Session;
using PureCms.Web.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PureCms.Web.Controllers
{
    public class ToolController : Controller
    {

        /// <summary>
        /// 验证图片
        /// </summary>
        /// <param name="width">图片宽度</param>
        /// <param name="height">图片高度</param>
        /// <returns></returns>
        public ImageResult VerifyImage(int width = 56, int height = 20)
        {
            //获得用户唯一标示符sid
            //string sid = MallUtils.GetSidCookie();
            ////当sid为空时
            //if (sid == null)
            //{
            //    //生成sid
            //    sid = Sessions.GenerateSid();
            //    //将sid保存到cookie中
            //    MallUtils.SetSidCookie(sid);
            //}
            Randoms _randoms = new Randoms();
            SessionService _session = new SessionService(this.HttpContext);
            //生成验证值
            string verifyValue = _randoms.CreateRandomValue(4, false).ToLower();
            //生成验证图片
            RandomImage verifyImage = _randoms.CreateRandomImage(verifyValue, width, height, Color.White, Color.Blue, Color.DarkRed);
            //将验证值保存到session中
            _session.SetItem("verifyCode", verifyValue, 5);

            //输出验证图片
            return new ImageResult(verifyImage.Image, verifyImage.ContentType);
        }


        ///// <summary>
        ///// 验证图片(纯数字)
        ///// </summary>
        ///// <param name="width">图片宽度</param>
        ///// <param name="height">图片高度</param>
        ///// <returns></returns>
        //public ImageResult VerifyImageNum(int width = 56, int height = 20)
        //{
        //    //获得用户唯一标示符sid
        //    string sid = MallUtils.GetSidCookie();
        //    //当sid为空时
        //    if (sid == null)
        //    {
        //        //生成sid
        //        sid = Sessions.GenerateSid();
        //        //将sid保存到cookie中
        //        MallUtils.SetSidCookie(sid);
        //    }

        //    //生成验证值
        //    string verifyValue = Randoms.CreateRandomValue(4).ToLower();
        //    //生成验证图片
        //    RandomImage verifyImage = Randoms.CreateRandomImage(verifyValue, width, height, Color.White, Color.Blue, Color.DarkRed);
        //    //将验证值保存到session中
        //    Sessions.SetItem(sid, "verifyCode", verifyValue);

        //    //输出验证图片
        //    return new ImageResult(verifyImage.Image, verifyImage.ContentType);
        //}

    }
}
