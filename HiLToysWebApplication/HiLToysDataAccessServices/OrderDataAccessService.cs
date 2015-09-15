using System;
using System.Data.Sql;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiLToysDataModel.Models;
using HiLToysDataModel;
using System.Data;
using System.Data.SqlClient;
using HiLToysViewModel;
//using EMD;
using HiLToysWebApplication.Models;

namespace HiLToysWebApplication.HiLToysDataAccessServices
{
    public class OrderDataAccessService
    {
        private IApplicationDbContext storeDB;
        public OrderDataAccessService()
        {
            storeDB = new HiLToysApplicationDbContext();
        }
        //public OrderDataAccessService(IApplicationDbContext dbContext)
        //{
        //    storeDB = dbContext;
        //}
        public OrderCustomer LoadOrderCustomer(List<HiLToysDataModel.OrderCustomerResult> orderCustomerResult)
        {
            OrderCustomer orderCustomer = new OrderCustomer();


            orderCustomer.Customer.CustomerID = orderCustomerResult[0].CustomerID;
            orderCustomer.Customer.CompanyName = orderCustomerResult[0].CompanyName;
            orderCustomer.Customer.Address = orderCustomerResult[0].Address;
            orderCustomer.Customer.City = orderCustomerResult[0].City;
            orderCustomer.Customer.ContactName = orderCustomerResult[0].ContactName;
            orderCustomer.Customer.ContactTitle = orderCustomerResult[0].ContactTitle;
            orderCustomer.Customer.Fax = orderCustomerResult[0].Fax;
            orderCustomer.Customer.Phone = orderCustomerResult[0].Phone;
            orderCustomer.Customer.PostalCode = orderCustomerResult[0].PostalCode;
            orderCustomer.Customer.Region = orderCustomerResult[0].Region;
            orderCustomer.Customer.Country = orderCustomerResult[0].Country;
            orderCustomer.Order.CustomerID = orderCustomerResult[0].CustomerID;
            orderCustomer.Order.OrderDate = orderCustomerResult[0].OrderDate;
            orderCustomer.Order.OrderID = orderCustomerResult[0].OrderID;
            orderCustomer.Order.ShipAddress = orderCustomerResult[0].ShipAddress;
            orderCustomer.Order.ShipCity = orderCustomerResult[0].ShipCity;
            orderCustomer.Order.ShipCountry = orderCustomerResult[0].ShipCountry;
            orderCustomer.Order.ShipName = orderCustomerResult[0].ShipName;
            orderCustomer.Order.ShipPostalCode = orderCustomerResult[0].ShipPostalCode;
            orderCustomer.Order.ShipRegion = orderCustomerResult[0].ShipRegion;
            orderCustomer.Order.ShipVia = orderCustomerResult[0].ShipVia;
            orderCustomer.Order.RequiredDate = orderCustomerResult[0].RequiredDate;
            orderCustomer.Shipper.CompanyName = orderCustomerResult[0].ShipperName;

            return orderCustomer;

        }

        public OrderViewModel LoadOrderDetailProduct(List<HiLToysDataModel.OrderDetailProductResult> OrderDetailProductResult)
        {
            OrderViewModel orderViewModel = new OrderViewModel();
            

            /*
              ProductID=p.ProductID,
                       ProductName=p.ProductName,
                       QuantityPerUnit=p.QuantityPerUnit,
                       UnitPrice=p.UnitPrice,
                       Quantity=od.Quantity,
                       CustomerID = cu.CustomerID,
                        FirstName = cu.FirstName,
                       LastName = cu.LastName,
                       OrderDate = od.OrderDate,
                       OrderID=ord.OrderID,
                       Discount = od.Discount
             */
            orderViewModel.Order.OrderID = OrderDetailProductResult[0].OrderID;
            orderViewModel.Order.OrderDate = OrderDetailProductResult[0].OrderDate;
           
            OrderCustomer orderCustomer = new OrderCustomer();
            orderCustomer.Customer.CustomerID = OrderDetailProductResult[0].CustomerID;
            orderCustomer.Customer.FirstName = OrderDetailProductResult[0].FirstName;
            orderCustomer.Customer.LastName = OrderDetailProductResult[0].LastName;
            orderViewModel.Customer = orderCustomer.Customer;
           
            for (int i = 0;  i < OrderDetailProductResult.Count; i++)
            {
                orderViewModel.OrderDetailProducts[i].Products.ProductID = OrderDetailProductResult[i].ProductID;
                orderViewModel.OrderDetailProducts[i].Products.ProductName = OrderDetailProductResult[i].ProductName;
                orderViewModel.OrderDetailProducts[i].Products.QuantityPerUnit = OrderDetailProductResult[i].QuantityPerUnit;

                orderViewModel.OrderDetailProducts[i].Products.UnitPrice = OrderDetailProductResult[i].UnitPrice;
                orderViewModel.OrderDetailProducts[i].OrderDetail.Quantity = OrderDetailProductResult[i].Quantity;
                orderViewModel.OrderDetailProducts[i].OrderDetail.OrderDate = OrderDetailProductResult[i].OrderDate;

                orderViewModel.OrderDetailProducts[i].OrderDetail.Discount = OrderDetailProductResult[i].Discount;
            }

            return orderViewModel;

        }
       /* public List<HiLToysDataModel.Models.Shipper> GetShippers()
        {
            HiLToysEMDModelContainer storeDB = new HiLToysEMDModelContainer();
            return storeDB.Shippers.ToList();

        }*/
        public List<OrderDetailProductResult> GetOrderDetailsx(int orderID)
        {
          //  HiLToysEMDModelContainer storeDB = new HiLToysEMDModelContainer();
            var rslt = from p in storeDB.Products join od in storeDB.OrderDetails on p.ProductID equals od.ProductID
                       join ord in storeDB.Orders on od.OrderID equals ord.OrderID
                       join cu in storeDB.Customers on ord.CustomerID equals cu.CustomerID
                       select new OrderDetailProductResult() {
                       ProductID=p.ProductID,
                       ProductName=p.ProductName,
                       QuantityPerUnit=p.QuantityPerUnit,
                       UnitPrice=p.UnitPrice,
                       Quantity=od.Quantity,
                       CustomerID = cu.CustomerID,
                       FirstName = cu.FirstName,
                       LastName = cu.LastName,
                       OrderDate = od.OrderDate,
                       OrderID=ord.OrderID,
                       Discount = od.Discount
                       
                       };


            if (orderID > 0) 
            { 
                rslt = from a in rslt where a.OrderID == orderID select a; 
            }

            
            return rslt.ToList<OrderDetailProductResult>(); 
        }
        public OrderViewModel GetOrderDetails(int orderID)
        {
           // HiLToysEMDModelContainer storeDB = new HiLToysEMDModelContainer();
            var rslt = from p in storeDB.Products
                       join od in storeDB.OrderDetails on p.ProductID equals od.ProductID
                       join ord in storeDB.Orders on od.OrderID equals ord.OrderID
                       join cu in storeDB.Customers on ord.CustomerID equals cu.CustomerID
                       select new OrderDetailProductResult()
                       {
                           ProductID = p.ProductID,
                           ProductName = p.ProductName,
                           QuantityPerUnit = p.QuantityPerUnit,
                           UnitPrice = p.UnitPrice,
                           Quantity = od.Quantity,
                           CustomerID = cu.CustomerID,
                           FirstName = cu.FirstName,
                           LastName = cu.LastName,
                           OrderDate = od.OrderDate,
                           OrderID = ord.OrderID,
                           Discount = od.Discount
                       };

            if (orderID > 0)
            {
                rslt = from a in rslt where a.OrderID == orderID select a;
            }


            //return rslt.ToList<OrderCustomerResult>());
            // return rslt.ToList<OrderCustomerResult>();
            return LoadOrderDetailProduct(rslt.ToList<OrderDetailProductResult>());


        }
        public OrderCustomer GetOrder(int orderID)
        {

           // HiLToysEMDModelContainer storeDB = new HiLToysEMDModelContainer();
            //OrderCustomer rsltorderCustomer = new OrderCustomer();

          var  rslt = from o in storeDB.Orders
                       join cu in storeDB.Customers on o.CustomerID equals cu.CustomerID
                       join shp in storeDB.Shippers on o.ShipVia equals shp.ShipperID
                      select new OrderCustomerResult()
                      {
                         CustomerID =cu.CustomerID,
                         CompanyName =cu.CompanyName,
                         Address =cu.Address,
                         City =cu.City,
                         ContactName =cu.ContactName,
                         ContactTitle =cu.ContactTitle,
                         Fax =cu.Fax,
                         Phone =cu.Phone,
                         PostalCode =cu.PostalCode,
                         Region =cu.Region,
                         Country =cu.Country ,
                         OrderDate =o.OrderDate,
                         OrderID =o.OrderID,
                         ShipAddress =o.ShipAddress,
                         ShipCity =o.ShipCity,
                         ShipCountry =o.ShipCountry,
                         ShipName =o.ShipName,
                         ShipPostalCode =o.ShipPostalCode,
                          ShipRegion =o.ShipRegion,
                          ShipVia =o.ShipVia,
                           RequiredDate =o.RequiredDate,
                          ShipperName =shp.CompanyName

            
                    };
           if (orderID > 0)
           {
               rslt = from a in rslt where a.OrderID == orderID select a;
           }


           //return rslt.ToList<OrderCustomerResult>());
          // return rslt.ToList<OrderCustomerResult>();
           return LoadOrderCustomer(rslt.ToList<OrderCustomerResult>());
        }
       /* public OrderCustomer GetOrder(int orderID)
        {
            List<HiLToysDataModel.OrderCustomerResult> rsltlst=new List<HiLToysDataModel.OrderCustomerResult>();
             rsltlst=GetOrderx(orderID);
             OrderCustomer x = new OrderCustomer();
            x= LoadOrderCustomer(rsltlst);
             return x;
        
        }*/
        public double GetOrderTotal(int orderID)
        {
           // HiLToysEMDModelContainer storeDB = new HiLToysEMDModelContainer();
            OrderDetail Xm=new OrderDetail();
            var total = (from orderDetails in storeDB.OrderDetails
                         where orderDetails.OrderID == orderID
                         select (int?)(orderDetails.Quantity) * (1.00 - orderDetails.Discount) * orderDetails.UnitPrice).Sum();
            return Convert.ToDouble(total);
        }
        public List<HiLToysDataModel.Models.Shipper> GetShippers()
        {
           // HiLToysEMDModelContainer storeDB = new HiLToysEMDModelContainer();
           // List<HiLToysDataModel.Models.Shipper> shippers = new List<HiLToysDataModel.Models.Shipper>();
           // shippers=storeDB.Shippers().

            var results = (from a in storeDB.Shippers  orderby a.CompanyName descending select a); 
            return results.ToList<HiLToysDataModel.Models.Shipper>();
        }
         
    }
}
