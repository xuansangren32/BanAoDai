using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BanAoDai
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        protected void Session_Start()
        {
            //Session["Cart"] = "";
            Session["UserAdmin"] = "";
            Session["UserAdmin_id"] = "";
            Session["UserCustomer"] = "";
        }
    }
}
