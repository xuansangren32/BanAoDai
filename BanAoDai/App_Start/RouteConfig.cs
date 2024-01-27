using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BanAoDai
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
             name: "Home",
             url: "trang-chu",
             defaults: new { controller = "Site", action = "Index", id = UrlParameter.Optional }
         );
            routes.MapRoute(
             name: "Contacts",
             url: "lien-he",
             defaults: new { controller = "Contacts", action = "Index", id = UrlParameter.Optional }
         );
            routes.MapRoute(
            name: "Posts",
            url: "tin-tuc",
            defaults: new { controller = "Post", action = "Index", id = UrlParameter.Optional }
        );
            routes.MapRoute(
             name: "CheckOut",
             url: "thanh-toan",
             defaults: new { controller = "ShoppingCart", action = "CheckOut", id = UrlParameter.Optional }
         );
            routes.MapRoute(
             name: "ShoppingCart",
             url: "gio-hang",
             defaults: new { controller = "ShoppingCart", action = "Index", id = UrlParameter.Optional }
         );
            routes.MapRoute(
           name: "ProductsDetail",
           url: "chi-tiet/{slug3}-p{id}",
           defaults: new { controller = "Products", action = "Detail", id = UrlParameter.Optional }
   );
            routes.MapRoute(
          name: "DetailPosts",
          url: "{slug}-n{id}",
          defaults: new { controller = "Post", action = "Detail", id = UrlParameter.Optional }
  );

            routes.MapRoute(
           name: "ProductCategorys",
           url: "danh-muc-san-pham/{slug2}-{id}",
           defaults: new { controller = "Products", action = "ProductCategorys", id = UrlParameter.Optional }
       );
            routes.MapRoute(
                name: "Products",
                url: "san-pham",
                defaults: new { controller = "Products", action = "Index", slug = UrlParameter.Optional }
            );



            routes.MapRoute(
                name: "SiteSlug",
                url: "{slug}",
                defaults: new { controller = "Site", action = "Index", id = UrlParameter.Optional }
            );


            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Site", action = "Index", id = UrlParameter.Optional }
           );
        }
    }
}
