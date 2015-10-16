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

            productViewModel2.Products = productApplicationService.GetCategoryProducts(category).Products;
            return View("Index", productViewModel2);
        }
        [ChildActionOnly]
        public ActionResult CategoryMenu()
        {

            return PartialView();
        }
	}
}