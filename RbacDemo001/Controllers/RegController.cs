 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
    using RbacDemo001.Filters;
    using RbacDemo001.Models;

namespace RbacDemo001.Controllers
{
    [CustomAuthrozi(AuthorizationType = AuthorizationType.None)]
    public class RegController : Controller
    {
        RbacDb db=new RbacDb();
        // GET: rEG
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Reg(User regUser)
        {
            if (!ModelState.IsValid)
            {
                return Json(new {code = 400});
            }
            try
            {
                var role = db.Roles.FirstOrDefault(r => r.Id == 3);

                regUser.Roles.Add(role);

                db.Users.Add(regUser);

                db.SaveChanges();
            }
            catch (Exception)
            {

                return Json(new { code = 404 });
            }


            return Json(new { code = 200 });
        }
    }
}