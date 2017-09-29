using System.Linq;
using System.Web.Mvc;
using RbacDemo001.Models;
using RbacDemo001.Filters;

namespace RbacDemo001.Controllers
{

    [CustomAuthrozi(AuthorizationType=AuthorizationType.None)]
    public class LoginController : Controller
    {
         RbacDb db = new RbacDb();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User LoginUser)
        {
            //模型绑定验证
            if (!ModelState.IsValid)
            {
                return Json(new {code = 400});
            }
            //查找用户
            var user = db.Users.FirstOrDefault(u => u.Username == LoginUser.Username && u.Password == LoginUser.Password);

            if (user == null) return Json(new {code = 404});
            //设置session，作为身份验证的标识
            Session["user"] = user;

            var roleModules = user.Roles.ToDictionary(r => r.Id, r => r.Modules);

            Session["roleModules"] = roleModules;

            var roles = user.Roles.ToList();

            Session["roles"] = roles;

            Session["role"] = roles[0];

            return Json(new {code = 200});
        }
    }
}