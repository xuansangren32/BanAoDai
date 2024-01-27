using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanAoDai.Controllers
{
    public class ModuleController : Controller
    {
        BanAoDaiDBContext db=new BanAoDaiDBContext();
        // GET: Module
        public ActionResult MainMenu()
        {
            var list=db.Menus.Where(x=>x.Position== "mainmenu" && x.Status==1 && x.ParentId == 0)
                .OrderBy(x=>x.Orders).ToList();
            return View("MainMenu", list);
        }
        public ActionResult BannerShow()
        {
            var list = db.Banners.Where(x => x.Position == "BannerShow" && x.Status == 1 )
               .OrderByDescending(x => x.CreatedBy_At).ToList();
            return View("BannerShow", list);
        }

        public ActionResult MouleArrivals()
        {
            var items= db.Categorys.ToList();
            return View("_MouleArrivals", items);
        }  public ActionResult MouleLeft(int? id)
        {
            if (id != null)
            {
                ViewBag.CateId = id;
            }
            var items= db.Categorys.ToList();
            return View("_MouleLeft", items);
        }
       

    }
}