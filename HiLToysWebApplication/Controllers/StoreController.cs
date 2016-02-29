using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using EMD;
using System.Text;
using System.Web.Mvc;
using HiLToysApplicationServices;
using HiLToysViewModel;
using HiLToysWebApplication.Models;
namespace HiLToysWebApplication.Controllers
{
    public class StoreController : Controller
    {
       
        //
        // GET: /Store/
        private ApplicationDbContext storeDB=new ApplicationDbContext();
       
        public ActionResult Index()
        {
            ProductApplicationService productApplicationService = new ProductApplicationService();
            
            ProductViewModel productViewModel = new ProductViewModel();
            productViewModel.Products = productApplicationService.GetProducts().Products;
            return View("Index", productViewModel);
        }
        public ActionResult BeginProductInquiry()
        {
            return PartialView("ProductInquiry");
        }
      
        public ActionResult Browse(string category)
        {
            ProductApplicationService productApplicationService = new ProductApplicationService();
            ProductViewModel productViewModel2 = new ProductViewModel();
            // Retrieve Category and its Associated Products from database
            ViewData["category"] = category;
            productViewModel2.Products = productApplicationService.GetCategoryProducts(category).Products;
            return View("Browse", productViewModel2);
        }
        [ChildActionOnly]
        public ActionResult CategoryMenu()
        {

            return PartialView();
        }
        public ActionResult BrowseMob(string categoryName)
        {
            ProductApplicationService productApplicationService = new ProductApplicationService();
            ProductViewModel productViewModel = new ProductViewModel();
            // Retrieve Category and its Associated Products from database

            productViewModel.Products = productApplicationService.GetCategoryProducts(categoryName).Products;
            return View("BrowseMob", productViewModel);
        }
        public ActionResult Details(int id)
        {
            var product = storeDB.Products.Find(id);
            ProductViewModel productViewModel2 = new ProductViewModel();
            productViewModel2.Product = product;
            return View("Details", productViewModel2);
        }
       
        public ActionResult AddToCart(int ProductID, string ProductName, double UnitPrice, string Quantity)
        {
           // Int32 CartCount = 0;
            //CartCount = Convert.ToInt32(ViewData["CartCount"]);
            //CartCount = CartCount + 1;
            //ViewData["CartCount"] = CartCount;

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
            //if (Request.IsAjaxRequest())
              //  HttpContext.Response.AddHeader("Content-Title", 8);
            Session["payment_amt"] = cartViewModel.Cart.CartTotal;
            return Json(new
            {
                ReturnStatus = cartViewModel.ReturnStatus,
                viewModel = cartViewModel,
                //ValidationErrors = cartViewModel.ValidationErrors,
                //MessageBoxView = Helpers.MvcHelpers.RenderPartialView(this, "_MessageBox", cartViewModel),
            }, JsonRequestBehavior.AllowGet);
            
            //return PartialView("CartSummary");

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
      
       [ChildActionOnly]
        public ActionResult CategoryMenuMob()
        {
            var Categories = storeDB.Categories.ToList();

            return PartialView(Categories);
        }
	}
}