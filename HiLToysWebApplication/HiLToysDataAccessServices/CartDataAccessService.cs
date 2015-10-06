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
using HiLToysWebApplication.Models;
using System.Data.Entity.Infrastructure;


namespace HiLToysWebApplication.HiLToysDataAccessServices
{
    public class CartDataAccessService
    {
        private ApplicationDbContext storeDB;
        public CartDataAccessService()
        {
            storeDB = new ApplicationDbContext();
        }
        /*public CartDataAccessService(IApplicationDbContext dbContext)
        {
            storeDB = dbContext;
       }*/
        string ShoppingCartId { get; set; }

        public double GetTotal()
        {
            // Multiply product price by Quantity of that product to get 
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

            //var cartItem = storeDB.Database.SqlQuery<Cart>("SELECT* FROM Carts WHERE CartID=@ShoppingCartId AND ProductID=@ProductID", new SqlParameter("@ShoppingCartId", ShoppingCartId), new SqlParameter("@ProductID", cartViewModel.Cart.ProductID)).FirstOrDefault();


            if (cartItem == null)
            {


                // Create a new cart item if no cart item exists
                cartItem = new HiLToysDataModel.Cart()
                {
                    CartID = ShoppingCartId,
                    RecordId = Guid.NewGuid().ToString(),
                    ProductID = cartViewModel.Cart.ProductID,
                    ProductName = cartViewModel.Cart.ProductName,
                    Quantity = cartViewModel.Cart.Quantity,
                    UnitPrice = cartViewModel.Cart.UnitPrice,
                    SubTotal = cartViewModel.Cart.SubTotal,
                    Count = 1,
                    DateCreated = DateTime.Now
                };

                storeDB.Carts.Add(cartItem);
                // storeDB.Entry(cartItem).State = System.Data.Entity.EntityState.Modified;
            
               //   string sql="INSERT INTO Carts(CartID,RecordId,ProductID,ProductName,Quantity,UnitPrice,SubTotal,Count,DateCreated)VALUES(@CartID,@RecordId,@ProductID,@ProductName,@Quantity,@UnitPrice,@SubTotal,@Count,@DateCreated)";
               //  List<SqlParameter> parameterlist=new List<SqlParameter>();
               //  parameterlist.Add(new SqlParameter("@CartID",ShoppingCartId));
               //      parameterlist.Add(new SqlParameter("@RecordId",Guid.NewGuid().ToString()));
               //   parameterlist.Add(new SqlParameter("@ProductName",cartViewModel.Cart.ProductName));
               //   parameterlist.Add(new SqlParameter("@Quantity",cartViewModel.Cart.Quantity));
               //   parameterlist.Add(new SqlParameter("@UnitPrice",cartViewModel.Cart.UnitPrice));
               //   parameterlist.Add(new SqlParameter("@SubTotal",cartViewModel.Cart.SubTotal));
               //   parameterlist.Add(new SqlParameter("@Count",1));
               //    parameterlist.Add(new SqlParameter("@DateCreated", DateTime.Now));
               // SqlParameter[] parameters=parameterlist.ToArray();
                   
               //storeDB.Database.ExecuteSqlCommand(sql,parameters);
            
            
            }
            else
            {
                // If the item does exist in the cart, then add one to the quantity

                int totqant = 0;
                int count = 0;
                count = cartItem.Count + 1;
                totqant = cartItem.Quantity + cartViewModel.Cart.Quantity;
                storeDB.Database.ExecuteSqlCommand("update Carts set Quantity = @qnt,Count=@count where ProductID =@Pid", new SqlParameter("@Pid", cartItem.ProductID), new SqlParameter("@qnt", totqant), new SqlParameter("@count", count));

            }
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
         // DateTime orddate = Convert.ToDateTime(order.OrderDate);
          
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
            
            storeDB.Database.ExecuteSqlCommand("update Carts set CartID = @userName WHERE CartID=@ShoppingCartId ", new SqlParameter("@userName", userName), new SqlParameter("@ShoppingCartId", ShoppingCartId));
            storeDB.SaveChanges();
        }
        public List<HiLToysDataModel.Models.OrderDetail> GetOrderDetail(int OrderID)
        {
            return storeDB.OrderDetails.Where(orddetail => orddetail.OrderID == OrderID).ToList();
        }
        public List<HiLToysDataModel.Cart> GetCartItems()
        { 

            //return storeDB.Carts.Where(cart => cart.CartID == ShoppingCartId).ToList();
            return storeDB.Database.SqlQuery<Cart>("SELECT* FROM Carts WHERE CartID = @ShoppingCartId", new SqlParameter("@ShoppingCartId", ShoppingCartId)).ToList();
        }
       
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
            rtncartViewModel.Cart.CartTotal = GetTotal();
            rtncartViewModel.Cart.Count = GetCount();

            rtncartViewModel.Carts = storeDB.Database.SqlQuery<HiLToysDataModel.Cart>("SELECT* FROM Carts WHERE CartID = @ShoppingCartId", new SqlParameter("@ShoppingCartId", ShoppingCartId)).ToList();
            return rtncartViewModel;

        }
        

        public CartViewModel DeleteCartDetailLineItemX(CartViewModel cartViewModel)
        {
            CartViewModel rtncartViewModel = new CartViewModel();
           
            ShoppingCartId = cartViewModel.Cart.CartID;
            rtncartViewModel.ReturnStatus = false;
            var cartItem = storeDB.Carts.Single(
           cart => cart.CartID == ShoppingCartId && cart.RecordId == cartViewModel.Cart.RecordId);

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
        public CartViewModel DeleteCartDetailLineItem(CartViewModel cartViewModel)
        {
            CartViewModel rtncartViewModel = new CartViewModel();
            string RecordId = "";
            RecordId = cartViewModel.Cart.RecordId;
            ShoppingCartId = cartViewModel.Cart.CartID;
            rtncartViewModel.ReturnStatus = false;
            var cartItem = storeDB.Carts.Single(
           cart => cart.CartID == ShoppingCartId && cart.RecordId == cartViewModel.Cart.RecordId);

           // int itemCount = 0;

            if (cartItem != null)
            {
                storeDB.Database.ExecuteSqlCommand("DELETE FROM Carts WHERE CartID = @ShoppingCartId AND RecordId=@RecordId", new SqlParameter("@RecordId", RecordId), new SqlParameter("@ShoppingCartId", ShoppingCartId));

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
         CartViewModel rtncartViewModel = new CartViewModel();

         ShoppingCartId = cartViewModel.Cart.CartID;
         rtncartViewModel.ReturnStatus = false;

         var cartItem = storeDB.Carts.SingleOrDefault(
         c => c.CartID == ShoppingCartId
         && c.ProductID == cartViewModel.Cart.ProductID);

         if (cartItem != null)
         {
             int quantity = 0;
             quantity = cartViewModel.Cart.Quantity;
             storeDB.Database.ExecuteSqlCommand("update Carts set Quantity = @quantity WHERE CartID=@ShoppingCartId AND ProductID=@ProductID", new SqlParameter("@quantity", quantity), new SqlParameter("@ShoppingCartId", ShoppingCartId), new SqlParameter("@ProductID", cartViewModel.Cart.ProductID));

             //cartItem.Quantity = cartViewModel.Cart.Quantity;
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
