using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using BanAoDai.Libraries;
using MyClass.Model;

namespace BanAoDai.Areas.Admin.Controllers
{
    public class BrandController : BaseController
    {
        private BanAoDaiDBContext db = new BanAoDaiDBContext();

        // GET: Admin/Brands
        public ActionResult Index()
        {   //select + from Brand where status!=0
            var list = db.Brands.Where(x => x.Status != 0).ToList();
            return View("Index", list);
        }

        // GET: Admin/Brands/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // GET: Admin/brand/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.ListCatId = new SelectList(db.Categorys.Where(x => x.Status != 0).ToList(), "Id", "Name");
            ViewBag.ListOrder = new SelectList(db.Categorys.Where(x => x.Status != 0).ToList(), "Orders", "Name");
            return View();
        }

        // POST: Admin/Brands/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Brand brand)
        {
            if (ModelState.IsValid)
            {
                //Upload file
                var file = Request.Files["Img"];
                string[] Extention = { ".jpg", ".png", ".gif" };
                //if (file != null)
                if (file.ContentLength > 0)
                {
                    //có chọn file
                    var extention = file.FileName.Substring(file.FileName.LastIndexOf("."));
                    if (Extention.Contains(extention))
                    {
                        //hợp lệ
                        //đưa tập tin lên server
                        var fileName = Path.Combine(Server.MapPath("~/Public/images/"), file.FileName);
                        file.SaveAs(fileName);
                        brand.Img = file.FileName;
                        //lưu lại kết quả vào csdl

                    }
                    else
                    {
                        TempData["messege"] = new MessegeAlert("danger", "Định dạng tập tin không hợp lệ");
                        return RedirectToAction("Create", "Brand");

                    }
                }
                else
                {
                    TempData["messege"] = new MessegeAlert("success", "Chưa chọn tập tin");
                    return RedirectToAction("Create", "Brand");

                }
                brand.Slug = MyString.str_Slug(brand.Name);
                brand.CreatedBy = (Session["UserAdmin_id"].ToString() != "") ? int.Parse(Session["UserAdmin_id"].ToString()) : 1;
                brand.CreatedBy_At = DateTime.Now;

                if (brand.Orders == null)
                {
                    brand.Orders = 1;
                }
                else
                {
                    brand.Orders += 1;
                }
                //if (brand.ParentId == null)
                //{
                //    brand.ParentId = 0;
                //}
                db.Brands.Add(brand);

                if (db.SaveChanges() != 0)
                {
                    Link link = new Link();
                    link.Slug = brand.Slug;
                    link.TypeLink = "brand";
                    link.TableId = brand.Id;
                    db.Links.Add(link);
                    db.SaveChanges();
                }
                TempData["messege"] = new MessegeAlert("success", "Thành Công");
                return RedirectToAction("Index");
            }
            ViewBag.ListCatId = new SelectList(db.Categorys.Where(x => x.Status != 0).ToList(), "Id", "Name");
            ViewBag.ListOrder = new SelectList(db.Categorys.Where(x => x.Status != 0).ToList(), "Orders", "Name");
            return View(brand);
        }

        // GET: Admin/brand/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListCatId = new SelectList(db.Brands.Where(x => x.Status != 0).ToList(), "Id", "Name");
            ViewBag.ListOrder = new SelectList(db.Brands.Where(x => x.Status != 0).ToList(), "Orders", "Name");
            return View(brand);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Brand brand)
        {
            if (ModelState.IsValid)
            {//Lưu
             //Upload file
                var file = Request.Files["Img"];
                string[] Extention = { ".jpg", ".png", ".gif" };
                //if (file != null)
                if (file.ContentLength > 0)
                {
                    //có chọn file
                    var extention = file.FileName.Substring(file.FileName.LastIndexOf("."));
                    if (Extention.Contains(extention))
                    {
                        //hợp lệ
                        //đưa tập tin lên server
                        var fileName = Path.Combine(Server.MapPath("~/Public/images/"), file.FileName);
                        file.SaveAs(fileName);
                        brand.Img = file.FileName;
                        //lưu lại kết quả vào csdl

                    }
                    else
                    {
                        TempData["messege"] = new MessegeAlert("danger", "Định dạng tập tin không hợp lệ");
                        return RedirectToAction("Create", "Brand");

                    }
                }
                else
                {
                    TempData["messege"] = new MessegeAlert("success", "Chưa chọn tập tin");
                    return RedirectToAction("Create", "Brand");

                }
                brand.Slug = MyString.str_Slug(brand.Name);
                brand.UpdatedBy = (Session["UserAdmin_id"].ToString() != "") ? int.Parse(Session["UserAdmin_id"].ToString()) : 1;
                brand.UpdatedAt = DateTime.Now;

                if (brand.Orders == null)
                {
                    brand.Orders = 1;
                }
                else
                {
                    brand.Orders += 1;
                }
                //if (brand.ParentId == null)
                //{
                //    brand.ParentId = 0;
                //}\
                db.Brands.Attach(brand);
                db.Entry(brand).State = EntityState.Modified;
                if (db.SaveChanges() != 0)
                {
                    Link link = db.Links.Where(x => x.TypeLink == "brand" && x.TableId == brand.Id).FirstOrDefault();
                    link.Slug = brand.Slug;
                    link.TypeLink = "brand";
                    link.TableId = brand.Id;
                    db.Links.Add(link);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            ViewBag.ListCatId = new SelectList(db.Brands.Where(x => x.Status != 0).ToList(), "Id", "Name");
            ViewBag.ListOrder = new SelectList(db.Brands.Where(x => x.Status != 0).ToList(), "Orders", "Name");
            return View(brand);
        }

        // GET: Admin/Brands/Destroy/5
        // xóa khỏi csdl
        public ActionResult Destroy(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View("Destroy", brand);
        }

        // POST: Admin/brand/Destroy/5
        [HttpPost, ActionName("Destroy")]
        [ValidateAntiForgeryToken]
        public ActionResult DestroyConfirmed(int id)
        {
            Brand brand = db.Brands.Find(id);
            db.Brands.Remove(brand);
            if (db.SaveChanges() != 0)
            {
                Link link = db.Links.Where(x => x.TypeLink == "brand" && x.TableId == id).FirstOrDefault();
                db.Links.Remove(link);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        // GET: Admin/brand/Delete/5
        // xóa vào thùng rác status=0
        public ActionResult Delete(int? id)
        {
            Brand brand = db.Brands.Find(id);
            brand.Status = 0;
            brand.UpdatedBy = (Session["UserAdmin_id"].ToString() != "") ? int.Parse(Session["UserAdmin_id"].ToString()) : 1;
            brand.UpdatedAt = DateTime.Now;
            db.Entry(brand).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Admin/brand/status/5
        // thay đổi trạng thái
        public ActionResult Status(int? id)
        {
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                TempData["messege"] = new MessegeAlert("danger", "Thư mục không tồn tại");
                return RedirectToAction("Index");
            }
            brand.Status = (brand.Status == 2) ? 1 : 2;
            brand.UpdatedBy = (Session["UserAdmin_id"].ToString() != "") ? int.Parse(Session["UserAdmin_id"].ToString()) : 1;
            brand.UpdatedAt = DateTime.Now;
            db.Entry(brand).State = EntityState.Modified;
            db.SaveChanges();
            TempData["messege"] = new MessegeAlert("success", "Thành Công");
            return RedirectToAction("Index");
        }

        // GET: Admin/brand/Restore/5
        // khôi phục staatus =2

        public ActionResult Restore(int? id)
        {
            Brand brand = db.Brands.Find(id);
            brand.Status = 2;
            brand.UpdatedBy = (Session["UserAdmin_id"].ToString() != "") ? int.Parse(Session["UserAdmin_id"].ToString()) : 1;
            brand.UpdatedAt = DateTime.Now;
            db.Entry(brand).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Trash", "Brand");
        }
        // GET: Admin/brand/trash/5
        // hiện danh sách rác
        public ActionResult Trash()
        {
            var list = db.Brands.Where(x => x.Status == 0).ToList();
            return View("Trash", list);
        }
      
    }
}
