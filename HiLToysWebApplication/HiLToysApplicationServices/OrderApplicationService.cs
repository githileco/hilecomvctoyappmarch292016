using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiLToysDataModel;
using HiLToysDataModel.Models;

using HiLToysViewModel;
using HiLToysWebApplication.Models;
using HiLToysWebApplication.HiLToysDataAccessServices;

namespace HiLToysApplicationServices
{
    public class OrderApplicationService
    {
        
        public OrderViewModel BeginOrderEntry(OrderViewModel orderViewModel)
        {
           // OrderViewModel orderViewModel = new OrderViewModel();

            CustomerDataAccessService customerDataAccessService = new CustomerDataAccessService();
           // Customer customer = customerDataAccessService.GetCustomerInformation(orderViewModel.Customer.CustomerID);

            orderViewModel.Customer = customerDataAccessService.GetCustomerInformation(orderViewModel.Customer.CustomerID);

            OrderDataAccessService orderDataAccessService = new OrderDataAccessService();
            orderViewModel.Shippers = orderDataAccessService.GetShippers();

           // OrderBusinessService orderBusinessService = new OrderBusinessService();
            //orderViewModel.Order = orderBusinessService.InitializeOrderHeader(customer);

            return orderViewModel;

        }
        public void UpdateOrderEmptyCart(int currentOrderId, string PaymentConfirmation, string CartID)
        {
            CartDataAccessService cartDataAccessService = new CartDataAccessService();
            cartDataAccessService.UpdateOrderEmptyCart(currentOrderId, PaymentConfirmation, CartID);
        }
       public OrderViewModel CreateOrder(OrderViewModel orderViewModel)
       {
           CartDataAccessService cartDataAccessService = new CartDataAccessService();
           return cartDataAccessService.CreateOrder(orderViewModel);
       }
       public OrderViewModel GetOrderDetails(int orderID)
       {

           OrderDataAccessService orderDataAccessService = new OrderDataAccessService();
           OrderViewModel orderViewModel = new OrderViewModel();
            List<OrderDetailProductResult> orderDetailProductResult =new  List<OrderDetailProductResult> ();
            orderViewModel = orderDataAccessService.GetOrderDetails(orderID);
         //  OrderCustomer orderCustomer = orderDataAccessService.GetOrder(orderID);
          // orderViewModel.OrderDetailProductResults = orderDetailProductResult;
          // orderViewModel.Order = orderCustomer.Order;
          // orderViewModel.Customer = orderCustomer.Customer;

           return orderViewModel;

       }
       public OrderViewModel GetOrderDetailsx(int orderID)
       {

           OrderDataAccessService orderDataAccessService = new OrderDataAccessService();
           OrderViewModel orderViewModel = new OrderViewModel();
           List<OrderDetailProductResult> orderDetailProductResult = new List<OrderDetailProductResult>();
           orderViewModel = orderDataAccessService.GetOrderDetails(orderID);
            OrderCustomer orderCustomer = orderDataAccessService.GetOrder(orderID);
           orderViewModel.OrderDetailProductResults = orderDetailProductResult;
           orderViewModel.Order = orderCustomer.Order;
           orderViewModel.Customer = orderCustomer.Customer;

           return orderViewModel;

       }
       public OrderViewModel BeginOrderEdit(int orderID)
       {

           OrderDataAccessService orderDataAccessService = new OrderDataAccessService();
           OrderViewModel orderViewModel = new OrderViewModel();
           OrderCustomer orderCustomer = orderDataAccessService.GetOrder(orderID);
           orderCustomer.Order.OrderTotal = orderDataAccessService.GetOrderTotal(orderID);
           orderCustomer.Order.OrderTotalFormatted = orderCustomer.Order.OrderTotal.ToString("C");

           orderViewModel.Customer = orderCustomer.Customer;
           orderViewModel.Order = orderCustomer.Order;
           orderViewModel.Shippers = orderDataAccessService.GetShippers();
           orderViewModel.Order.ShipperName = orderCustomer.Shipper.CompanyName;

           return orderViewModel;

       }

    }
}
