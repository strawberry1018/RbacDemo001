using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using RbacDemo001.Filters;
using RbacDemo001.Models;
using System.Linq;

namespace RbacDemo001.Controllers
{

    [CustomAuthrozi(AuthorizationType = AuthorizationType.Identity)]
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 头部分部视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Header()
        {

            var user = Session["user"] as User;
            var roles = Session["roles"] as List<Role>;
            var role = Session["role"] as Role;


            List<SelectListItem> roleList = new List<SelectListItem>();
            foreach (var item in roles)
            {
                roleList.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString(), Selected = item.Id == role.Id });
            }
            ViewBag.roleList = roleList;
            return PartialView(user);
        }

        /// <summary>
        /// 导航分部视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Nav(int roleId=0)
        {
            var roleModules = Session["roleModules"] as Dictionary<int, ICollection<Module>>;

            var role = Session["role"] as Role;

            List<Module> modules;
            if (roleModules.ContainsKey(roleId))
            {
                modules = roleModules[roleId].ToList();
            }
            else
            {
                modules = roleModules[role.Id].ToList();
            }

            return PartialView(modules);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}