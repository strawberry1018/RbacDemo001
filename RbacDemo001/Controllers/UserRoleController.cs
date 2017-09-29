using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Web.WebPages.Html;
using RbacDemo001.Models;
using RbacDemo001.RoleModules;

namespace RbacDemo001.Controllers
{
    public class UserRoleController : Controller
    {
        RbacDb db = new RbacDb();
        // GET: UserRole
        public ActionResult Index()
        {
            var result = db.Users.Include(r =>r.Roles);
            return View(result);
        }

        public ActionResult Creat()
        {
            //获取所有用户的下拉列表
            ViewBag.UserOptions = from r in db.Users
                                  select new System.Web.Mvc.SelectListItem { Text = r.Username, Value = r.Id.ToString() };
            //获取所有角色的下拉列表
            ViewBag.RoleOptions = from r in db.Roles
                                  select new System.Web.Mvc.SelectListItem { Text = r.Name, Value = r.Id.ToString() };
            return View();
        }

        public ActionResult Edit(UserRoleView userRole)
        {
            userRole.UserName = db.Users.FirstOrDefault(r => r.Id == userRole.UserId).Username;
            userRole.RoleName = db.Roles.FirstOrDefault(r => r.Id == userRole.RoleId).Name;

            ViewBag.ModuleOption = from r in db.Roles
                                   select new System.Web.Mvc.SelectListItem { Text = r.Name, Value = r.Id.ToString() };
            return View(userRole);
        }

        public ActionResult Delete(UserRoleView userRole)
        {
            //先把要添加角色的用户找出来
            var user = db.Users.FirstOrDefault(r => r.Id == userRole.UserId);

            var role = new Role { Id = userRole.RoleId };

            //构造一个要添加的权限模块
            db.Roles.Attach(role);

            //吧这个要添加的权限模块，add到角色的模块集合中
            user.Roles.Remove(role);

            if (db.SaveChanges() == 0)
            {
                return Json(new { code = 400 });
            }
            return Json(new { code = 200 });
        }

        public ActionResult Insert(UserRoleView userRole)
        {
            //先把要添加角色的用户找出来
            var user = db.Users.FirstOrDefault(r => r.Id == userRole.UserId);

            var role = new Role { Id = userRole.RoleId };

            //构造一个要添加的权限模块
            db.Roles.Attach(role);

            //吧这个要添加的权限模块，add到角色的模块集合中
            user.Roles.Add(role);

            if (db.SaveChanges() == 0)
            {
                return Json(new { code = 400 });
            }
            return Json(new { code = 200 });
        }

        public ActionResult Update(UserRoleView userRole)
        {
            if (userRole.RoleId == userRole.UpdateRoleId)
                return Content("更新失败");
            //先把要更新角色的用户找出来
            var user = db.Users.FirstOrDefault(r => r.Id == userRole.UserId);
            //var module = db.Modules.FirstOrDefault(r => r.Id == roleModule.Moduleid);
            //构造一个原来的角色
            var role = new Role { Id = userRole.RoleId };
            db.Roles.Attach(role);

            //构造一个要更新的模块
            var updaterole = new Role { Id = userRole.UpdateRoleId };
            db.Roles.Attach(updaterole);
            //更新就是先删除后添加
            user.Roles.Remove(role);
            user.Roles.Add(updaterole);

            if (db.SaveChanges() == 0)
            {
                return Json(new { code = 400 });
            }
            return Json(new { code = 200 });
        }
    }
}