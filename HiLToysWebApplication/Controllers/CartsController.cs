﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;
using System.Web.Mvc;
using HiLToysApplicationServices;
using HiLToysViewModel;
using HiLToysWebApplication.Models;

namespace HiLToysWebApplication.Controllers
{
    public class CartsController : Controller
    {

        //
        // GET: /Carts/
     public ActionResult Index()
     {
         CartApplicationService cartApplicationService = new CartApplicationService();

           var Cart = ShoppingCartActions.GetCart();
           string CartID = Cart.ShoppingCartId;

            CartViewModel cartViewModel = new CartViewModel();
           cartViewModel = cartApplicationService.GetCarts(CartID);
            if (cartViewModel.Cart.CartTotal < 1)
             Session["payment_amt"] = null;
             else
             Session["payment_amt"] = cartViewModel.Cart.CartTotal;

            return View("Index", cartViewModel);
    }
//[ChildActionOnly]
     public ActionResult LoginPartialCustom()
          
     {
         return PartialView("LoginPartialCustom");
}
     public ActionResult LoginPartialCustom2()
     {
         ProductApplicationService productApplicationService = new ProductApplicationService();

         ProductViewModel productViewModel = new ProductViewModel();
         productViewModel.Products = productApplicationService.GetProducts().Products;
        // return View("Index", St);
         //return RedirectToAction("Index", "Store");
         return PartialView("LoginPartialCustom2", productViewModel);
     }
     [ChildActionOnly]
     public ActionResult CartSummary()
          
     {
         List<HiLToysDataModel.Cart> products = new List<HiLToysDataModel.Cart>();
        CartApplicationService cartApplicationService = new CartApplicationService();
         CartViewModel cartViewModel = new CartViewModel();
         var Cart = ShoppingCartActions.GetCart();


         cartViewModel.Cart.CartID = Cart.ShoppingCartId;
         cartViewModel = cartApplicationService.GetCarts(Cart.ShoppingCartId);
        
        if (cartViewModel.Cart.CartTotal < 1)
             Session["payment_amt"] = null;
         else
             Session["payment_amt"] = cartViewModel.Cart.CartTotal;
        cartViewModel = cartApplicationService.GetCartCount(cartViewModel);
      ViewData["CartCount"] = cartViewModel.Cart.Count;

         return PartialView("CartSummary");
     }
     public ActionResult ResetCartSummary()
     {
         CartApplicationService cartApplicationService = new CartApplicationService();
         CartViewModel cartViewModel = new CartViewModel();
         var Cart = ShoppingCartActions.GetCart();
         cartViewModel.Cart.CartID = Cart.ShoppingCartId;
         cartViewModel = cartApplicationService.GetCartCount(cartViewModel);
         ViewData["CartCount"] = cartViewModel.Cart.Count;

         return PartialView("CartSummary");
     }
        //
     public ActionResult GetCartCount(FormCollection postedFormData)
     {
         CartApplicationService cartApplicationService = new CartApplicationService();
         CartViewModel cartViewModel = new CartViewModel();
         var Cart = ShoppingCartActions.GetCart();
         cartViewModel.Cart.CartID = Cart.ShoppingCartId;
         cartViewModel = cartApplicationService.GetCartCount(cartViewModel);
         return Json(new
         {
             TotalCatCount = cartViewModel.Cart.Count

         });
     }
     public ActionResult AddToCart(int ProductID, string ProductName,double UnitPrice, string Quantity)
     {
         Int32 CartCount = 0;
         CartCount = Convert.ToInt32(ViewData["CartCount"]);
         CartCount = CartCount + 1;
         ViewData["CartCount"] = CartCount;

         // Add it to the shopping cart
         CartApplicationService cartApplicationService = new CartApplicationService();
         CartViewModel cartViewModel = new CartViewModel();
         var Cart = ShoppingCartActions.GetCart();
         double subtotal = 99;
         cartViewModel.Cart.CartID = Cart.ShoppingCartId;

         cartViewModel.Cart.ProductID = ProductID;
         cartViewModel.Cart.ProductName = ProductName;

         // if (HiLToysBusinessServices.Utilities.IsNumeric((postedFormData["Quantity"])) == true)
         cartViewModel.Cart.Quantity = Convert.ToInt32(Quantity);

         // if (HiLToysBusinessServices.Utilities.ToDecimal((postedFormData["UnitPrice"])) == true)
         cartViewModel.Cart.UnitPrice = UnitPrice;
         // if (HiLToysBusinessServices.Utilities.ToDouble((postedFormData["SubTotal"])) == true)
         //cartViewModel.Cart.SubTotal = Convert.ToDouble(postedFormData["SubTotal"]);
         cartViewModel.Cart.SubTotal = subtotal;
         cartViewModel = cartApplicationService.AddCartDetailLineItem(cartViewModel);
         return PartialView("CartSummary");
        
         /*
         // Retrieve the album from the database
         var addedAlbum = storeDB.Albums
             .Single(album => album.AlbumId == id);

         // Add it to the shopping cart
         var cart = ShoppingCart.GetCart(this.HttpContext);

         cart.AddToCart(addedAlbum);

         // Go back to the main store page for more shopping
         return RedirectToAction("Index");*/
     }
      
        // GET: /Carts/Details/5
        public ActionResult AddNewProductCartDetailLineItem(FormCollection postedFormData)
        {
            // Retrieve the album from the database
          //  var addedAlbum = storeDB.Albums.Single(album => album.AlbumId == id);

            // Add it to the shopping cart
            CartApplicationService cartApplicationService = new CartApplicationService();
            CartViewModel cartViewModel = new CartViewModel();
            var Cart = ShoppingCartActions.GetCart();
            double subtotal = 99;
            cartViewModel.Cart.CartID = Cart.ShoppingCartId;

            cartViewModel.Cart.ProductID = Convert.ToInt32(postedFormData["ProductID"]);
            cartViewModel.Cart.ProductName = Convert.ToString(postedFormData["ProductName"]);

           // if (HiLToysBusinessServices.Utilities.IsNumeric((postedFormData["Quantity"])) == true)
                cartViewModel.Cart.Quantity = Convert.ToInt32(postedFormData["Quantity"]);

           // if (HiLToysBusinessServices.Utilities.ToDecimal((postedFormData["UnitPrice"])) == true)
                cartViewModel.Cart.UnitPrice = Convert.ToDouble(postedFormData["UnitPrice"]);
           // if (HiLToysBusinessServices.Utilities.ToDouble((postedFormData["SubTotal"])) == true)
                //cartViewModel.Cart.SubTotal = Convert.ToDouble(postedFormData["SubTotal"]);
                cartViewModel.Cart.SubTotal = subtotal;
            cartViewModel = cartApplicationService.AddCartDetailLineItem(cartViewModel);
            Session["payment_amt"] = cartViewModel.Cart.CartTotal;
            return Json(new
            {
                ReturnStatus = cartViewModel.ReturnStatus,
                viewModel = cartViewModel,
                //ValidationErrors = cartViewModel.ValidationErrors,
                //MessageBoxView = Helpers.MvcHelpers.RenderPartialView(this, "_MessageBox", cartViewModel),
            });
        }
         public ActionResult UpdateCartDetailLineItem(FormCollection postedFormData)
          {
              CartApplicationService cartApplicationService = new CartApplicationService();
              CartViewModel cartViewModel = new CartViewModel();

             var Cart = ShoppingCartActions.GetCart();
            cartViewModel.Cart.CartID = Cart.ShoppingCartId;
            cartViewModel.Cart.ProductID = Convert.ToInt32(postedFormData["ProductID"]);
        //if (HiLToysBusinessServices.Utilities.IsNumeric((postedFormData["Quantity"])) == true)
                cartViewModel.Cart.Quantity = Convert.ToInt32(postedFormData["Quantity"]);
               
              cartViewModel = cartApplicationService.UpdateCartDetailLineItem(cartViewModel);
               Session["payment_amt"]=cartViewModel.Cart.CartTotal;
              return Json(new
              {
                  ReturnStatus = cartViewModel.ReturnStatus,
                  viewModel = cartViewModel,
                  xtotal=cartViewModel.Cart.CartTotal,
                                   //ValidationErrors = cartViewModel.ValidationErrors,
  //                MessageBoxView = Helpers.MvcHelpers.RenderPartialView(this, "_MessageBox", cartViewModel),
              },JsonRequestBehavior.AllowGet);

          }
         public ActionResult DeleteCartDetailLineItem(FormCollection postedFormData)
        {
            CartApplicationService cartApplicationService = new CartApplicationService();
            CartViewModel cartViewModel = new CartViewModel();
           var Cart = ShoppingCartActions.GetCart();
           cartViewModel.Cart.CartID = Cart.ShoppingCartId; 
            cartViewModel.Cart.ProductID = Convert.ToInt32(postedFormData["ProductID"]);
            cartViewModel.Cart.ProductName=Convert.ToString(postedFormData["productName"]);
            string rowIndex = Convert.ToString(postedFormData["RowIndex"]);
            cartViewModel.Cart.RecordId = Convert.ToString(postedFormData["RecordId"]);

            cartViewModel = cartApplicationService.DeleteCartDetailLineItem(cartViewModel);
            Session["payment_amt"] = cartViewModel.Cart.CartTotal;

           return Json(new
            {
                ReturnStatus = cartViewModel.ReturnStatus,
                viewModel = cartViewModel,
                TotalCartCounts = cartViewModel.Cart.Count,
                xtotal = cartViewModel.Cart.CartTotal,
                RowIndex = rowIndex,
               // ValidationErrors = cartViewModel.ValidationErrors,
               // MessageBoxView = Helpers.MvcHelpers.RenderPartialView(this, "_MessageBox", cartViewModel),
            });
            
            //return RedirectToAction("Index");
        }
        //
        // GET: /Carts/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Carts/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Carts/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Carts/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Carts/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Carts/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
