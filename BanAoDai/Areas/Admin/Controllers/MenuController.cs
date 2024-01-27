using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BanAoDai.Libraries;
using MyClass.Model;

namespace BanAoDai.Areas.Admin.Controllers
{
    public class MenuController : BaseController
    {
        private BanAoDaiDBContext db = new BanAoDaiDBContext();
        Category category= new Category();
        Post post=new Post();
        Brand brand=new Brand();


        // GET: Admin/Menu
        public ActionResult Index()
        {   //select + from Menu where status!=0



            //ViewBag.ListBrnd=brand.
            var list = db.Menus.Where(x => x.Status != 0).ToList();
            return View("Index", list);
        }

        // GET: Admin/Menu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // GET: Admin/Menu/Create
        [HttpGet]

        public ActionResult Create()
        {
            ViewBag.ListCatId = new SelectList(db.Menus.Where(x => x.Status != 0).ToList(), "Id", "Name");
            ViewBag.ListOrder = new SelectList(db.Menus.Where(x => x.Status != 0).ToList(), "Orders", "Name");
            return View();
        }

        // POST: Admin/Menu/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Menu menu)
        {
            if (ModelState.IsValid)
            {
                menu.Slug = MyString.str_Slug(menu.Name);
                menu.Link = MyString.str_Slug(menu.Name);
                menu.CreatedBy = (Session["UserAdmin_id"].ToString() != "") ? int.Parse(Session["UserAdmin_id"].ToString()) : 1;
                menu.CreatedBy_At = DateTime.Now;

                if (menu.Orders == null)
                {
                    menu.Orders = 1;
                }
                else
                {
                    menu.Orders += 1;
                }
                if (menu.ParentId == null)
                {
                    menu.ParentId = 0;
                }
                db.Menus.Add(menu);

                if (db.SaveChanges() != 0)
                {
                    Link link = new Link();
                    link.Slug = menu.Slug;
                    link.TypeLink = "menu";
                    link.TableId = menu.Id;
                    db.Links.Add(link);
                  
                    db.SaveChanges();
                }
                TempData["messege"] = new MessegeAlert("success", " Thêm Danh Mục Thành Công");
                return RedirectToAction("Index");
            }
            ViewBag.ListCatId = new SelectList(db.Menus.Where(x => x.Status != 0).ToList(), "Id", "Name");
            ViewBag.ListOrder = new SelectList(db.Menus.Where(x => x.Status != 0).ToList(), "Orders", "Name");
            return View(menu);
        }

        // GET: Admin/Menu/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListCatId = new SelectList(db.Menus.Where(x => x.Status != 0).ToList(), "Id", "Name");
            ViewBag.ListOrder = new SelectList(db.Menus.Where(x => x.Status != 0).ToList(), "Orders", "Name");
            return View(menu);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Menu menu)
        {
            if (ModelState.IsValid)
            {//Lưu
                menu.Slug = MyString.str_Slug(menu.Name);
                menu.Link = MyString.str_Slug(menu.Name);
                menu.UpdatedBy = (Session["UserAdmin_id"].ToString() != "") ? int.Parse(Session["UserAdmin_id"].ToString()) : 1;
                menu.UpdatedAt = DateTime.Now;

                if (menu.Orders == null)
                {
                    menu.Orders = 1;
                }
                else
                {
                    menu.Orders += 1;
                }
                if (menu.ParentId == null)
                {
                    menu.ParentId = 0;
                }
                db.Entry(menu).State = EntityState.Modified;
                if (db.SaveChanges() != 0)
                {
                    Link link = db.Links.Where(x => x.TypeLink == "menu" && x.TableId == menu.Id).FirstOrDefault();
                    link.Slug = menu.Slug;
                    link.Slug = menu.Link;
                    link.TypeLink = "menu";
                    link.TableId = menu.Id;
                    db.Entry(link).State = EntityState.Modified;
                    //db.Links.Add(link);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            ViewBag.ListCatId = new SelectList(db.Menus.Where(x => x.Status != 0).ToList(), "Id", "Name");
            ViewBag.ListOrder = new SelectList(db.Menus.Where(x => x.Status != 0).ToList(), "Orders", "Name");
            return View(menu);
        }

        // GET: Admin/Menu/Destroy/5
        // xóa khỏi csdl
        public ActionResult Destroy(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View("Destroy", menu);
        }

        // POST: Admin/Menu/Destroy/5
        [HttpPost, ActionName("Destroy")]
        [ValidateAntiForgeryToken]
        public ActionResult DestroyConfirmed(int id)
        {
            Menu menu = db.Menus.Find(id);
            db.Menus.Remove(menu);
            if (db.SaveChanges() != 0)
            {
                Link link = db.Links.Where(x => x.TypeLink == "menu" && x.TableId == id).FirstOrDefault();
                db.Links.Remove(link);
                db.SaveChanges();
            }
            return RedirectToAction("Trash");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        // GET: Admin/menu/Delete/5
        // xóa vào thùng rác status=0
        public ActionResult Delete(int? id)
        {
            Menu menu = db.Menus.Find(id);
            menu.Status = 0;
            menu.UpdatedBy = (Session["UserAdmin_id"].ToString() != "") ? int.Parse(Session["UserAdmin_id"].ToString()) : 1;
            menu.UpdatedAt = DateTime.Now;
            db.Entry(menu).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Admin/menu/status/5
        // thay đổi trạng thái

        public ActionResult Status(int? id)
        {
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                TempData["messege"] = new MessegeAlert("danger", "Thư mục không tồn tại");
                return RedirectToAction("Index");
            }
            menu.Status = (menu.Status == 2) ? 1 : 2;
            menu.UpdatedBy = (Session["UserAdmin_id"].ToString() != "") ? int.Parse(Session["UserAdmin_id"].ToString()) : 1;
            menu.UpdatedAt = DateTime.Now;
            db.Entry(menu).State = EntityState.Modified;
            db.SaveChanges();
            TempData["messege"] = new MessegeAlert("success", "Đổi Trạng Thái Thành Công");
            return RedirectToAction("Index");
        }

        // GET: Admin/menu/Restore/5
        // khôi phục staatus =2

        public ActionResult Restore(int? id)
        {
            Menu menu = db.Menus.Find(id);
            menu.Status = 2;
            menu.UpdatedBy = (Session["UserAdmin_id"].ToString() != "") ? int.Parse(Session["UserAdmin_id"].ToString()) : 1;
            menu.UpdatedAt = DateTime.Now;
            db.Entry(menu).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Trash", "Menu");
        }
        // GET: Admin/menu/trash/5
        // hiện danh sách rác
        public ActionResult Trash()
        {
            var list = db.Menus.Where(x => x.Status == 0).ToList();
            return View("Trash", list);
        }
    }
}
