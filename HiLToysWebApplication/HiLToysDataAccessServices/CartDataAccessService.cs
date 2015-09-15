using System;
using System.Data.Sql;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using HiLToysViewModel;
using HiLToysDataModel;
using HiLToysDataModel.Models;
//using HiLToysWebApplication.HiLToysDataAccessServices;
using HiLToysWebApplication.Models;

//using EMD;

namespace HiLToysWebApplication.HiLToysDataAccessServices
{
    public class CartDataAccessService
    {
        private HiLToysApplicationDbContext storeDB;
        public CartDataAccessService()
        {
            storeDB = new HiLToysApplicationDbContext();
        }
        /*public CartDataAccessService(IApplicationDbContext dbContext)
        {
            storeDB = dbContext;
       }*/
        string ShoppingCartId { get; set; }

        public double GetTotal()
        {
            // Multiply album price by count of that product to get 
            // the current price for each of those products in the cart
            // sum all product price totals to get the cart total
            //double total = 0;

            var total = (from cartItems in storeDB.Carts
                         where cartItems.CartID == ShoppingCartId
                         select (int?)cartItems.Quantity * cartItems.UnitPrice).Sum();
            return Convert.ToDouble(total);
        }
        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up

            int? count = (from cartItems in storeDB.Carts
                          where cartItems.CartID == ShoppingCartId
                          select (int?)cartItems.Count).Sum();

            // Return 0 if all entries are null
            return count ?? 0;
        }
        public Boolean AddToCart(CartViewModel cartViewModel)
        {

            Boolean added = true;
            int counter = 0;
            ShoppingCartId = cartViewModel.Cart.CartID;
            var cartItem = storeDB.Carts.SingleOrDefault(
            c => c.CartID == ShoppingCartId
            && c.ProductID == cartViewModel.Cart.ProductID);

            if (cartItem == null)
            {


                // Create a new cart item if no cart item exists
                cartItem = new HiLToysDataModel.Cart()
                {
                    CartID = ShoppingCartId,
                    ProductID = cartViewModel.Cart.ProductID,
                    ProductName = cartViewModel.Cart.ProductName,
                    Quantity = cartViewModel.Cart.Quantity,
                    UnitPrice = cartViewModel.Cart.UnitPrice,
                    SubTotal = cartViewModel.Cart.SubTotal,
                    Count = 1,
                    DateCreated = DateTime.Now
                };

                storeDB.Carts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, then add one to the quantity
                cartItem.Quantity = cartItem.Quantity + cartViewModel.Cart.Quantity;
                cartItem.Count++;
            }



            // Save changes
            storeDB.SaveChanges();
            counter = GetCount();
            if (counter == cartViewModel.Cart.Count)
                added = false;
            return added;
        }
       
         
         
        public OrderViewModel CreateOrder(OrderViewModel orderViewModel)
     {
          
           ShoppingCartId=  orderViewModel.Order.CartID;
           OrderDataAccessService orderDataAccessService = new OrderDataAccessService();
          var order=new HiLToysDataModel.Models.Order();
          order = orderViewModel.Order;
          storeDB.Orders.Add(order);
          storeDB.SaveChanges();
	      List<HiLToysDataModel.Cart> myOrderList = GetCartItems();

            // Add OrderDetail information to the DB for each product purchased.
            for (int i = 0; i < myOrderList.Count; i++)
            {
              // Create a new OrderDetail object.
                var myOrderDetail = new HiLToysDataModel.Models.OrderDetail();
              myOrderDetail.OrderID = order.OrderID;
              myOrderDetail.Username = orderViewModel.Order.UserName;
              myOrderDetail.ProductID = myOrderList[i].ProductID;
              myOrderDetail.Quantity = myOrderList[i].Quantity;
              myOrderDetail.UnitPrice = myOrderList[i].UnitPrice;
              myOrderDetail.OrderDate = DateTime.Now;

              // Add OrderDetail to DB.
              storeDB.OrderDetails.Add(myOrderDetail);
              storeDB.SaveChanges();
            }
             orderViewModel.OrderDetailProductResults = orderDataAccessService.GetOrderDetailsx(order.OrderID);

             orderViewModel.TotalOrders = orderDataAccessService.GetOrderTotal(order.OrderID);
            return orderViewModel;
	
    }
        public void UpdateOrderEmptyCart(int currentOrderId, string PaymentConfirmation, string CartID)
        {
            HiLToysDataModel.Models.Order myCurrentOrder;

            if (currentOrderId >= 0)
            {
                // Get the order based on order id.
                myCurrentOrder = storeDB.Orders.Single(o => o.OrderID == currentOrderId);
                // Update the order to reflect payment has been completed.
                myCurrentOrder.PaymentTransactionId = PaymentConfirmation;
                // Save to DB.
                storeDB.SaveChanges();
            }
            ShoppingCartId = CartID;
            EmptyCart();
            // Clear shopping cart.
           

        }
        public void EmptyCart()
        {

            var cartItems = storeDB.Carts.Where(cart => cart.CartID == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                storeDB.Carts.Remove(cartItem);
            }

            // Save changes
            storeDB.SaveChanges();
        }
        public void MigrateCart(string userName, string ShoppingCartId)
        {
            var shoppingCart = storeDB.Carts.Where(c => c.CartID == ShoppingCartId);

            if (shoppingCart != null)
            {
                foreach (HiLToysDataModel.Cart item in shoppingCart)
                {
                    item.CartID = userName;
                }
                storeDB.SaveChanges();
            }

        }
        public List<HiLToysDataModel.Models.OrderDetail> GetOrderDetail(int OrderID)
        {
            return storeDB.OrderDetails.Where(orddetail => orddetail.OrderID == OrderID).ToList();
        }
        public List<HiLToysDataModel.Cart> GetCartItems()
        { 

            return storeDB.Carts.Where(cart => cart.CartID == ShoppingCartId).ToList();

        }
       /* public List<HiLToysDataModel.Cart> GetCarts(string CartID)
        {
            ShoppingCartId = CartID;
            storeDB.Configuration.ProxyCreationEnabled = false;
            //List<HiLToysDataModel.Cart> carts = new List<HiLToysDataModel.Cart>();
            return storeDB.Carts.Where(cart => cart.CartID == ShoppingCartId).ToList();
            //carts = storeDB.Carts.ToList();
           // return carts;

        }*/
        public CartViewModel GetCartCount(CartViewModel cartViewModel)
        {
            ShoppingCartId = cartViewModel.Cart.CartID;
            cartViewModel.Cart.Count = GetCount();
           return cartViewModel;
        }
        public CartViewModel GetCarts(CartViewModel cartViewModel)

        {

            CartViewModel rtncartViewModel = new CartViewModel();
            ShoppingCartId = cartViewModel.Cart.CartID;
            rtncartViewModel.Carts=storeDB.Carts.Where(cart => cart.CartID == ShoppingCartId).ToList();
            rtncartViewModel.Cart.CartTotal = GetTotal();
            rtncartViewModel.Cart.Count = GetCount();

            return rtncartViewModel;
            
        }
        public CartViewModel DeleteCartDetailLineItem(CartViewModel cartViewModel)
        {
            CartViewModel rtncartViewModel = new CartViewModel();

            ShoppingCartId = cartViewModel.Cart.CartID;
            rtncartViewModel.ReturnStatus = false;
            var cartItem = storeDB.Carts.Single(
           cart => cart.CartID == ShoppingCartId
           && cart.RecordId == cartViewModel.Cart.RecordId);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    storeDB.Carts.Remove(cartItem);
                }

                // Save changes
                storeDB.SaveChanges();
                rtncartViewModel.ReturnStatus = true;
            }

            rtncartViewModel.Cart.CartTotal = GetTotal();

            rtncartViewModel.Cart.Count = GetCount();
            return rtncartViewModel;

        }
     public CartViewModel UpdateCartDetailLineItem(CartViewModel cartViewModel)
    {
         CartViewModel rtncartViewModel=new CartViewModel();

         ShoppingCartId = cartViewModel.Cart.CartID;
         rtncartViewModel.ReturnStatus = false;

        var cartItem = storeDB.Carts.SingleOrDefault(
        c => c.CartID == ShoppingCartId
        && c.ProductID == cartViewModel.Cart.ProductID);

        if (cartItem != null)
        {

            cartItem.Quantity = cartViewModel.Cart.Quantity;
            storeDB.SaveChanges();
           
            rtncartViewModel.ReturnStatus = true;
        }
        else
        {
            rtncartViewModel.ReturnStatus = false;
        }

        rtncartViewModel.Cart.CartTotal = GetTotal();

            
         return rtncartViewModel;
     }

        
        
    }
}
