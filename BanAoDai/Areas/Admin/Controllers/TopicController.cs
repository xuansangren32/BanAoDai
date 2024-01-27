using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BanAoDai.Libraries;
using MyClass.Model;

namespace BanAoDai.Areas.Admin.Controllers
{
    public class TopicController : BaseController
    {
        private BanAoDaiDBContext db = new BanAoDaiDBContext();

        // GET: Admin/Topic
        public ActionResult Index()
        {   //select + from categorys where status!=0
            var list = db.Topics.Where(x => x.Status != 0).ToList();
            return View("Index", list);
        }

        // GET: Admin/Topic/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // GET: Admin/Topic/Create
        [HttpGet]
        public ActionResult Create()
        {
           
            return View();
        }

        // POST: Admin/Topics/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public ActionResult Create(Topic topic)
        {
            if (ModelState.IsValid)
            {
                //topic.Slug = MyString.str_Slug(topic.Name);

                topic.CreatedBy = (Session["UserAdmin_id"].ToString() != "") ? int.Parse(Session["UserAdmin_id"].ToString()) : 1;
                topic.CreatedBy_At = DateTime.Now;

                //if (topic.Orders == null)
                //{
                //    topic.Orders = 1;
                //}
                //else
                //{
                //    topic.Orders += 1;
                //}
                //if (topic.ParentId == null)
                //{
                //    topic.ParentId = 0;
                //}
                db.Topics.Add(topic);
                db.SaveChanges();
                if (db.SaveChanges() != 0)
                {
                    Link link = new Link();
                    link.Slug = topic.Slug;
                    link.TypeLink = "topic";
                    link.TableId = topic.Id;
                    db.Links.Add(link);

                    db.SaveChanges();
                }
                TempData["messege"] = new MessegeAlert("success", " Thêm Danh Mục Thành Công");
                return RedirectToAction("Index");
            }
         
            return View(topic);
        }

        // GET: Admin/Topics/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
           
            return View(topic);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Topic topic)
        {
            if (ModelState.IsValid)
            {//Lưu
                topic.Slug = MyString.str_Slug(topic.Name);
                topic.UpdatedBy = (Session["UserAdmin_id"].ToString() != "") ? int.Parse(Session["UserAdmin_id"].ToString()) : 1;
                topic.UpdatedAt = DateTime.Now;

                if (topic.Orders == null)
                {
                    topic.Orders = 1;
                }
                else
                {
                    topic.Orders += 1;
                }
                if (topic.ParentId == null)
                {
                    topic.ParentId = 0;
                }
                db.Entry(topic).State = EntityState.Modified;
                if (db.SaveChanges() != 0)
                {
                    Link link = db.Links.Where(x => x.TypeLink == "topic" && x.TableId== topic.Id).FirstOrDefault();
                    link.Slug = topic.Slug;
                    link.TypeLink = "topic";
                    link.TableId = topic.Id;
                    db.Links.Add(link);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
          
            return View(topic);
        }

        // GET: Admin/Topics/Destroy/5
        // xóa khỏi csdl
        public ActionResult Destroy(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View("Destroy", topic);
        }

        // POST: Admin/Topics/Destroy/5
        [HttpPost, ActionName("Destroy")]
        [ValidateAntiForgeryToken]
        public ActionResult DestroyConfirmed(int id)
        {
            Topic topic = db.Topics.Find(id);
            db.Topics.Remove(topic);
            if (db.SaveChanges()!=0)
            {
                Link link = db.Links.Where(x => x.TypeLink == "topic" && x.TableId == id).FirstOrDefault();
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
        // GET: Admin/topic/Delete/5
        // xóa vào thùng rác status=0
        public ActionResult Delete(int? id)
        {
            Topic topic = db.Topics.Find(id);
            topic.Status = 0;
            topic.UpdatedBy = (Session["UserAdmin_id"].ToString() != "") ? int.Parse(Session["UserAdmin_id"].ToString()) : 1;
            topic.UpdatedAt = DateTime.Now;
            db.Entry(topic).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Admin/topic/status/5
        // thay đổi trạng thái
        public ActionResult Status(int? id)
        {
            Topic topic = db.Topics.Find(id);
            if(topic == null)
            {
                TempData["messege"] = new MessegeAlert("danger", "Thư mục không tồn tại");
                return RedirectToAction("Index");
            }
            topic.Status = (topic.Status == 2) ? 1 : 2;
            topic.UpdatedBy = (Session["UserAdmin_id"].ToString() != "") ? int.Parse(Session["UserAdmin_id"].ToString()) : 1;
            topic.UpdatedAt = DateTime.Now;
            db.Entry(topic).State = EntityState.Modified;
            db.SaveChanges();
            TempData["messege"] = new MessegeAlert("success", "Thành Công");
            return RedirectToAction("Index");
        }

        // GET: Admin/topic/Restore/5
        // khôi phục staatus =2

        public ActionResult Restore(int? id)
        {
            Topic topic = db.Topics.Find(id);
            topic.Status =   2;
            topic.UpdatedBy = (Session["UserAdmin_id"].ToString() != "") ? int.Parse(Session["UserAdmin_id"].ToString()) : 1;
            topic.UpdatedAt = DateTime.Now;
            db.Entry(topic).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Trash","Topic");
        }
        // GET: Admin/topic/trash/5
        // hiện danh sách rác
        public ActionResult Trash()
        {   
            var list = db.Topics.Where(x => x.Status == 0).ToList();
            return View("Trash", list);
        }
    }
}
