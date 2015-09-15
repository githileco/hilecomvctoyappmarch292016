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

//using EMD;
using HiLToysWebApplication.Models;

namespace HiLToysWebApplication.HiLToysDataAccessServices
{
        
    public class ProductDataAccessService
    {
        private IApplicationDbContext storeDB;
        public ProductDataAccessService()
        {
            storeDB = new HiLToysApplicationDbContext();
        }
        //public ProductDataAccessService(IApplicationDbContext dbContext)
        //{
        //    storeDB = dbContext;
        //}
       
        public HiLToysDataModel.Product GetProductInformation(int productID)
        {
            HiLToysDataModel.Product product = new HiLToysDataModel.Product();
             product = storeDB.Products.Single(
                        sproduct => sproduct.ProductID == productID);
             return product;
        }
        public HiLToysDataModel.Category GetCategoryInformation(string categoryName)
        {
            HiLToysDataModel.Category category = new HiLToysDataModel.Category();
            category = storeDB.Categories.Single(
                       scategory => scategory.CategoryName == categoryName);
            return category;
        }
        public List<HiLToysDataModel.Product> GetProducts()
        {
               return storeDB.Products.ToList(); 

        }
        
     public List<HiLToysDataModel.Product> GetCategoryProducts(string category)
       {
           HiLToysDataModel.Category rtndcategory = new HiLToysDataModel.Category();
           rtndcategory = GetCategoryInformation(category);
           List<HiLToysDataModel.Product> products = new List<HiLToysDataModel.Product>();
           products= storeDB.Products.Where(product => product.CategoryID == rtndcategory.CategoryID).ToList();
            
            return products;
       }
     
    }
}
