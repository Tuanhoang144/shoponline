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
    public class ProductController : BaseController
    {
        WebBanHangDB db = new WebBanHangDB();
        // GET: Product
        public ActionResult Index()
        {
            var Products = db.Products.Where(x => (x.delete == false || x.delete == null)).ToList();
            return View(Products);
           
        }
        public ActionResult Create()
        {
          
       
            List<Categorie> Category = db.Categories.Where(x => x.delete != true).ToList();
            ViewBag.Category = new SelectList(Category, "Id", "display_name");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection collection ,Product product)
        {
      
            Product productNew = new Product();
            if (product.category_id >0)
            {
                var pricediscount = product.price;
                try {
                    if (product.discount != null && product.discount.Contains("%"))
                    {
                       var discount = decimal.Parse(product.discount.Split('%')[0])/(decimal)100;
                        pricediscount =(decimal)((decimal
                            )pricediscount * (decimal) discount);

                        pricediscount = product.price - pricediscount;
                    }
                    else
                    if (product.discount != null)
                    {
                        var discount = decimal.Parse(product.discount);
                        pricediscount = pricediscount - discount;

                    }

                } catch {
                    ModelState.AddModelError("", "Vui lòng nhập lại giảm giá");
                List<Categorie> Category1 = db.Categories.Where(x => x.delete != true).ToList();

                ViewBag.Category = new SelectList(Category1, "Id", "display_name");
                return View(product);

            }
                if (ModelState.IsValid)
                {
                    List<string> Img = (List<string>)Session["Img"];


                    productNew.name = product.name;
                    productNew.category_id = product.category_id;
                    productNew.delete = false;
                    productNew.price = pricediscount;
                    productNew.price_before_discount = product.price;
                    productNew.price_min_before_discount = product.price;
                    productNew.rating_star = 0;
                    productNew.discount = product.discount;
                    productNew.stock = 0;
                    productNew.description = product.description;
                    productNew.liked_count = 0;
                    productNew.cmt_count = 0;
                    productNew.image = Img?.FirstOrDefault();
                    string fileLink = Path.Combine(Server.MapPath("~" + Img?.FirstOrDefault()));
                    using (Bitmap bmpImage = new Bitmap(Image.FromFile(fileLink), 40, 30))
                    {
                        List<string> lists = new List<string>();
                        for (int j = 0; j < bmpImage.Width; j++)
                        {
                            for (int k = 0; k < bmpImage.Height; k++)
                            {
                                Color c1 = bmpImage.GetPixel(j, k);
                                lists.Add(c1.ToArgb().ToString());
                            }
                        }
                        productNew.dataSearch = string.Join(",", lists);
                    }
                    db.Products.Add(productNew);
                    db.SaveChanges();
                    try
                    {

                        foreach (var item in Img)
                        {
                            db.Imgs.Add(new Img()
                            {
                                Url = item,
                                IdProduct = productNew.Id,
                            });
                            db.SaveChanges();
                        }
                        var number = collection["numberoption"];
                        List<Option_Product> a = new List<Option_Product>();
                        for (int i = 0; i < int.Parse(number); i++)
                        {
                            string s = collection[$"stock{i}"];
                            a.Add(new Option_Product()
                            {
                                IdProduct = productNew.Id,
                                option = collection[$"option{i}"],
                                Stock = int.Parse(s)

                            }) ;
                        }
                        db.Option_Product.AddRange(a);
                        db.SaveChanges();
                        Session["Img"] = null;
                        return RedirectToAction("Index");
                    }
                    catch { }

                }
            }else
            {
                ModelState.AddModelError("", "Vui lòng chọn danh mục");
            }

            List<Categorie> Category = db.Categories.Where(x => x.delete != true).ToList();
            
            ViewBag.Category = new SelectList(Category, "Id", "display_name");
            return View(product);
        }
        public ActionResult Delete(long id)
        {
    
            Product productNew = db.Products.Find(id);
            if (productNew != null)
            {
                productNew.delete = true;
                db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Edit(long? id)
        {
            try
            {
                if (id == null)
                {
                    return View("Error");
                }

   
                Product productNew = db.Products.Where(x => x.Id==id).FirstOrDefault();
                List<Categorie> Category = db.Categories.Where(x => x.delete != true).ToList();
                ViewBag.Category = new SelectList(Category, "Id", "display_name");
       
                List<string> photo = new List<string>();
                foreach (var item in productNew.Imgs)
                {
                    photo.Add(item.Url);
                }
                Session["Img"] = photo;
                return View(productNew);
            }
            catch { }
            return RedirectToAction("Create");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection collection, Product product)
        {
            Product productNew = new Product();
            if (product.category_id > 0)
            {
                var pricediscount = product.price;

                if (ModelState.IsValid)
                {
                    try
                    {
                        if (product.discount != null && product.discount.Contains("%"))
                        {
                            var discount = decimal.Parse(product.discount.Split('%')[0]) / (decimal)100;
                            pricediscount = (decimal)((decimal
                                )pricediscount * (decimal)discount);
                            pricediscount = product.price - pricediscount;
                        }
                        else
                        if (product.discount != null)
                        {
                            var discount = decimal.Parse(product.discount);
                            pricediscount = pricediscount - discount;

                        }

                    }
                    catch
                    {
                        ModelState.AddModelError("", "Vui lòng nhập lại giảm giá");
                        List<Categorie> Category1 = db.Categories.Where(x => x.delete != true).ToList();

                        ViewBag.Category = new SelectList(Category1, "Id", "display_name");
                        return View(product);

                    }
                     productNew = db.Products.Find(product.Id);
                    if (productNew != null)
                    {
                        List<string> Img = (List<string>)Session["Img"];
                        if (Img != null)
                        {
                            productNew.name = product.name;
                            productNew.category_id = product.category_id;
                            productNew.delete = false;
                            productNew.price = pricediscount;
                            productNew.price_before_discount = product.price;
                            productNew.price_min_before_discount = product.price;
                            productNew.rating_star = 0;
                            productNew.discount = product.discount;
                            productNew.stock = 0;
                            productNew.description = product.description;
                            productNew.liked_count = 0;
                            productNew.cmt_count = 0;
                            productNew.image = Img?.FirstOrDefault();
                            string fileLink = Path.Combine(Server.MapPath("~" + Img?.FirstOrDefault()));
                            using (Bitmap bmpImage = new Bitmap(Image.FromFile(fileLink), 40, 30))
                            {
                                List<string> lists = new List<string>();
                                for (int j = 0; j < bmpImage.Width; j++)
                                {
                                    for (int k = 0; k < bmpImage.Height; k++)
                                    {
                                        Color c1 = bmpImage.GetPixel(j, k);
                                        lists.Add(c1.ToArgb().ToString());
                                    }
                                }
                                productNew.dataSearch = string.Join(",", lists);
                            }
                            db.SaveChanges();
                            try
                            {
                                var photo = db.Imgs.Where(x => x.IdProduct == productNew.Id);
                                db.Imgs.RemoveRange(photo);
                                db.SaveChanges();
                                var Option = db.Option_Product.Where(x => x.IdProduct == productNew.Id);
                                db.Option_Product.RemoveRange(Option);
                                db.SaveChanges();
                                foreach (var item in Img)
                                {
                                    db.Imgs.Add(new Img()
                                    {
                                        Url = item,
                                        IdProduct = productNew.Id,
                                    });
                                    db.SaveChanges();
                                }
                                var number = collection["numberoption"];
                                List<Option_Product> a = new List<Option_Product>();
                                for (int i = 0; i < int.Parse(number); i++)
                                {
                                    string s = collection[$"stock{i}"];
                                    a.Add(new Option_Product()
                                    {
                                        IdProduct = productNew.Id,
                                        option = collection[$"option{i}"],
                                        Stock = int.Parse(s)

                                    });
                                }
                                db.Option_Product.AddRange(a);
                                db.SaveChanges();
                                Session["Img"] = null;
                                return RedirectToAction("Index");
                            }
                            catch { }

                        }
                        else
                        {
                            ModelState.AddModelError("", "Vui lòng chọn hình ảnh");
                        }
                     
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng chọn danh mục");
            }
            List<Categorie> Category = db.Categories.Where(x => x.delete != true).ToList();

            ViewBag.Category = new SelectList(Category, "Id", "display_name");
            return View(product);
        }
    }
}