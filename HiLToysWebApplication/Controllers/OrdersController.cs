using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HiLToysApplicationServices;
//using HiLToysDataAccessServices;
//using HiLToysWebApplication.HiLToysApplicationServices;
using HiLToysViewModel;
using HiLToysWebApplication.Models;
namespace HiLToysWebApplication.Controllers
{
    public class OrdersController : Controller
    {

        //
        // GET: /Orders/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult OrderEntryDetail(int orderID)
        {
            OrderApplicationService orderApplicationService = new OrderApplicationService();
            OrderViewModel orderViewModel = orderApplicationService.GetOrderDetailsx(orderID);

            return View("OrderEntryDetail", orderViewModel);
        }
        public ActionResult OrderEdit(int orderID)
        {
            OrderApplicationService orderApplicationService = new OrderApplicationService();
            OrderViewModel orderViewModel = orderApplicationService.BeginOrderEdit(orderID);
            return View("OrderEntryHeader", orderViewModel);
        }
      


	}
}