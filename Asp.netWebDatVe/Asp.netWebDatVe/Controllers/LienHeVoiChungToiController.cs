using Asp.netWebDatVe.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Asp.netWebDatVe.Controllers
{
    public class LienHeVoiChungToiController : Controller
    {
        private QLDatVeContext db = new QLDatVeContext();

      
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Index(LienHe lienHe)
        {
            if (ModelState.IsValid)
            {
                lienHe.NgayGui = DateTime.Now;
                db.LienHes.Add(lienHe);
                db.SaveChanges();

                ViewBag.Message = "Gửi liên hệ thành công!";
                ModelState.Clear();
                return View();
            }

            return View(lienHe);
        }
        public IActionResult DanhSach()
        {
            var danhSachLienHe = db.LienHes.OrderByDescending(x => x.NgayGui).ToList();
            return View(danhSachLienHe);
        }

        [HttpPost]
        public IActionResult Xoa(int id)
        {
            var lienHe = db.LienHes.Find(id);
            if (lienHe != null)
            {
                db.LienHes.Remove(lienHe);
                db.SaveChanges();
            }
            return RedirectToAction("DanhSach");
        }

    }
}
