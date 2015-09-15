using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiLToysViewModel;
using HiLToysDataModel.Models;

using HiLToysDataModel;
//using HiLToysDataAccessServices;
using HiLToysBusinessServices;
using HiLToysWebApplication.Models;
using HiLToysWebApplication.HiLToysDataAccessServices;

namespace HiLToysApplicationServices
{
    public class ProductApplicationService
    {
        
        
        /// <summary>
        /// Get Product Information
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        ///
       

       /// <summary>
        /// Product Inquiry
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
      
        public ProductViewModel GetProductInformation(int productID)
        {
            Product product = new Product();
            ProductViewModel productViewModel = new ProductViewModel();

            if (productID == 0)
            {
                List<String> returnMessage = new List<String>();
                returnMessage.Add("An invalid product ID was entered.");
                productViewModel.ReturnMessage = returnMessage;
                productViewModel.ReturnStatus = false;
                return productViewModel;
            }
            
            ProductDataAccessService productDataAccessService = new ProductDataAccessService();

            product = productDataAccessService.GetProductInformation(productID);
            productViewModel.Product = product;
            productViewModel.ReturnStatus = true;

            if (product.ProductID == 0)
            {
                List<String> returnMessage = new List<String>();
                returnMessage.Add(productID.ToString() + " is not a valid product ID");
                productViewModel.ReturnMessage = returnMessage;
                productViewModel.ReturnStatus = false;
            }

            return productViewModel;

        }
        public ProductViewModel GetProducts()
        {
            ProductDataAccessService productDataAccessService = new ProductDataAccessService();
            ProductViewModel viewModel = new ProductViewModel();

            viewModel.Products = productDataAccessService.GetProducts();
            return viewModel;

        }
       
        public ProductViewModel GetCategoryProducts(string category)
        {
            ProductDataAccessService productDataAccessService = new ProductDataAccessService();
            ProductViewModel viewModel = new ProductViewModel();


            viewModel.Products = productDataAccessService.GetCategoryProducts(category);
            return viewModel;

        }
        
    }
}
