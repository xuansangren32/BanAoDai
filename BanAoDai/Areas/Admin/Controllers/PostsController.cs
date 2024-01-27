using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using BanAoDai.Libraries;
using MyClass.Model;

namespace BanAoDai.Areas.Admin.Controllers
{
    public class PostsController : Controller
    {
        private BanAoDaiDBContext db = new BanAoDaiDBContext();

        // GET: Admin/Post
        public ActionResult Index()
        {   //select + from Topics where status!=0
            var list = db.Posts.Where(x => x.Status != 0).ToList();
            return View("Index", list);
        }

        // GET: Admin/Post/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Admin/post/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.ListCatId = new SelectList(db.Categorys.Where(x => x.Status != 0).ToList(), "Id", "Name");
            ViewBag.ListOrder = new SelectList(db.Categorys.Where(x => x.Status != 0).ToList(), "Orders", "Name");
            return View();
        }

        // POST: Admin/post/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Post post)
        {
            if (ModelState.IsValid)
            {
                //Upload file
                post.Slug = MyString.str_Slug(post.Title);
                var file = Request.Files["Img"];
                string[] Extention = { ".jpg", ".png", ".gif" };
                //if (file != null)
                if (file.ContentLength>0)
                {
                    //có chọn file
                    var extention = file.FileName.Substring(file.FileName.LastIndexOf("."));
                    if (Extention.Contains(extention))
                    {
                        //hợp lệ
                        //đưa tập tin lên server
                        var fileName = Path.Combine(Server.MapPath("~/Public/images/"), file.FileName);
                        file.SaveAs(fileName);
                        post.Img = file.FileName;
                        //lưu lại kết quả vào csdl

                    }
                    else
                    {
                        TempData["messege"] = new MessegeAlert("danger", "Định dạng tập tin không hợp lệ");
                        return RedirectToAction("Create", "Posts");

                    }
                }
                else
                {
                    TempData["messege"] = new MessegeAlert("success", "Chưa chọn tập tin");
                    return RedirectToAction("Create", "Posts");

                }
                //End upload file

                post.CreatedBy_At = DateTime.Now;
                post.CreatedBy = (Session["UserAdmin_id"].ToString() != "") ? int.Parse(Session["UserAdmin_id"].ToString()) : 1;
                db.Posts.Add(post);
                db.SaveChanges();

                if (db.SaveChanges() != 0)
                {
                    Link link = new Link();
                    link.Slug = post.Slug;
                    link.TypeLink = "post";
                    link.TableId = post.Id;
                    db.Links.Add(link);
                    db.SaveChanges();
                }
                TempData["messege"] = new MessegeAlert("success", "Thành Công");
                return RedirectToAction("Index");
            }
           
            return View(post);
        }

        // GET: Admin/post/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListCatId = new SelectList(db.Posts.Where(x => x.Status != 0).ToList(), "Id", "Name");
            ViewBag.ListOrder = new SelectList(db.Posts.Where(x => x.Status != 0).ToList(), "Orders", "Name");
            return View(post);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                //Upload file
                //post.Slug = MyString.str_Slug(post.Title);
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
                        post.Img = file.FileName;
                        //lưu lại kết quả vào csdl

                    }
                    else
                    {
                        TempData["messege"] = new MessegeAlert("danger", "Định dạng tập tin không hợp lệ");
                        return RedirectToAction("Create", "Banner");

                    }
                }
                else
                {
                    TempData["messege"] = new MessegeAlert("success", "Chưa chọn tập tin");
                    return RedirectToAction("Create", "Banner");

                }
                post.Title = MyString.str_Slug(post.Title);
                post.UpdatedBy = (Session["UserAdmin_id"].ToString() != "") ? int.Parse(Session["UserAdmin_id"].ToString()) : 1;
                post.UpdatedAt = DateTime.Now;

                db.Entry(post).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.ListCatId = new SelectList(db.Categorys.Where(x => x.Status != 0).ToList(), "Id", "Name");
            ViewBag.ListOrder = new SelectList(db.Categorys.Where(x => x.Status != 0).ToList(), "Orders", "Name");
            return View(post);
        }

        // GET: Admin/post/Destroy/5
        // xóa khỏi csdl
        public ActionResult Destroy(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View("Destroy", post);
        }

        // POST: Admin/post/Destroy/5
        [HttpPost, ActionName("Destroy")]
        [ValidateAntiForgeryToken]
        public ActionResult DestroyConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db. Posts.Remove(post);
            if (db.SaveChanges() != 0)
            {
                Link link = db.Links.Where(x => x.TypeLink == "post" && x.TableId == id).FirstOrDefault();
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
        // GET: Admin/post/Delete/5
        // xóa vào thùng rác status=0
        public ActionResult Delete(int? id)
        {
            Post post = db.Posts.Find(id);
            post.Status = 0;
            post.UpdatedBy = (Session["UserAdmin_id"].ToString() != "") ? int.Parse(Session["UserAdmin_id"].ToString()) : 1;
            post.UpdatedAt = DateTime.Now;
            db.Entry(post).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Admin/post/status/5
        // thay đổi trạng thái
        public ActionResult Status(int? id)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                TempData["messege"] = new MessegeAlert("danger", "Thư mục không tồn tại");
                return RedirectToAction("Index");
            }
            post.Status = (post.Status == 2) ? 1 : 2;
            post.UpdatedBy = (Session["UserAdmin_id"].ToString() != "") ? int.Parse(Session["UserAdmin_id"].ToString()) : 1;
            post.UpdatedAt = DateTime.Now;
            db.Entry(post).State = EntityState.Modified;
            db.SaveChanges();
            TempData["messege"] = new MessegeAlert("success", "Thành Công");
            return RedirectToAction("Index");
        }

        // GET: Admin/post/Restore/5
        // khôi phục staatus =2

        public ActionResult Restore(int? id)
        {
            Post post = db.Posts.Find(id);
            post.Status = 2;
            post.UpdatedBy = (Session["UserAdmin_id"].ToString() != "") ? int.Parse(Session["UserAdmin_id"].ToString()) : 1;
            post.UpdatedAt = DateTime.Now;
            db.Entry(post).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Trash", "Posts");
        }
        // GET: Admin/post/trash/5
        // hiện danh sách rác
        public ActionResult Trash()
        {
            var list = db.Posts.Where(x => x.Status == 0).ToList();
            return View("Trash", list);
        }

    }
}
