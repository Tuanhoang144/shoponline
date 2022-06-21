using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models.Emtity;

namespace WebBanHang.Controllers
{
    public class NewController : Controller
    {
        WebBanHangDB db = new WebBanHangDB();
        // GET: New
        public ActionResult Index(long? id,int? page)
        {
            ViewBag.url = "Index";
            int pageSize = 3;

            // 4.1 Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);
            ViewBag.page = page;
            ViewBag.pageSize = pageSize;
            if (id != null)
            {
                ViewBag.url = "Index/" + id;
                return View(db.News.Where(x => x.Delete != true && x.IdType==id).OrderByDescending(x=>x.ID).ToPagedList(pageNumber, pageSize));
            }
            return View(db.News.Where(x=>x.Delete!=true).OrderByDescending(x => x.ID).ToPagedList(pageNumber, pageSize));
        }
        
        [HttpGet]
              public ActionResult AddComment(long? id , string name ,string message)
        {
            if (id != null)
            {
                CommentNew comment = new CommentNew()
                {
                    IDNew = id.Value,
                    Create = DateTime.Now,
                    Name=name,
                    Message=message,
                };
                
                db.CommentNews.Add(comment);
                db.SaveChanges();
            }
           return RedirectToAction("Detail/" + id);
        
        }
            public ActionResult Detail(long? id)
        {
            if (id != null)
            {
                var New = db.News.Where(x => x.Delete != true && x.ID == id).FirstOrDefault();
                if (New != null)
                {
                    return View(New);
                }

              ;
            }
           
            return View("Error");
        }
        
    }
}