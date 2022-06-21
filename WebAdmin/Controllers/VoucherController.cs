using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAdmin.Models;

namespace WebAdmin.Controllers
{
    public class VoucherController : BaseController
    {
        private WebBanHangDB db = new WebBanHangDB();
        // GET: Voucher
        public ActionResult Index()
        {

            return View(db.Vouchers.Where(x=>x.Delete!=true).ToList().Distinct());
        }
        // GET: Voucher
       [HttpGet]
        public ActionResult Create()
        {
          

                return View();
        }

        
        [HttpGet]
        public ActionResult Edit(long? id)
        {
            var voucher = db.Vouchers.Where(x => x.Id == id).FirstOrDefault();
            if (voucher == null)
            {

                return View("Error");
            }
         

            return View(voucher);
        }
        [HttpPost]
        public ActionResult Edit(Voucher voucher, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                var IdProducts = collection["IdProducts"];
            
                var Vouchers = db.Vouchers.Where(x => x.Id == voucher.Id).FirstOrDefault();
                if (voucher == null)
                {
                    ModelState.AddModelError("", "Không tìm thấy");

                    return View(voucher);
                }


                if (db.Vouchers.Where(x => x.voucher_code.Equals(voucher.voucher_code) && x.Id!=voucher.Id).FirstOrDefault() != null)
                {
                    ModelState.AddModelError("", "Mã này đã đc sử dụng");

                    return View(voucher);
                }

                if (voucher.start_time == voucher.end_time || voucher.start_time > voucher.end_time)
                {
                    ModelState.AddModelError("", "Ngày áp dụng không hợp lệ");

                    return View(voucher);
                }


                Vouchers.Delete = false;
                Vouchers.end_time = voucher.end_time;
                Vouchers.start_time = voucher.start_time;
                Vouchers.discount_value = voucher.discount_value;
                Vouchers.voucher_code = voucher.voucher_code;
                Vouchers.Description = voucher.Description;

                Vouchers.Type = voucher.Type;

                db.SaveChanges();
                var voucherDetails = db.VoucherDetails.Where(x => x.IdVoucher == voucher.Id).ToList();
                db.VoucherDetails.RemoveRange(voucherDetails);
                db.SaveChanges();
        

                return RedirectToAction("Index");
            }
            return View(voucher);
        }
        public ActionResult Delete(long id )
        {
            var voucher = db.Vouchers.Where(x => x.Id == id).FirstOrDefault();
            if (voucher == null)
            {

                return View("Error");
            }
            else
            {
                voucher.Delete = true;
                voucher.voucher_code = "";
                db.SaveChanges();
                return RedirectToAction("Index");

            }
           
        }
        [HttpPost]
        public ActionResult Create(Voucher voucher ,FormCollection collection)
        {
            if (ModelState.IsValid)
            {
               
             
                if (db.Vouchers.Where(x => x.voucher_code.Equals(voucher.voucher_code)).FirstOrDefault() != null)
                {
                    ModelState.AddModelError("","Mã này đã đc sử dụng");

                    return View(voucher);
                }

                if (voucher.start_time == voucher.end_time || voucher.start_time > voucher.end_time)
                {
                    ModelState.AddModelError("", "Ngày áp dụng không hợp lệ");

                    return View(voucher);
                }

                Voucher voucher1 = new Voucher()
                {
                    Delete = false,
                    end_time = voucher.end_time,
                    start_time = voucher.start_time,
                    discount_value = voucher.discount_value,
                    Description=voucher.Description,
                    voucher_code = voucher.voucher_code,
                    Type=voucher.Type
                    
                };
            db.Vouchers.Add(voucher1);

            db.SaveChanges();

           

                return RedirectToAction("Index");
            }
                return View(voucher);
        }

    }
}