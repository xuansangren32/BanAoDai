using System.Web.Mvc;

namespace BanAoDai.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_Login",
                "Admin/Login",
                new { Controller="Auth", action = "LoginAdmin", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Admin_Logout",
                "Admin/Logout",
                new { Controller = "Auth", action = "LogoutAdmin", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { Controller = "Dashboard", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}