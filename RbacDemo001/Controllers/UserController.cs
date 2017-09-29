using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RbacDemo001.Models;
using System.Data.Entity.Migrations;

namespace RbacDemo001.Controllers
{
    public class UserController : Controller
    {
        RbacDb db=new RbacDb();
        // GET: User
        public ActionResult Index()
        {
            return View(db.Users);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return Content("未找到编辑的模块");
            }

            return View(user);
        }
        public ActionResult Delete(int id)
        {
            User user = new User();

            user.Id = id;
            db.Users.Attach(user);

            db.Users.Remove(user);
            db.SaveChanges();

            return RedirectToAction("Index");

        }


        public ActionResult Save(User user)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = 400 });
            }
            db.Users.AddOrUpdate(user);

            db.SaveChanges();

            return Json(new { code = 200 });

        }
    }
}