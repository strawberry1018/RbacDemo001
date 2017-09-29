using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RbacDemo001.Models;

namespace RbacDemo001.Filters
{
    /// <summary>
    /// 授权验证类型枚举
    /// none 无限制 不进行身份验证
    /// Identity只进行身份验证 不进行授权验证
    /// Authorization使用身份和授权验证
    /// </summary>
    public enum AuthorizationType { None,Identity, Authorization}

   /// <summary>
   /// 自定义的过滤器
   /// </summary>
    public class CustomAuthroziAttribute:FilterAttribute,IAuthorizationFilter
    {
        /// <summary>
        /// 授权类型属性
        /// </summary>
        public AuthorizationType AuthorizationType { get; set; } = AuthorizationType.Authorization;

        public void OnAuthorization(AuthorizationContext filterContent)
        {
            //无限制
            if (AuthorizationType == AuthorizationType.None) return;
            //身份验证
            if (filterContent.HttpContext.Session["user"] == null)
            {
                RedirectToLogin(filterContent);
                return;
            }

            if(AuthorizationType==AuthorizationType.Identity) return;

            var contorller = filterContent.ActionDescriptor.ControllerDescriptor.ControllerName;

            var role = filterContent.HttpContext.Session["role"] as Role;

            if (role == null)
            {
                RedirectToLogin(filterContent);
                return;
            }

            var module = role.Modules.FirstOrDefault(m => m.Controller == contorller);

            if (module == null)
            {
                RedirectToLogin((filterContent));
            }



        }

        public void RedirectToLogin(AuthorizationContext filterContent)
        {
            var  url=new UrlHelper(filterContent.RequestContext);
            filterContent.Result=new RedirectResult(url.Action("Index","Login"));
        }
    }
}