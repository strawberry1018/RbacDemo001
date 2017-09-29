using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RbacDemo001.Models;
using System.Data.Entity.Migrations;

namespace RbacDemo001.Controllers
{
    public class ModuleController : Controller
    {
        RbacDb db=new RbacDb();
        // GET: Module
        public ActionResult Index()
        {
            return View(db.Modules);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            var module = db.Modules.FirstOrDefault(m => m.Id == id);
            if(module==null)
            {
                return Content("未找到编辑的模块");
            }
            
            return View(module);
        }

        public ActionResult Delete(int id)
        {
            Module module=new Module();

            module.Id = id;
            db.Modules.Attach(module);

            db.Modules.Remove(module);
            db.SaveChanges();

            return RedirectToAction("Index");

        }

        public ActionResult Save(Module module)
        {
            if (!ModelState.IsValid)
            {
                return Json(new {code = 400});
            }
            db.Modules.AddOrUpdate(module);

            db.SaveChanges();

            return Json(new {code = 200});

        }
    }
}