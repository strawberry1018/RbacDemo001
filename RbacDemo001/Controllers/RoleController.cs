using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RbacDemo001.Models;
using System.Data.Entity.Migrations;

namespace RbacDemo001.Controllers
{
    public class RoleController : Controller
    {
        RbacDb db = new RbacDb();
        // GET: Role
        public ActionResult Index()
        {
            return View(db.Roles);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            var role = db.Roles.FirstOrDefault(r => r.Id == id);
            if (role == null)
            {
                return Content("未找到编辑的模块");
            }

            return View(role);
        }

        public ActionResult Delete(int id)
        {
            Role role = new Role();

            role.Id = id;
            db.Roles.Attach(role);

            db.Roles.Remove(role);
            db.SaveChanges();

            return RedirectToAction("Index");

        }
        public ActionResult Save(Role role)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = 400 });
            }
            db.Roles.AddOrUpdate(role);

            db.SaveChanges();

            return Json(new { code = 200 });

        }
    }
}