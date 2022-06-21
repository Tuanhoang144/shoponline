using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAdmin.Models;

namespace WebAdmin.Controllers
{
    public class MessageController : BaseController
    {
        WebBanHangDB db = new WebBanHangDB();
        // GET: Message
        public ActionResult Index()
        {
            return View(db.Contracts.OrderByDescending(x=>x.ID));
        }
        public ActionResult Rely(long idMESS,string Subject,string Message)
        {
            try {

                var MESS = db.Contracts.Find(idMESS);
                EmailHelper email = new EmailHelper();
                email.SenContract(MESS.Email, Message, Subject);

                db.Contracts.Remove(MESS);
                db.SaveChanges();

            } catch { }
            return RedirectToAction("Index");
        }
        public ActionResult DeleteMess(long id)
        {
            try
            {

                var MESS = db.Contracts.Find(id);
              

                db.Contracts.Remove(MESS);
                db.SaveChanges();
            }
            catch { }
            return RedirectToAction("Index");
        }

    }
}