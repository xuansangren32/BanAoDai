using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BanAoDai.Libraries;
using CKFinder.Settings;
using MyClass.Model;
using PagedList;

namespace BanAoDai.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private BanAoDaiDBContext db = new BanAoDaiDBContext();

        // GET: Admin/Product
        public ActionResult Index(string Searchtext, int? page)
        {

            var pageSize = 4;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Product> items = db.Products.Where(x => x.Status != 0).OrderByDescending(x => x.Id);
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Slug.Contains(Searchtext) || x.Name.Contains(Searchtext));
                //var items = db.Products.Where(x => x.Status != 0).ToList();
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            //ViewBag.Brand = new SelectList(db.Brands.Where(x => x.Status != 0).ToList(), "Id", "Name");
            ViewBag.Brand = new SelectList(db.Brands.ToList(), "Id", "Name");
            return View("Index",items);
            //return View(items);
        }
      
        // GET: Admin/Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Product/Create
        public ActionResult Create()
        {
            ViewBag.ListCatId = new SelectList(db.Categorys.Where(x => x.Status != 0).ToList(), "Id", "Name");
            //ViewBag.Brand = new SelectList(db.Brands.ToList(), "Id", "Name");
            ViewBag.Category = new SelectList(db.Categorys.ToList(), "Id", "Title");
            ViewBag.Brand = new SelectList(db.Brands.Where(x => x.Status != 0).ToList(), "Id", "Name");
            return View();
            //return View();
            //return View();
        }

        // POST: Admin/Product/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Product product, List<string> Images, List<int> rDefault)
        {
            //if (ModelState.IsValid)
            //{
                if (ModelState.IsValid)
                {
                    if (Images != null && Images.Count > 0)
                    {
                        for (int i = 0; i < Images.Count; i++)
                        {
                            if (i + 1 == rDefault[0])
                            {
                                product.Img = Images[i];
                                product.ProductImages.Add(new ProductImage
                                {
                                    ProductId = product.Id,
                                    Img = Images[i],
                                    IsDefault = true
                                });
                            }
                            else
                            {
                                product.ProductImages.Add(new ProductImage
                                {
                                    ProductId = product.Id,
                                    Img = Images[i],
                                    IsDefault = false
                                });
                            }
                        }
                    }
                //them noi dung vao 5 trường
                if (string.IsNullOrEmpty(product.SeoKeyword))
                {
                    product.SeoKeyword = product.Name;
                }
                if (string.IsNullOrEmpty(product.Slug))
                    product.Slug = BanAoDai.Models.Common.Filter.FilterChar(product.Name);
                //product.Slug = MyString.str_Slug(product.Name);
                product.CreatedBy_At = DateTime.Now;
                product.CreatedBy = 1;
                //product.Detail = BanAoDai.Models.Common.Filter.FilterChar(product.Detail);
                product.PriceBuy = product.PriceBuy;
                product.PriceSale = product.PriceSale;
                
                db.Products.Add(product);

                if (db.SaveChanges() != 0)
                {
                    
                    Link link = new Link();
                    link.Slug = product.Slug;
                    link.TypeLink = "product";
                    link.TableId = product.Id;
                    db.Links.Add(link);
                    db.SaveChanges();
                }
                TempData["message"] = new MessegeAlert("success", "Thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListCatId = new SelectList(db.Categorys.Where(x => x.Status != 0).ToList(), "Id", "Name");
            ViewBag.Brand = new SelectList(db.Brands.ToList(), "Id", "Name");
            return View(product);
        }

        // GET: Admin/Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListCatId = new SelectList(db.Categorys.Where(x => x.Status != 0).ToList(), "Id", "Name");
            //Product product = db.Products.Find(id);
            return View(product);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product, List<string> Images, List<int> rDefault)
        {
            if (ModelState.IsValid)
            {
                
                //them noi dung vao 5 trường
                product.Slug = MyString.str_Slug(product.Name);
                product.CreatedBy = (Session["UserAdmin_id"].ToString() != "") ? int.Parse(Session["UserAdmin_id"].ToString()) : 1;
                product.CreatedBy_At = DateTime.Now;
                if (product.BrandId != 0)
                {
                    product.BrandId = 1;
                }
                db.Products.Attach(product);
                db.Entry(product).State = EntityState.Modified;
               
                if (db.SaveChanges() !=0)
                {
                    Link link = new Link();
                    link.Slug = product.Slug;
                    link.TypeLink = "product";
                    link.TableId = product.Id;
                    db.Links.Add(link);
                    db.SaveChanges();
                };
                TempData["message"] = new MessegeAlert("success", "Thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListCatId = new SelectList(db.Categorys.Where(x => x.Status != 0).ToList(), "Id", "Name");
            return View(product);
        }

        // GET: Admin/product/Destroy/5
        // Xóa khỏi CSDL
        [HttpGet]
        public ActionResult Destroy(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View("Destroy", product);
        }

        // POST: Admin/product/Delete/5
        [HttpPost, ActionName("Destroy")]
        [ValidateAntiForgeryToken]
        public ActionResult DestroyConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            if (db.SaveChanges() != 0)
            {
                Link link = db.Links.Where(x => x.TypeLink == "product" && x.TableId == id).FirstOrDefault();
                db.Links.Remove(link);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        // GET: Admin/product/Delete/5
        // Xóa vào thùng rác status=0
    
        public ActionResult Delete(int? id)
        {
            Product product = db.Products.Find(id); // lấy ra 1 chi tiết mẫu tin
            product.Status = 0;
            product.UpdatedAt = DateTime.Now;
            product.UpdatedBy = 1;
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");//cập nhật và chuyển hướng trang
        }

        // GET: Admin/product/Status/5
        //Thay đổi trạng thái 1-->2 2-->1
       
        public ActionResult Status(int? id)
        {
            Product product = db.Products.Find(id); // lấy ra 1 chi tiết mẫu tin
            if (product == null)
            {
                TempData["message"] = new MessegeAlert("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index");
            }
            product.Status = (product.Status == 2) ? 1 : 2;
            product.UpdatedAt = DateTime.Now;
            product.UpdatedBy = (Session["UserAdmin_id"].ToString() != "") ? int.Parse(Session["UserAdmin_id"].ToString()) : 1;
            db.Entry(product).State = EntityState.Modified;//
            db.SaveChanges();
            TempData["message"] = new MessegeAlert("success", "Thành công");
            return RedirectToAction("Index");//cập nhật và chuyển hướng trang
        }
        // GET: Admin/product/Restore/5
        //Khôi phục Status=2
        
        public ActionResult Restore(int? id)
        {
            Product product = db.Products.Find(id); // lấy ra 1 chi tiết mẫu tin
            product.Status = 2;
            product.UpdatedAt = DateTime.Now;
            product.UpdatedBy = 1;
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Trash", "Product");//cập nhật và chuyển hướng trang
        }
        // GET: Admin/product/Trash
        //hiện danh sách rác của danh mục
       
        public ActionResult Trash()
        {
            var list = db.Products.Where(x =>x.Status == 0).ToList();
            return View("Trash", list);
        }
        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, isAcive = item.IsActive });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult IsHome(int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                item.IsHome = !item.IsHome;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, isHome = item.IsHome });
            }
            return Json(new { success = false });
        }
    }
}
