using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanAoDai.Controllers
{
    public class ContactsController : Controller
    {
        // GET: Contact
        public ActionResult Index(string id)
        {
            return View();
        }
    }
}