using ImgSearch;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAdmin.Models;

namespace WebAdmin.Controllers
{
    public class ImgSearchController : Controller
    {
        private Bitmap bmpSearchImage;
        private Bitmap bmpSearchImageProcessed;
        //private ImageSearchAlgorithm _algorithm = new ImageSearchAlgorithm();
        private List<Color> _centroidColor = new List<Color>() { Color.Red, Color.Blue,Color.Yellow,Color.Gray};
        private SearchImage _searchImage;
        private string[] _fileArray;
        private WebBanHangDB DB = new WebBanHangDB();
        private List<KeyValuePair<string, double>> similarityList;
        public static List<KeyValuePair<string, string>> listImg;
        // GET: ImgSearch
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFiles()
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var files = Request.Files;

                    //iterating through multiple file collection   
                    foreach (string str in files)
                    {
                        HttpPostedFileBase file = Request.Files[str] as HttpPostedFileBase;
              
                        //Method 2 Get file details from HttpPostedFileBase class    

                        if (file != null)
                        {
                            string path = Path.Combine(Server.MapPath("~/UploadedFilesTest"), Guid.NewGuid().ToString() + ".jpg");
                            file.SaveAs(path);
                            bmpSearchImage = new Bitmap(Image.FromFile(path),40, 30);
                            //_algorithm.RunAlgorithm(bmpSearchImage, _centroidColor.Count);
                            //bmpSearchImageProcessed = _algorithm.ProcessImage(bmpSearchImage, _centroidColor);
                            Dictionary<int, string> listSearchImageProcessed = new Dictionary<int, string>();
                            Dictionary<long, string> listSearchImageData = new Dictionary<long, string>();
                            int cout = -1;
                            for (int j = 0; j < bmpSearchImage.Width; j++)
                            {
                                for (int k = 0; k < bmpSearchImage.Height; k++)
                                {
                                    Color c1 = bmpSearchImage.GetPixel(j, k);
                                    listSearchImageProcessed.Add(++cout, c1.ToArgb().ToString());

                                }
                            }
                            _searchImage = new SearchImage();

                            WebBanHangDB db = new WebBanHangDB();
                            var Product = db.Products.Where(x => x.delete != true).ToList();
                            //foreach (var item in Product)
                            //{

                            //    string fileLink = Path.Combine(Server.MapPath("~" + item.image));

                            //    using (Bitmap bmpImage = new Bitmap(Image.FromFile(fileLink), bmpSearchImage.Width, bmpSearchImage.Height))
                            //    {
                            //        List<string> lists = new List<string>();
                            //        using (Bitmap bmpPorcessedImage = _algorithm.ProcessImage(bmpImage, _centroidColor))
                            //        {
                            //            cout = -1;
                            //            for (int j = 0; j < bmpSearchImage.Width; j++)
                            //            {
                            //                for (int k = 0; k < bmpSearchImage.Height; k++)
                            //                {
                            //                    Color c1 = bmpImage.GetPixel(j, k);
                            //                    lists.Add(c1.ToArgb().ToString());
                            //                }
                            //            }
                            //        }
                            //        var product = db.Products.Find(item.Id);
                            //        product.dataSearch = string.Join(",", lists);
                            //        db.SaveChanges();

                            //    }

                            //}
                            //_fileArray = System.IO.Directory.GetFiles(Server.MapPath("~/Content/ImgSearch"));

                            var ListId = _searchImage.SortBySimilarity(listSearchImageProcessed , _centroidColor).Take(6).ToList();
                            return Json(data: ListId, JsonRequestBehavior.AllowGet);

                        }
                        return Json(data: new List<long>(), JsonRequestBehavior.AllowGet);
                    
                    }
                    
                }
                catch (Exception e)
                {
                    ViewBag.FileStatus = "Error while file uploading." +e.Message;
                    return Json(data:new List<long>() , JsonRequestBehavior.AllowGet);
                 
                }
            }
            return Json(data: new List<long>(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}