using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAdmin.Models;
using WebAdmin.Models.MD5;

namespace WebAdmin.Controllers
{
    public class AccountController : Controller
    {
        WebBanHangDB db = new WebBanHangDB();
        // GET: Account
        public ActionResult Index()
        {
            Session["Login"] = null;
            return View();
        }
        [HttpPost]
        // GET: Account
        public ActionResult Index(FormCollection collection)
        {
            string UseName = collection["UseName"];
            string PASS = collection["pass"];
            var PassVery = new MD5().GetMD5(PASS);
            var User = db.Users.Where(x => x.Email.Trim().Equals(UseName) && x.Pass.Equals(PassVery)).FirstOrDefault();
            if (User != null)
            {

                Session["Login"] = User;
                return RedirectToAction("Index", "HOME");
            }


            return View();
        }
    }
}