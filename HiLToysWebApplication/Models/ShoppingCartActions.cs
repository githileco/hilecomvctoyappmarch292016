
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HiLToysApplicationServices;
using HiLToysWebApplication.HiLToysDataAccessServices;
using HiLToysWebApplication.Models;
//using HiLToysApplicationServices;

namespace HiLToysWebApplication
{
    public partial class ShoppingCartActions
    {

       public string ShoppingCartId { get; set; }

       public const string ShoppingCartItemsessionKey = "cartid";
        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId()
        {
            if (HttpContext.Current.Session[ShoppingCartItemsessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name))
                {
                    HttpContext.Current.Session[ShoppingCartItemsessionKey] = HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();

                    // Send tempCartId back to client as a cookie
                    HttpContext.Current.Session[ShoppingCartItemsessionKey] = tempCartId.ToString();
                }
            }

            return HttpContext.Current.Session[ShoppingCartItemsessionKey].ToString();
        }
        public static ShoppingCartActions GetCart()
        {
            var cart = new ShoppingCartActions();
            cart.ShoppingCartId = cart.GetCartId();
            return cart;
        }
        // Helper method to simplify shopping cart calls
        //public static ShoppingCartActions GetCart(Controller controller)
        //{
        //    return GetCart(controller.HttpContext);
        //}
        public void MigrateCart(string userName)
       {

           CartApplicationService cartApplicationService = new CartApplicationService();

           cartApplicationService.MigrateCart(userName, ShoppingCartId);
           HttpContext.Current.Session[ShoppingCartItemsessionKey] = userName;

       }
        public void MigrateUser(string Email,string FirstName,string LastName)
        {
            CustomerApplicationService customerApplicationService = new CustomerApplicationService();

            customerApplicationService.MigrateUser(Email, FirstName, LastName);
        }
    }
}