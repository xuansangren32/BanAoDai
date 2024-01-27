﻿using MyClass.Model;
using BanAoDai.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace BanAoDai.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        private BanAoDaiDBContext dbShoppingCart = new BanAoDaiDBContext();
        public ActionResult Index()
        {
            
            return View();
        }
        public ActionResult Partial_CheckOut()
        {
            return PartialView();
        }
        public ActionResult CheckOut()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
            }

            return View();
        }
        public ActionResult CheckOutSuccess()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(OrderViewModel req)
        {
            var db = new BanAoDaiDBContext();
            var code = new { Success = false, code = -1 };
            if (ModelState.IsValid)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart != null)
                {

                    Order order = new Order();
                    order.ReceiveName = req.ReceiveName;
                    order.ReceivePhone = req.ReceivePhone;
                    order.ReceiveAddress = req.ReceiveAddress;
                    order.ReceiveEmail = req.ReceiveEmail;
                    cart.Items.ForEach(x => order.OrderDetails.Add(new OrderDetail
                    {
                        ProductId=x.ProductId,
                        Quantity = x.Quantity,
                        Price=x.Price
                    }));
                    order.TotalAmount = cart.Items.Sum(x => (x.Price * x.Quantity));
                    order.TypePayment = req.TypePayment;
                    order.CreatedBy_At = DateTime.Now;
                    order.UpdatedAt = DateTime.Now;
                    order.CreatedBy = req.ReceivePhone;
                    Random rd = new Random();
                    order.Code = "DH" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
                    db.Orders.Add(order);
                    db.SaveChanges();
                    //send mail cho khachs hang
                    var strSanPham = "";
                    var thanhtien = decimal.Zero;
                    var TongTien = decimal.Zero;
                    foreach (var sp in cart.Items)
                    {
                        strSanPham += "<tr>";
                        strSanPham += "<td>" + sp.ProductName + "</td>";
                        strSanPham += "<td>" + sp.Quantity + "</td>";
                        strSanPham += "<td>" + BanAoDai.Common.Common.FormatNumber(sp.TotalPrice, 0) + "</td>";
                        strSanPham += "</tr>";
                        thanhtien += sp.Price * sp.Quantity;
                    }
                    TongTien = thanhtien;
                    string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send2.html"));
                    contentCustomer = contentCustomer.Replace("{{MaDon}}", order.Code);
                    contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
                    contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", order.ReceiveName);
                    contentCustomer = contentCustomer.Replace("{{Phone}}", order.ReceivePhone);
                    contentCustomer = contentCustomer.Replace("{{Email}}", req.ReceiveEmail);
                    contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", order.ReceiveAddress);
                    contentCustomer = contentCustomer.Replace("{{ThanhTien}}", BanAoDai.Common.Common.FormatNumber(thanhtien, 0));
                    contentCustomer = contentCustomer.Replace("{{TongTien}}", BanAoDai.Common.Common.FormatNumber(TongTien, 0));
                    BanAoDai.Common.Common.SendMail("ShopOnline", "Đơn hàng #" + order.Code, contentCustomer.ToString(), req.ReceiveEmail);

                    string contentAdmin = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send1.html"));
                    contentAdmin = contentAdmin.Replace("{{MaDon}}", order.Code);
                    contentAdmin = contentAdmin.Replace("{{SanPham}}", strSanPham);
                    contentAdmin = contentAdmin.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    contentAdmin = contentAdmin.Replace("{{TenKhachHang}}", order.ReceiveName);
                    contentAdmin = contentAdmin.Replace("{{Phone}}", order.ReceivePhone);
                    contentAdmin = contentAdmin.Replace("{{Email}}", req.ReceiveEmail);
                    contentAdmin = contentAdmin.Replace("{{DiaChiNhanHang}}", order.ReceiveAddress);
                    contentAdmin = contentAdmin.Replace("{{ThanhTien}}", BanAoDai.Common.Common.FormatNumber(thanhtien, 0));
                    contentAdmin = contentAdmin.Replace("{{TongTien}}", BanAoDai.Common.Common.FormatNumber(TongTien, 0));
                    BanAoDai.Common.Common.SendMail("ShopOnline", "Đơn hàng mới #" + order.Code, contentAdmin.ToString(), ConfigurationManager.AppSettings["EmailAdmin"]);

                    cart.ClearCart();

                    return RedirectToAction("CheckOutSuccess");
                }
            }
            return Json(code);
        }
        public ActionResult Partial_Item_Cart()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                return View(cart.Items);
            }
            return PartialView();
        }
        public ActionResult Partial_Item_ThanhToan()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                return View(cart.Items);
            }
            return PartialView();
        }
        public ActionResult ShowCount()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                return Json(new { Count = cart.Items.Count }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Count = 0 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddToCart(int id, int quantity)
        {
            var code = new { Success = false, msg = "", code = -1,Count=0 };
            var db = new BanAoDaiDBContext();
            var checkProduct = db.Products.FirstOrDefault(x => x.Id == id);
            if (checkProduct != null)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if(cart==null)
                {
                    cart = new ShoppingCart();
                }
                ShoppingCartItem item = new ShoppingCartItem
                {
                    ProductId = checkProduct.Id,
                    ProductName = checkProduct.Name,
                    Slug = checkProduct.Slug,
                    Quantity = quantity

                };
                if (checkProduct.ProductImages.FirstOrDefault(x => x.IsDefault) != null)
                {
                    item.Img = checkProduct.ProductImages.FirstOrDefault(x => x.IsDefault).Img;
                }
                item.Price = checkProduct.PriceBuy;
                if (checkProduct.PriceSale > 0)
                {
                    item.Price = (decimal)checkProduct.PriceSale;
                }
                item.TotalPrice = item.Quantity * item.Price;
                cart.AddToCart(item, quantity);
                Session["Cart"] = cart;
                code =new { Success = true, msg = "Thêm Sản Phẩm Vào Giỏ Hàng Thành Công!", code = -1,Count=cart.Items.Count };
            }
            return Json(code);
        }

        [HttpPost]
        public ActionResult Update(int id, int quantity)
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                cart.UpdateQuantity(id, quantity);
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                var checkProduct = cart.Items.FirstOrDefault(x => x.ProductId == id);
                if (checkProduct != null)
                {
                    cart.Remove(id);
                    code = new { Success = true, msg = "", code = -1, Count = cart.Items.Count };
                }
            }
            return Json(code);
        }
        [HttpPost]
        public ActionResult DeteleAll()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                cart.ClearCart();
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }

       
    }
}