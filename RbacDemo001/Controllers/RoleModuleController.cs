using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Web.WebPages.Html;
using RbacDemo001.Models;
using RbacDemo001.RoleModules;

namespace RbacDemo001.Controllers
{
    public class RoleModuleController : Controller
    {
        RbacDb db=new RbacDb();
        // GET: RoleModule
        public ActionResult Index()
        {
            var result = db.Roles.Include(r => r.Modules);

            return View(result);
        }

        public ActionResult Creat()
        {
            ViewBag.RoleOptions = from r in db.Roles
               select new System.Web.Mvc.SelectListItem { Text = r.Name,Value = r.Id.ToString()};
            ViewBag.ModuleOptions = from r in db.Modules
                select new System.Web.Mvc.SelectListItem { Text = r.Name, Value = r.Id.ToString()};
            
            return View();
        }

        public ActionResult Edit(RoleModuleView roleModule)
        {
            roleModule.RoleName = db.Roles.FirstOrDefault(r => r.Id == roleModule.RoleId).Name;
            roleModule.ModuleName = db.Modules.FirstOrDefault(r => r.Id == roleModule.ModuleId).Name;

            ViewBag.ModuleOption = from r in db.Modules
                select new System.Web.Mvc.SelectListItem {Text = r.Name, Value = r.Id.ToString()};
            return View(roleModule);
        }

        public ActionResult Delete(RoleModuleView roleModule)
        {
            var role = db.Roles.FirstOrDefault(r => r.Id == roleModule.RoleId);

            var module = new Module { Id = roleModule.ModuleId };
            db.Modules.Attach(module);

            role.Modules.Remove(module);
            if (db.SaveChanges() == 0)
            {
                return Json(new { code = 400 });
            }
            return Json(new { code = 200 });

        }

        public ActionResult Insert(RoleModuleView roleModule)
        {

            var role = db.Roles.FirstOrDefault(r => r.Id == roleModule.RoleId);

            var module = new Module { Id = roleModule.ModuleId };
            db.Modules.Attach(module);

            role.Modules.Add(module);
            if (db.SaveChanges() == 0)
            {
                return Json(new { code = 400 });
            }
            return Json(new { code = 200 });

            //if (!ModelState.IsValid)
            //{
            //    return Json(new  {code = 400});
            //}

            //var num=db.Database.ExecuteSqlCommand("insert into RoleModule values(@roleId,@moduleId)",new SqlParameter { ParameterName = "@roleId",Value = roleModule.RoleId},
            //new SqlParameter {ParameterName = "@moduleId", Value = roleModule.ModuleId});
            //if (num == 0)
            //{
            //    return Json(new {code = 400});
            //}
            //return Json(new {code = 200});

        }

        public ActionResult Update(RoleModuleView roleModule)
        {
            if (roleModule.ModuleId == roleModule.UpdateModuleId)
            {
                return Json(new { code = 400 });
            }

            var role = db.Roles.FirstOrDefault(r => r.Id == roleModule.RoleId);

            var module = new Module { Id = roleModule.ModuleId };
            db.Modules.Attach(module);

            var updateModule = new Module {Id = roleModule.UpdateModuleId};
            db.Modules.Attach(updateModule);

            role.Modules.Remove(module);
            role.Modules.Add(updateModule);

            if (db.SaveChanges() == 0)
            {
                return Json(new { code = 400 });
            }
            return Json(new { code = 200 });
        }
    }
}