using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BanAoDai.Libraries;
using MyClass.Model;
using PagedList;

namespace BanAoDai.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        private BanAoDaiDBContext db = new BanAoDaiDBContext();

        // GET: Admin/Category
        public ActionResult Index()
        {   //select + from categorys where status!=0




            var list = db.Categorys.Where(x => x.Status != 0).ToList();
            return View("Index", list);
        }

        // GET: Admin/Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categorys.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Admin/Category/Create
        [HttpGet]

        public ActionResult Create()
        {
            ViewBag.ListCatId = new SelectList(db.Categorys.Where(x => x.Status != 0).ToList(), "Id", "Name");
            ViewBag.ListOrder = new SelectList(db.Categorys.Where(x => x.Status != 0).ToList(), "Orders", "Name");
            return View();
        }

        // POST: Admin/Category/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = MyString.str_Slug(category.Name);
                category.CreatedBy = (Session["UserAdmin_id"].ToString() != "") ? int.Parse(Session["UserAdmin_id"].ToString()):1 ;
                category.CreatedBy_At = DateTime.Now;
             
                if (category.Orders == null)
                {
                    category.Orders = 1;
                }
                else
                {
                    category.Orders += 1; 
                }
                if (category.ParentId == null)
                {
                    category.ParentId = 0;
                }
                db.Categorys.Add(category);

                if (db.SaveChanges() !=0)
                {
                    Link link= new Link();
                    link.Slug=category.Slug;
                    link.TypeLink = "category";
                    link.TableId= category.Id;
                    db.Links.Add(link);
                    db.SaveChanges();
                }
                TempData["messege"] = new MessegeAlert("success", " Thêm Danh Mục Thành Công");
                return RedirectToAction("Index");
            }
            ViewBag.ListCatId = new SelectList(db.Categorys.Where(x => x.Status != 0).ToList(), "Id", "Name");
            ViewBag.ListOrder = new SelectList(db.Categorys.Where(x => x.Status != 0).ToList(), "Orders", "Name");
            return View(category);
        }

        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categorys.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListCatId = new SelectList(db.Categorys.Where(x => x.Status != 0).ToList(), "Id", "Name");
            ViewBag.ListOrder = new SelectList(db.Categorys.Where(x => x.Status != 0).ToList(), "Orders", "Name");
            return View(category);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {//Lưu
                category.Slug = MyString.str_Slug(category.Name);
                category.UpdatedBy = (Session["UserAdmin_id"].ToString() != "") ? int.Parse(Session["UserAdmin_id"].ToString()) : 1;
                category.UpdatedAt = DateTime.Now;

                if (category.Orders == null)
                {
                    category.Orders = 1;
                }
                else
                {
                    category.Orders += 1;
                }
                if (category.ParentId == null)
                {
                    category.ParentId = 0;
                }
                db.Entry(category).State = EntityState.Modified;
                if (db.SaveChanges() != 0)
                {
                    Link link = db.Links.Where(x => x.TypeLink == "category" && x.TableId==category.Id).FirstOrDefault();
                    link.Slug = category.Slug;
                    link.TypeLink = "category";
                    link.TableId = category.Id;
                    db.Entry(link).State = EntityState.Modified;
                    //db.Links.Add(link);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            ViewBag.ListCatId = new SelectList(db.Categorys.Where(x => x.Status != 0).ToList(), "Id", "Name");
            ViewBag.ListOrder = new SelectList(db.Categorys.Where(x => x.Status != 0).ToList(), "Orders", "Name");
            return View(category);
        }

        // GET: Admin/Category/Destroy/5
        // xóa khỏi csdl
        public ActionResult Destroy(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categorys.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View("Destroy",category);
        }

        // POST: Admin/Category/Destroy/5
        [HttpPost, ActionName("Destroy")]
        [ValidateAntiForgeryToken]
        public ActionResult DestroyConfirmed(int id)
        {
            Category category = db.Categorys.Find(id);
            db.Categorys.Remove(category);
            if (db.SaveChanges()!=0)
            {
                Link link = db.Links.Where(x => x.TypeLink == "category" && x.TableId == id).FirstOrDefault();
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
        // GET: Admin/Category/Delete/5
        // xóa vào thùng rác status=0
        public ActionResult Delete(int? id)
        {
            Category category = db.Categorys.Find(id);
            category.Status = 0;
            category.UpdatedBy = (Session["UserAdmin_id"].ToString() != "") ? int.Parse(Session["UserAdmin_id"].ToString()) : 1;
            category.UpdatedAt = DateTime.Now;
            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Admin/Category/status/5
        // thay đổi trạng thái
       
        public ActionResult Status(int? id)
        {
            Category category = db.Categorys.Find(id);
            if(category== null)
            {
                TempData["messege"] = new MessegeAlert("danger", "Thư mục không tồn tại");
                return RedirectToAction("Index");
            }
            category.Status = (category.Status == 2) ? 1 : 2;
            category.UpdatedBy = (Session["UserAdmin_id"].ToString() != "") ? int.Parse(Session["UserAdmin_id"].ToString()) : 1;
            category.UpdatedAt = DateTime.Now;
            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();
            TempData["messege"] = new MessegeAlert("success", "Đổi Trạng Thái Thành Công");
            return RedirectToAction("Index");
        }

        // GET: Admin/Category/Restore/5
        // khôi phục staatus =2

        public ActionResult Restore(int? id)
        {
            Category category = db.Categorys.Find(id);
            category.Status =   2;
            category.UpdatedBy = (Session["UserAdmin_id"].ToString() != "") ? int.Parse(Session["UserAdmin_id"].ToString()) : 1;
            category.UpdatedAt = DateTime.Now;
            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Trash","Category");
        }
        // GET: Admin/Category/trash/5
        // hiện danh sách rác
        public ActionResult Trash()
        {   
            var list = db.Categorys.Where(x => x.Status == 0).ToList();
            return View("Trash", list);
        }
    }
}
