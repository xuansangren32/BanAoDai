using MyClass.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanAoDai.Controllers
{
    public class PostController : Controller
    {
        // GET: Post
        private BanAoDaiDBContext db=new BanAoDaiDBContext();
        public ActionResult Index(int? page)
        {
            var pageSize = 4;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Post> items = db.Posts.OrderByDescending(x=>x.CreatedBy_At);
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
           
            return View(items);
        }
        public ActionResult Detail(int id)
        {
            var item = db.Posts.Find(id);
            return View(item);
        }
        public ActionResult Post_Home()
        {
            var item = db.Posts.Take(3).ToList();
            return PartialView(item);
        }
    }
}