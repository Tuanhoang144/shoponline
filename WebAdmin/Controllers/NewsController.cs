using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAdmin.Models;

namespace WebAdmin.Controllers
{
    public class NewsController : BaseController
    {
        WebBanHangDB db = new WebBanHangDB();
        // GET: News
        public ActionResult Index()
        {
            return View(db.NewTypes.Where(x=>x.Delete!=true));
        }
        public ActionResult ListAllNews()
        {
            return View(db.News.Where(x => x.Delete != true));
        }

        public ActionResult CreatNews()
        {
            List<NewType> Category = db.NewTypes.Where(x => x.Delete != true).ToList();
            ViewBag.Category = new SelectList(Category, "ID", "Name");
            return View();
        }
        public ActionResult DeleteNews(long? id)
        {
            try {
                if (id != null)
                {
                    var News = db.News.Find(id);
                    News.Delete = true;
                    db.SaveChanges();
                }
                return RedirectToAction("ListAllNews");
            } catch
            {
                return View("Error");
            }
           
        }
        

       [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreatNews(News news)
        {
            List<NewType> Category = db.NewTypes.Where(x => x.Delete != true).ToList();
            ViewBag.Category = new SelectList(Category, "ID", "Name");

            if (news.IdType > 0)
            {
                List<string> Img = (List<string>)Session["Img"];
                if (Img != null)
                {
                    if (ModelState.IsValid)
                    {
                        News newType = new News();
                        newType.Description = news.Description;
                        newType.Title = news.Title;
                        newType.Delete = false;
                        newType.Img = string.Join(",", Img);
                        newType.View = 0;
                        newType.CreateDate = DateTime.Now;
                        newType.IdType = news.IdType;
                        db.News.Add(newType);
                        
                        db.SaveChanges();
                        return RedirectToAction("ListAllNews");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Vui lòng chọn hình ảnh");
                }
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng chọn danh mục tin tức");
            }
                return View();
        }

        [HttpPost]
        public ActionResult CreateNewType(FormCollection collection)
        {

            var id = collection["idCategory"];
            var Url = collection["Url"];
            if (id.Length > 0)
            {

                NewType Newsupdate = db.NewTypes.Find(long.Parse(id));
                Newsupdate.Name = collection["Name"];
                Newsupdate.Description = collection["DesCrip"];
                Newsupdate.Img = Url;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {

                NewType categorynew = new NewType();
             
              
                categorynew.Name = collection["Name"];
                categorynew.Description = collection["DesCrip"];
                categorynew.Delete = false;
                categorynew.Img = Url;
                db.NewTypes.Add(categorynew);
                db.SaveChanges();
              
            }
            return RedirectToAction("Index");
        }
        public ActionResult DeleteNewType(long? id)
        {
            if (id !=null)
            {

                NewType Newsupdate = db.NewTypes.Find(id);
                if (Newsupdate != null)
                {
                    Newsupdate.Delete =true;
                    db.SaveChanges();
                 
              
               
            }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        

    }
}