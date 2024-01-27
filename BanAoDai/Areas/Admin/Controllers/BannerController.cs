
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BanAoDai.Libraries;
using MyClass.Model;

namespace BanAoDai.Areas.Admin.Controllers
{
    public class BannerController : Controller
    {
        private BanAoDaiDBContext db = new BanAoDaiDBContext();

        // GET: Admin/Banners
        public ActionResult Index()
        {
            var list=db.Banners.Where(x=>x.Status!=0).OrderByDescending(x=>x.CreatedBy).ToList();
            return View("Index",list);
        }

        // GET: Admin/Banners/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner banner = db.Banners.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }

        // GET: Admin/Banners/Create
        public ActionResult Create()
        {
            return View();
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(Banner banner)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //uploand file
        //        var file = Request.Files["Img"];
        //        string[] Extention = { ".jpg", ".png", ".gif" };
        //        if (file!=null)
        //        {
        //            var extention = file.FileName.Substring(file.FileName.LastIndexOf("."));
        //            if (Extention.Contains(extention))
        //            {
        //                //hop le
        //                var fileName = Path.Combine(Server.MapPath("~/Public/images/banners/"), file.FileName);
        //                file.SaveAs(fileName);
        //                banner.Img = file.FileName;
        //                banner.CreatedBy = (Session["UserAdmin_id"].ToString() != "") ? int.Parse(Session["UserAdmin_id"].ToString()) : 1;
        //                banner.CreatedBy_At = DateTime.Now;
        //                db.Banners.Add(banner);
        //                db.SaveChanges();
        //                TempData["messege"] = new MessegeAlert("success", "Thành Công");
        //                return RedirectToAction("Index");

        //            }
        //            else
        //            {
        //                //ko hop le
        //                TempData["message"] = new MessegeAlert("danger", "Định dạng tập tin không hợp lệ");
        //                return RedirectToAction("Create", "Banner");
        //            }
        //        }
        //        else
        //        {
        //            TempData["message"] = new MessegeAlert("success", "Chưa chọn tập tin");
        //            return RedirectToAction("Create", "Banner");
        //        }


        //    }

        //    return View(banner);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Banner banner)
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
                        banner.Img = file.FileName;
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
                //End upload file
             
                banner.CreatedBy_At = DateTime.Now;
                banner.CreatedBy = (Session["UserAdmin_id"].ToString() != "") ? int.Parse(Session["UserAdmin_id"].ToString()) : 1;
                db.Banners.Add(banner);
                db.SaveChanges();
                TempData["message"] = new MessegeAlert("success", "Thành công");
                return RedirectToAction("Index");
            }
            return View(banner);
        }
        // GET: Admin/Banner/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner banner = db.Banners.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListCatId = new SelectList(db.Banners.Where(x => x.Status != 0).ToList(), "Id", "Name");
            ViewBag.ListOrder = new SelectList(db.Banners.Where(x => x.Status != 0).ToList(), "Orders", "Name");
            return View(banner);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Banner banner)
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
                        banner.Img = file.FileName;
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
                banner.Link = MyString.str_Slug(banner.Name);
                banner.UpdatedBy = (Session["UserAdmin_id"].ToString() != "") ? int.Parse(Session["UserAdmin_id"].ToString()) : 1;
                banner.UpdatedAt = DateTime.Now;

                db.Entry(banner).State = EntityState.Modified;
                
                    db.SaveChanges();
                
                return RedirectToAction("Index");
            }
            ViewBag.ListCatId = new SelectList(db.Categorys.Where(x => x.Status != 0).ToList(), "Id", "Name");
            ViewBag.ListOrder = new SelectList(db.Categorys.Where(x => x.Status != 0).ToList(), "Orders", "Name");
            return View(banner);
        }

        // GET: Admin/banner/Destroy/5
        // xóa khỏi csdl
        public ActionResult Destroy(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner banner = db.Banners.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            return View("Destroy", banner);
        }

        // POST: Admin/banner/Destroy/5
        [HttpPost, ActionName("Destroy")]
        [ValidateAntiForgeryToken]
        public ActionResult DestroyConfirmed(int id)
        {
            Banner banner = db.Banners.Find(id);
            db.Banners.Remove(banner);
            if (db.SaveChanges() != 0)
            {
                Link link = db.Links.Where(x => x.TypeLink == "banner" && x.TableId == id).FirstOrDefault();
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
        // GET: Admin/banner/Delete/5
        // xóa vào thùng rác status=0
        public ActionResult Delete(int? id)
        {
            Banner banner = db.Banners.Find(id);
            banner.Status = 0;
            banner.UpdatedBy = (Session["UserAdmin_id"].ToString() != "") ? int.Parse(Session["UserAdmin_id"].ToString()) : 1;
            banner.UpdatedAt = DateTime.Now;
            db.Entry(banner).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Admin/banner/status/5
        // thay đổi trạng thái

        public ActionResult Status(int? id)
        {
            Banner banner = db.Banners.Find(id);
            if (banner == null)
            {
                TempData["messege"] = new MessegeAlert("danger", "Thư mục không tồn tại");
                return RedirectToAction("Index");
            }
            banner.Status = (banner.Status == 2) ? 1 : 2;
            banner.UpdatedBy = (Session["UserAdmin_id"].ToString() != "") ? int.Parse(Session["UserAdmin_id"].ToString()) : 1;
            banner.UpdatedAt = DateTime.Now;
            db.Entry(banner).State = EntityState.Modified;
            db.SaveChanges();
            TempData["messege"] = new MessegeAlert("success", "Đổi Trạng Thái Thành Công");
            return RedirectToAction("Index");
        }

        // GET: Admin/banner/Restore/5
        // khôi phục staatus =2

        public ActionResult Restore(int? id)
        {
            Banner banner = db.Banners.Find(id);
            banner.Status = 2;
            banner.UpdatedBy = (Session["UserAdmin_id"].ToString() != "") ? int.Parse(Session["UserAdmin_id"].ToString()) : 1;
            banner.UpdatedAt = DateTime.Now;
            db.Entry(banner).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Trash", "Banner");
        }
        // GET: Admin/banner/trash/5
        // hiện danh sách rác
        public ActionResult Trash()
        {
            var list = db.Banners.Where(x => x.Status == 0).ToList();
            return View("Trash", list);
        }
    }
}
