
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
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[ShoppingCartItemsessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[ShoppingCartItemsessionKey] = context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();

                    // Send tempCartId back to client as a cookie
                    context.Session[ShoppingCartItemsessionKey] = tempCartId.ToString();
                }
            }

            return context.Session[ShoppingCartItemsessionKey].ToString();
        }
        public static ShoppingCartActions GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCartActions();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }
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