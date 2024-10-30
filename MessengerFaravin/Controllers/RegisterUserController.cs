using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MessengerFaravin.Models;

namespace MessengerFaravin.Controllers
{
    public class RegisterUserController : Controller
    {
        public faravinEntities1 db=new faravinEntities1();
        public faravinEntities4 dbb=new faravinEntities4();
        
        public ActionResult Index2()
        {
            var users = db.user.ToList();
            return View(users);
          
        }

        public ActionResult Index()
        {
            return View("registerUser");
        }

      /* 
      [HttpPost]
        //public ActionResult Index([Bind(Include = "firstName,lastName,phoneNumber,birthDate,joinDate")]user user)
        public ActionResult Indexx([Bind(Include = "id,firstName,lastname")]Table_2 user)
        {
            if (ModelState.IsValid)
            {
                dbb.Table_2.Add(user);
                dbb.SaveChanges();
                return View();
            }
            return View();
        }
      */

        public ActionResult example()
        {
            var tbl=dbb.Table_2.ToList();
            return View(tbl);
        }

        public JsonResult Indexx(string firstName, string lastName,string phoneNumber,DateTime? birthDate , string password)
        {
            byte A = 0;
            user ClassB = new user();
            var Result = db.user.Where(x => x.phoneNumber ==phoneNumber ).ToList();
            if (Result.Count() > 0)
            {
                A = 1;
            }
            else
            {
                ClassB.firstName = firstName;
                ClassB.lastName = lastName;
                ClassB.phoneNumber = phoneNumber;
                ClassB.joinDate = DateTime.Now;
                ClassB.birthDate=birthDate;
                ClassB.password = password;
                //ClassB.joinDate=joinDate;

                db.user.Add(ClassB);
                db.SaveChanges();
                A = 2;
            }
            //var users = db.user.ToList();
            return Json(new { status=A  , JsonRequestBehavior.AllowGet});
        }

       public ActionResult List()
        {
            var users = db.user.ToList();
            return PartialView(users);
        }

    }
}