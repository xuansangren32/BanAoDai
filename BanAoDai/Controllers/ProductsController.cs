using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanAoDai.Controllers
{
    public class ProductsController : Controller
    {
        private BanAoDaiDBContext db = new BanAoDaiDBContext();
        // GET: Products
        public ActionResult Index()
        {
            var items = db.Products.ToList();
            //if (id != null)
            //{
            //    items = items.Where(x => x.CatId == id).ToList();
            //}
            return View(items);
        }
        public ActionResult Detail(string slug3,int id)
        {
            var items = db.Products.Find(id);
            //int catid = product.CatId;
            //var listcatid = new List<int>();
            //listcatid.Add(catid);
            //var categorys = db.Categorys.Where(x => x.ParentId == catid && x.Status == 1).ToList();
            //if (categorys.Count > 0)
            //{
            //    foreach (var cat in categorys)
            //    {
            //        listcatid.Add(cat.Id);
            //        var categorys1 = db.Categorys.Where(x => x.ParentId == cat.Id && x.Status == 1).ToList();
            //        if (categorys1.Count > 0)
            //        {
            //            foreach (var cat1 in categorys1)
            //            {

            //                listcatid.Add(cat1.Id);
            //            }
            //        }
            //    }
            //}
            //var product_other = db.Products.Where(x => x.Status == 1 && x.Id != product.Id && listcatid.Contains(x.CatId))
            //    .OrderByDescending(x => x.CreatedBy).Take(5).ToList();
            //ViewBag.ListOrther = product_other;
            //return View(/*"Detail",*/ product);
            return View(items);
        }
        public ActionResult ProductCategorys(string slug2, int? id)
        {
            var items = db.Products.ToList();
            if (id > 0)
            {
                items = items.Where(x => x.CatId == id).ToList();
            }
            var cate = db.Products.Find(id);
            if (cate != null)
            {
                ViewBag.CateName = cate.Name;
            }
            ViewBag.CateId = id;
            return View(items);
        }
       
        public ActionResult ItemCateId()
        {

            var item = db.Products.Where(x => x.IsHome && x.IsActive && x.Status == 1).Take(30).ToList();
            return PartialView(item);
        }

        public ActionResult ProductSale()
        {

            var item = db.Products.Where(x => x.IsHome && x.IsActive).Take(30).ToList();
            return PartialView(item);
        }
    }
}