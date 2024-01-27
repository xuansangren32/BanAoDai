using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanAoDai.Areas.Admin.Controllers
{
    public class ProductImageController : Controller
    {
        private BanAoDaiDBContext dbProductImage = new BanAoDaiDBContext();
        // GET: Admin/ProductImage
        public ActionResult Index(int id)
        {
            ViewBag.ProductId = id;
            var items = dbProductImage.ProductImages.Where(x => x.ProductId == id).ToList();
            return View(items);
        }
        [HttpPost]
        public ActionResult AddImage(int productId, string url)
        {
            dbProductImage.ProductImages.Add(new ProductImage
            {
                ProductId = productId,
                Img = url,
                IsDefault = false
            });
            dbProductImage.SaveChanges();
            return Json(new { Success = true });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = dbProductImage.ProductImages.Find(id);
            dbProductImage.ProductImages.Remove(item);
            dbProductImage.SaveChanges();
            return Json(new { Success = true });
        }
    }
}