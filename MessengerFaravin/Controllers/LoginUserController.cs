using MessengerFaravin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MessengerFaravin.Controllers
{
    public class LoginUserController : Controller
    {
        public faravinEntities1 db = new faravinEntities1();
        
        public ActionResult Index()
        {
            return View("loginUser");
        }


        public JsonResult Indexx(string userName, string password)
        {
            byte A = 0;
            var mainUser = db.user.Where(x => x.phoneNumber == userName).FirstOrDefault();
            if (mainUser == null)
            {
                A = 1;
            }
            else if (mainUser.password != password)
            {
                A = 2;
            }
            else
            {
                A = 3;
               Session["userId"] = Convert.ToInt32(mainUser.id);

            }
            return Json(new { status = A, JsonRequestBehavior.AllowGet });
        }

    }
}