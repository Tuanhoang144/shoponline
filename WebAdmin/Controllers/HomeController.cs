using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAdmin.Contant;
using WebAdmin.Models;

namespace WebAdmin.Controllers
{
    public class HomeController : BaseController
    {
     public   WebBanHangDB DB = new WebBanHangDB();
        public ActionResult Index()
        {
            //var Product = new WebBanHangDB().Products.Where(x => x.delete != true);
            //foreach (var item in Product)
            //{

            //    string Name = Guid.NewGuid() + ".jpg";
            //    string fileName = Server.MapPath("~/Content/ImgSearch/" + Name);

            //    GetFileFromUrl(fileName, item.Imgs.FirstOrDefault().Url);


            //    var P = DB.Products.Find(item.Id);
            //    P.image = "/Content/ImgSearch/" + Name;
            //    DB.SaveChanges();
            //}

            return View();
        }
        public void GetFileFromUrl(string fileName, string url)
        {
            byte[] content;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = request.GetResponse();

            Stream stream = response.GetResponseStream();
            using (BinaryReader reader = new BinaryReader(stream))
            {
                content = reader.ReadBytes(500000);
                reader.Close();
            }
            response.Close();
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            try
            {
                bw.Write(content);

            }
            finally
            {
                bw.Close();
                fs.Close();
            }
        }

        private DateTime MondayOfWeek(DateTime date)
        {

            var dayOfWeek = date.DayOfWeek;

            if (dayOfWeek == DayOfWeek.Sunday)
            {
                return date.AddDays(-6);
            }
            int offset = dayOfWeek - DayOfWeek.Monday;
            return date.AddDays(-offset);
        }
        public JsonResult statisticalWeek()
        {
            DateTime fromdate = MondayOfWeek(DateTime.Now);
            DateTime todate = fromdate.AddDays(6);
            WebBanHangDB db = new WebBanHangDB();
            var voucherOrder = db.VoucherOrders.Where(x => x.status == 4 && x.delete != true).ToList();
            var product = db.Products.ToList();
            var voucherOrderDetail = db.VoucherOrderDetails.ToList();

            var voucherOrders = (from o in voucherOrder
                                 select new
                                 {
                                     date = o.createdate.Value.ToShortDateString(),
                                     o.grossAmount,
                                 }
                               ).ToList();
            var data = (from p in voucherOrders
                        group p by new { p.date }
                       into gr
                        select new
                        {
                            date = gr.Key.date,
                            grossAmount = gr.Sum(x => x.grossAmount)
                        }
                      ).ToList();
            List<string> arr = new List<string>();
            arr = data.Select(x => x.date).ToList();
            List<long> arr1 = new List<long>();
            arr1 = data.Select(x => ((long)x.grossAmount.Value)).ToList();
            return Json(new { arr, arr1 }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult statistical()
        {
            DateTime fromdate = MondayOfWeek(DateTime.Now);
            DateTime todate = fromdate.AddDays(6);
            WebBanHangDB db = new WebBanHangDB();
            var voucherOrder = db.VoucherOrders.Where(x => x.status == 4 && x.delete != true).ToList();
            var product = db.Products.ToList();
            var voucherOrderDetail = db.VoucherOrderDetails.ToList();
            var productDetail = (from o in voucherOrder
                                 join od in voucherOrderDetail on o.id equals od.voucherId
                                 join p in product on od.product_id equals p.Id

                                 select new
                                 {
                                     p.Id,
                                     Name = p.name,
                                     sl = p.stock,
                                 }
                                 ).ToList();
            var arr = (from p in productDetail
                       group p by new { p.Name, p.Id }
                       into gr
                       select new
                       {
                           name = gr.Key.Name,
                           sl = gr.Sum(x => x.sl)
                       }
                      ).ToList().Take(5);

            return Json(new { arr }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Category(string id)
        {
            WebBanHangDB db = new WebBanHangDB();
            var Categories = db.Categories.Select(x => x).ToList();
            return View(Categories);
        }
    }
}