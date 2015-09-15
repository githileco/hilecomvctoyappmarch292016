﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HiLToysWebApplication.Logic;
using HiLToysApplicationServices;
using HiLToysDataModel.Models;
using HiLToysViewModel;
using HiLToysWebApplication.Models;
using HiLToysWebApplication.HiLToysDataAccessServices;

namespace HiLToysWebApplication.Controllers
{

     [Authorize]
    public class CheckoutController : Controller
    {
         private HiLToysApplicationDbContext db = new HiLToysApplicationDbContext();

        //
        // GET: /Checkout/
        public ActionResult CheckoutStart()
        {
            NVPAPICaller payPalCaller = new NVPAPICaller();
            string ErrorMessage = "Unkown";
            string retMsg = "Messgae from PayPal";
            string token = "";

            if (Session["payment_amt"] != null)
            {
                string amt = Session["payment_amt"].ToString();

                //bool ret = payPalCaller.ShortcutExpressCheckout(amt, ref token, ref retMsg);
                bool ret = true;
                if (ret)
                {
                    Session["token"] = token;
                    //Response.Redirect(retMsg);
                    return RedirectToAction("CheckoutReview");
                    // return RedirectToAction(retMsg);
                }
                else
                {
                    //Response.Redirect("CheckoutError.aspx?" + retMsg);
                    return RedirectToAction("CheckoutError", retMsg);
                }
            }
            else
            {
               // Response.Redirect("CheckoutError.aspx?ErrorCode=AmtMissing");
                ErrorMessage = "AmtMissing";
                RedirectToAction("CheckoutError", ErrorMessage);
            }
            return RedirectToAction("CheckoutError", ErrorMessage);
        }
        public ActionResult CheckoutError(string errorMessage)
        {
            CheckoutViewModel checkoutViewModel = new CheckoutViewModel();
            checkoutViewModel.ErrorMessage = errorMessage;
            return View("CheckoutError", checkoutViewModel);
        }
        public ActionResult CheckoutReview()
        {
            int CustomerID=0;
        string retMsg = "";
        string token = "";
        string PayerID = "5678912340";
        NVPCodec decoder = new NVPCodec();
       // token = Session["token"].ToString();
        OrderViewModel orderViewModel = new OrderViewModel();
        NVPAPICaller payPalCaller = new NVPAPICaller();
        CustomerDataAccessService customerDataAccessService = new CustomerDataAccessService();
        var Cart = ShoppingCartActions.GetCart(this.HttpContext);
        // string CartID=cart.ShoppingCartId;
        orderViewModel.Order.CartID = Cart.ShoppingCartId; 

       // bool ret = payPalCaller.GetCheckoutDetails(token, ref PayerID, ref decoder, ref retMsg);
        bool ret = true;
        if (ret)
        {
          Session["payerId"] = PayerID;
         //int CustomerID = 1;
        // int CustomerID=  GetCustomer(User.Identity.Name);
         /* orderViewModel.Order.OrderDate = Convert.ToDateTime(decoder["TIMESTAMP"].ToString());
          //orderViewModel. Order.UserName = User.Identity.Name;
          orderViewModel.Order.FirstName = decoder["FIRSTNAME"].ToString();
          orderViewModel.Order.LastName = decoder["LASTNAME"].ToString();
          orderViewModel.Order.SheepToStreet = decoder["SHIPTOSTREET"].ToString();
          orderViewModel.Order.ShipCity = decoder["SHIPTOCITY"].ToString();
          orderViewModel.Order.ShipToState = decoder["SHIPTOSTATE"].ToString();
          orderViewModel.Order.ShipPostalCode = decoder["SHIPTOZIP"].ToString();
          orderViewModel.Order.ShipCountry = decoder["SHIPTOCOUNTRYCODE"].ToString();
          orderViewModel.Order.Email = decoder["EMAIL"].ToString();
          orderViewModel.Order.OrderTotal = Convert.ToDouble(decoder["AMT"].ToString());
          orderViewModel.Customer.CustomerID = CustomerID;
            */
          CustomerID = customerDataAccessService.GetCustomerIdNumber(User.Identity.Name);
            string date="30/1/2014";
            orderViewModel.Order.CustomerID = CustomerID;
           orderViewModel.Order.OrderDate = Convert.ToDateTime(date);
          orderViewModel.Order.UserName = User.Identity.Name;
          orderViewModel.Order.FirstName = "ahmed";
          orderViewModel.Order.LastName = "reshid";
          orderViewModel.Order.SheepToStreet = "lukuanda";
          orderViewModel.Order.ShipCity = "addis";
          orderViewModel.Order.ShipToState = "shoa";
          orderViewModel.Order.ShipPostalCode = "gfhjty6";
          orderViewModel.Order.ShipCountry = "djubuti";
          orderViewModel.Order.Email = "amdmukim";
          orderViewModel.Order.ShipVia =2;
          orderViewModel.Order.OrderTotal = Convert.ToDouble("125014");
          orderViewModel.Order.RequiredDate = Convert.ToDateTime(date);
          orderViewModel.Order.ShippedDate = Convert.ToDateTime(date); 
      
          // Verify total payment amount as set on CheckoutStart.aspx.
        /*  try
          {
            decimal paymentAmountOnCheckout = Convert.ToDecimal(Session["payment_amt"].ToString());
            decimal paymentAmoutFromPayPal = Convert.ToDecimal(decoder["AMT"].ToString());
            if (paymentAmountOnCheckout != paymentAmoutFromPayPal)
            {
              //Response.Redirect("CheckoutError.aspx?" + "Desc=Amount%20total%20mismatch.");
              return RedirectToAction("CheckoutError", new { id = "Desc=Amount%20total%20mismatch." });
            }
          }
          catch (Exception)
          {
            //Response.Redirect("CheckoutError.aspx?" + "Desc=Amount%20total%20mismatch.");
            return RedirectToAction("CheckoutError", new { id = "Desc=Amount%20total%20mismatch." });

          }*/
            //Process the order

          OrderApplicationService orderApplicationService = new OrderApplicationService();
            orderViewModel = orderApplicationService.CreateOrder(orderViewModel);
            orderViewModel = orderApplicationService.BeginOrderEntry(orderViewModel);
            //orderViewModel.Shippers = orderDataAccessService.GetShippers();
             

            // Set OrderId.
            Session["currentOrderId"] = orderViewModel.Order.OrderID;

          
        }
        else
        {
          //Response.Redirect("CheckoutError.aspx?" + retMsg);
            RedirectToAction("CheckoutError", retMsg);
        }
        return View("CheckoutReview", orderViewModel);   
        }
        /*public ActionResult CheckoutComplete()
        {
          Session["userCheckoutCompleted"]="true";
            if ((string)Session["userCheckoutCompleted"] != "true")
        {
          Session["userCheckoutCompleted"] = string.Empty;
          Response.Redirect("CheckoutError.aspx?" + "Desc=Unvalidated%20Checkout.");
        }

        NVPAPICaller payPalCaller = new NVPAPICaller();

        string retMsg = "";
        string token = "";
        string finalPaymentAmount = "";
        string PayerID = "";
        NVPCodec decoder = new NVPCodec();

        token = Session["token"].ToString();
        PayerID = Session["payerId"].ToString();
        finalPaymentAmount = Session["payment_amt"].ToString();

        bool ret = payPalCaller.DoCheckoutPayment(finalPaymentAmount, token, PayerID, ref decoder, ref retMsg);
        if (ret)
        {
          // Retrieve PayPal confirmation value.
          string PaymentConfirmation = decoder["PAYMENTINFO_0_TRANSACTIONID"].ToString();
          TransactionId.Text = PaymentConfirmation;


          ProductContext _db = new ProductContext();
          // Get the current order id.
          int currentOrderId = -1;
          if (Session["currentOrderId"] != string.Empty)
          {
            currentOrderId = Convert.ToInt32(Session["currentOrderID"]);
          }
          Order myCurrentOrder;
          if (currentOrderId >= 0)
          {
            // Get the order based on order id.
            myCurrentOrder = _db.Orders.Single(o => o.OrderId == currentOrderId);
            // Update the order to reflect payment has been completed.
            myCurrentOrder.PaymentTransactionId = PaymentConfirmation;
            // Save to DB.
            _db.SaveChanges();
          }

          // Clear shopping cart.
          using (WingtipToys.Logic.ShoppingCartActions usersShoppingCart =
              new WingtipToys.Logic.ShoppingCartActions())
          {
            usersShoppingCart.EmptyCart();
          }

          // Clear order id.
          Session["currentOrderId"] = string.Empty;
        }
        else
        {
          Response.Redirect("CheckoutError.aspx?" + retMsg);
        }
      }
        }*/

       public ActionResult CheckoutComplete()
       {
           CheckoutViewModel checkoutViewModel = new CheckoutViewModel();
           string ErrorMessage = "";
            Session["userCheckoutCompleted"]="true";
           if ((string)Session["userCheckoutCompleted"] != "true")
           {
               Session["userCheckoutCompleted"] = string.Empty;
               ErrorMessage = "Unvalidated%20Checkout";
               RedirectToAction("CheckoutError", ErrorMessage);
           }

        NVPAPICaller payPalCaller = new NVPAPICaller();

      string retMsg = "";
      // string token = "";
       string finalPaymentAmount = "";
       //string PayerID = "";
       NVPCodec decoder = new NVPCodec();
       string PaymentConfirmation = "";

       //token = Session["token"].ToString();
     //  PayerID = Session["payerId"].ToString();
       finalPaymentAmount = Session["payment_amt"].ToString();

      // bool ret = payPalCaller.DoCheckoutPayment(finalPaymentAmount, token, PayerID, ref decoder, ref retMsg);
        bool ret = true;

        if (ret)
       {
         // Retrieve PayPal confirmation value.
         //string PaymentConfirmation = decoder["PAYMENTINFO_0_TRANSACTIONID"].ToString();
          PaymentConfirmation ="34rt56789";
          //checkoutViewModel.PayementConfirmationNo = PaymentConfirmation;

         // Get the current order id.
         int currentOrderId = -1;
         if (Session["currentOrderId"].ToString() !=string.Empty)
         {
           currentOrderId = Convert.ToInt32(Session["currentOrderID"]);
         }
            string CartID="";
         if (currentOrderId >= 0)
         {
             OrderApplicationService orderApplicationService = new OrderApplicationService();
             var Cart = ShoppingCartActions.GetCart(this.HttpContext);
             CartID = Cart.GetCartId(this.HttpContext);
           // Get the order based on order id.
           // Update the order to reflect payment has been completed.Clear shopping cart.
           orderApplicationService.UpdateOrderEmptyCart(currentOrderId, PaymentConfirmation, CartID);

           // Save to DB.
         }
         checkoutViewModel.PayementConfirmationNo = PaymentConfirmation;

         // Clear order id.
         Session["currentOrderId"] = string.Empty;
       }
       else
       {
           RedirectToAction("CheckoutError", retMsg);
       }
        return View("CheckoutComplete", checkoutViewModel);
     }
       
        
        //
        // GET: /Checkout/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        
        //
        // GET: /Checkout/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Checkout/Create
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
        // GET: /Checkout/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Checkout/Edit/5
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
        // GET: /Checkout/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Checkout/Delete/5
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
