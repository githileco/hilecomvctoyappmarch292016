using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiLToysViewModel;
using HiLToysDataModel;
using HiLToysWebApplication.HiLToysDataAccessServices;
using HiLToysBusinessServices;
using HiLToysWebApplication.Models;

namespace HiLToysApplicationServices
{
    public  class CartApplicationService
    {
        
        public CartViewModel AddCartDetailLineItem(CartViewModel cartViewModel)
        {
            CartDataAccessService cartDataAccessService = new CartDataAccessService();
            CartViewModel incartViewModel = new CartViewModel();
            CartViewModel incartViewModel2 = new CartViewModel();
            List<String> returnMessage = new List<String>();
            returnMessage.Add(cartViewModel.Cart.ProductName + " is added in the cart");
            int count = 0;
            incartViewModel.ReturnStatus = true;
            Boolean returnMessageforAddtoCart = true;
            returnMessageforAddtoCart = cartDataAccessService.AddToCart(cartViewModel);
            if (returnMessageforAddtoCart != true)
            {
                returnMessage.Add(cartViewModel.Cart.ProductName + " is not added in the cart");
                incartViewModel.ReturnStatus = false;
            }
            incartViewModel.ReturnMessage = returnMessage;
           incartViewModel2= cartDataAccessService.GetCartCount(cartViewModel);
           count = incartViewModel2.Cart.Count;
           incartViewModel.Cart.Count = count;
            return incartViewModel;
        }
         public void MigrateCart(string userName,string ShoppingCartId)
         {

            CartDataAccessService cartDataAccessService = new CartDataAccessService();
            cartDataAccessService.MigrateCart(userName,ShoppingCartId);
        }
        public CartViewModel GetCarts(string CartID)
        {
            CartDataAccessService cartDataAccessService = new CartDataAccessService();
            CartViewModel viewModel = new CartViewModel();
            viewModel.Cart.CartID = CartID;
            viewModel = cartDataAccessService.GetCarts(viewModel);
            

            return viewModel;

        }
        //GetCartCount(CartViewModel cartViewModel)
        public CartViewModel UpdateCartDetailLineItem(CartViewModel cartViewModel)
        {
            CartDataAccessService cartDataAccessService = new CartDataAccessService();
            CartViewModel viewModel = new CartViewModel();
            viewModel = cartDataAccessService.UpdateCartDetailLineItem(cartViewModel);
            return viewModel;
        }
        public CartViewModel DeleteCartDetailLineItem(CartViewModel cartViewModel)
        {
            CartDataAccessService cartDataAccessService = new CartDataAccessService();
            CartViewModel viewModel = new CartViewModel();
            viewModel = cartDataAccessService.DeleteCartDetailLineItem(cartViewModel);
            return viewModel;
        }
        public CartViewModel GetCartCount(CartViewModel cartViewModel)
        {
            CartDataAccessService cartDataAccessService = new CartDataAccessService();
            //CartViewModel viewModel = new CartViewModel();
           // viewModel = cartDataAccessService.GetCartCount(cartViewModel);
            return cartDataAccessService.GetCartCount(cartViewModel); 
        }
    }
    
}
