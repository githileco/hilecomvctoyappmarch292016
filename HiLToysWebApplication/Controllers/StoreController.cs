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
       /* public ActionResult ProductInquiry(FormCollection postedFormData)
        {

            ProductApplicationService productApplicationService = new ProductApplicationService();
            ProductViewModel productViewModel = new ProductViewModel();

            productViewModel.PageSize = Convert.ToInt32(postedFormData["PageSize"]);
            productViewModel.SortExpression = Convert.ToString(postedFormData["SortExpression"]);
            productViewModel.SortDirection = Convert.ToString(postedFormData["SortDirection"]);
            productViewModel.CurrentPageNumber = Convert.ToInt32(postedFormData["CurrentPageNumber"]);
            productViewModel.PageID = Convert.ToString(postedFormData["PageID"]);

            if (HiLToysBusinessServices.Utilities.IsNumeric(postedFormData["ProductID"]) == true)
                productViewModel.Product.ProductID = Convert.ToInt32(postedFormData["ProductID"]);

            productViewModel.Product.ProductName = Convert.ToString(postedFormData["ProductName"]);

            productViewModel = productApplicationService.ProductInquiry(productViewModel);

            return Json(new
            {
                ReturnStatus = productViewModel.ReturnStatus,
                ViewModel = productViewModel,
                MessageBoxView = Helpers.MvcHelpers.RenderPartialView(this, "_MessageBox", productViewModel),
                ProductInquiryView = Helpers.MvcHelpers.RenderPartialView(this, "ProductInquiryGrid", productViewModel)
            }, JsonRequestBehavior.AllowGet);

        }*/
      /*  public ActionResult GetProductInformation(string productID)
        {
            int productNumber = 0;

            if (HiLToysBusinessServices.Utilities.IsNumeric(productID) == true)
                productNumber = Convert.ToInt32(productID);
            productNumber = 25;
            ProductApplicationService GetproductApplicationService = new ProductApplicationService();
            ProductViewModel GetProductViewModel = GetproductApplicationService.GetProductInformation(productNumber);

            return Json(new
            {
                ReturnStatus = GetProductViewModel.ReturnStatus,
                ViewModel = GetProductViewModel,
                MessageBoxView = Helpers.MvcHelpers.RenderPartialView(this, "_MessageBox", GetProductViewModel)
            }, JsonRequestBehavior.AllowGet);

        }
        
       */
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