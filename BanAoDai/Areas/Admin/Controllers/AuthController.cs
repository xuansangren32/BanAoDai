using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace BanAoDai.Areas.Admin.Controllers
{
    public class AuthController : Controller
    {
        protected BanAoDaiDBContext db = new BanAoDaiDBContext();
        // GET: Admin/Auth
        public ActionResult LoginAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginAdmin(string username,string password)
        {
            string error = null;
            var f_password = GetMD5(password);
            User user = db.User.Where(x => x.Status == 1 
            && x.CountError == 1 && 
            (x.UserName == username || x.Email == username) 
            && x.Password == f_password).FirstOrDefault();
            if (user == null)
            {
                error = "Thông tin đăng nhập không chính xác !";
            }
            else
            {
                Session["UserAdmin"] = username;
                Session["UserAdmin_id"] = user.Id;
                return RedirectToAction("Index", "Dashboard");
            }
            ViewBag.Error= error;
            return View();
        }
        public ActionResult LogoutAdmin()
        {
            Session["UserAdmin"] = "";
            Session["UserAdmin_id"] = "";
            return Redirect("~/Admin/Login");
        }
        [HttpGet]

        public ActionResult RegisterAdmin()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterAdmin(User _user)
        {
            if (ModelState.IsValid)
            {
                var check = db.User.FirstOrDefault(x => x.Email != _user.Email && x.UserName != _user.UserName);
                if (check != null)
                {
                    _user.Password = GetMD5(_user.Password);
                    db.Configuration.ValidateOnSaveEnabled = false;
                    if (_user.CountError == 0)
                    {
                        _user.CountError = 1;
                    }
                    else
                    {
                        _user.CountError += 1;
                    }
                    if (_user.Roles == null)
                    {
                        _user.Roles +=1 ;
                    }
                    db.User.Add(_user);
                    db.SaveChanges();
                    ViewBag.error = "Đăng Ký Tài Khoản Thành Công!";
                    //return RedirectToAction("Auth", "RegisterAdmin");
                    //return RedirectToAction("RegisterAdmin", "Auth");
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    ViewBag.error = "Email đã được sử dụng, Vui lòng thử lại !";
                    ViewBag.error2 = "Tên tài khoản đã tồn tại!";
                    //return RedirectToAction("Auth");
                }
            }
            return View();
        }

        //create a string MD5
        public static string GetMD5(string str)
        {

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
    }
}