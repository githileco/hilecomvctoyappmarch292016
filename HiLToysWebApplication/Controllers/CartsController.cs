using System;
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
         //Session["ShoppingCartItemsessionKey"] = "d7762014-92eb-468a-8ab5-e87c25a63915";
         CartApplicationService cartApplicationService = new CartApplicationService();

           var Cart = ShoppingCartActions.GetCart();
           string CartID = Cart.ShoppingCartId;
         //string CartID = "7919";

            CartViewModel cartViewModel = new CartViewModel();
           cartViewModel = cartApplicationService.GetCarts(CartID);
            if (cartViewModel.Cart.CartTotal < 1)
             Session["payment_amt"] = null;
             else
             Session["payment_amt"] = cartViewModel.Cart.CartTotal;

            return View("Index", cartViewModel);
    }
     [ChildActionOnly]
     public ActionResult CartSummary()
          
     {
         List<HiLToysDataModel.Cart> products = new List<HiLToysDataModel.Cart>();
        CartApplicationService cartApplicationService = new CartApplicationService();
         CartViewModel cartViewModel = new CartViewModel();
         var Cart = ShoppingCartActions.GetCart();


         cartViewModel.Cart.CartID = Cart.ShoppingCartId;
         //cartViewModel.Cart.CartID = "5623";
         cartViewModel = cartApplicationService.GetCarts(Cart.ShoppingCartId);
        
        if (cartViewModel.Cart.CartTotal < 1)
             Session["payment_amt"] = null;
         else
             Session["payment_amt"] = cartViewModel.Cart.CartTotal;
        cartViewModel = cartApplicationService.GetCartCount(cartViewModel);
       //ViewData["CartCount"] = cartViewModel.Cart.CartTotal;
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
            //cart.AddToCart(addedAlbum);

            // Go back to the main store page for more shopping
            //return RedirectToAction("Index");
           // CartApplicationService cartApplicationService = new CartApplicationService();
           // CartViewModel cartViewModel = new CartViewModel();

            cartViewModel.Cart.CartID = Cart.ShoppingCartId;
          // cartViewModel.Cart.CartID = "5623";

            cartViewModel.Cart.ProductID = Convert.ToInt32(postedFormData["ProductID"]);
            cartViewModel.Cart.ProductName = Convert.ToString(postedFormData["ProductName"]);

           // if (HiLToysBusinessServices.Utilities.IsNumeric((postedFormData["Quantity"])) == true)
                cartViewModel.Cart.Quantity = Convert.ToInt32(postedFormData["Quantity"]);

           // if (HiLToysBusinessServices.Utilities.ToDecimal((postedFormData["UnitPrice"])) == true)
                cartViewModel.Cart.UnitPrice = Convert.ToDouble(postedFormData["UnitPrice"]);
           // if (HiLToysBusinessServices.Utilities.ToDouble((postedFormData["SubTotal"])) == true)
                //cartViewModel.Cart.SubTotal = Convert.ToDouble(postedFormData["SubTotal"]);
                cartViewModel.Cart.SubTotal = subtotal;
                //cartViewModel.Ca
            cartViewModel = cartApplicationService.AddCartDetailLineItem(cartViewModel);
           // ProductViewModel productViewModel = new ProductViewModel();
           // productViewModel.Cart.Count = cartViewModel.Cart.Count;
           // ViewData["CartCount"] = cartViewModel.Cart.Count;
           // productViewModel.ReturnStatus = cartViewModel.ReturnStatus;
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


          //    CartApplicationService cartApplicationService = new CartApplicationService();
      //      CartViewModel cartViewModel = new CartViewModel();
             var Cart = ShoppingCartActions.GetCart();
           // string CartID = "7919";
            cartViewModel.Cart.CartID = Cart.ShoppingCartId;
            //cartViewModel.Cart.CartID = CartID;
             //string rowIndex = Convert.ToString(postedFormData["RowIndex"]);
            cartViewModel.Cart.ProductID = Convert.ToInt32(postedFormData["ProductID"]);
        //if (HiLToysBusinessServices.Utilities.IsNumeric((postedFormData["Quantity"])) == true)
                cartViewModel.Cart.Quantity = Convert.ToInt32(postedFormData["Quantity"]);
               
                //cartViewModel.Cart.
              cartViewModel = cartApplicationService.UpdateCartDetailLineItem(cartViewModel);
              //cartViewModel.TotalCarts = cartViewModel.Cart.CartTotal;
               Session["payment_amt"]=cartViewModel.Cart.CartTotal;
              return Json(new
              {
                  ReturnStatus = cartViewModel.ReturnStatus,
                  viewModel = cartViewModel,
                  xtotal=cartViewModel.Cart.CartTotal,
                                   //ValidationErrors = cartViewModel.ValidationErrors,
  //                MessageBoxView = Helpers.MvcHelpers.RenderPartialView(this, "_MessageBox", cartViewModel),
              });

          }
         public ActionResult DeleteCartDetailLineItem(FormCollection postedFormData)
        {
            CartApplicationService cartApplicationService = new CartApplicationService();
            CartViewModel cartViewModel = new CartViewModel();
            //string CartID = "7919";
           var Cart = ShoppingCartActions.GetCart();
           cartViewModel.Cart.CartID = Cart.ShoppingCartId; 
            cartViewModel.Cart.ProductID = Convert.ToInt32(postedFormData["ProductID"]);
            cartViewModel.Cart.ProductName=Convert.ToString(postedFormData["productName"]);
            string rowIndex = Convert.ToString(postedFormData["RowIndex"]);
            cartViewModel.Cart.RecordId = Convert.ToString(postedFormData["RecordId"]);

            cartViewModel = cartApplicationService.DeleteCartDetailLineItem(cartViewModel);
           // if (cartViewModel.ReturnStatus==true)
             //{
                // RedirectToAction("ResetCartSummary");
             //}
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
