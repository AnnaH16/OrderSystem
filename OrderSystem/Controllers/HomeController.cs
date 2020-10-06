using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrderSystem.Models;

namespace OrderSystem.Controllers
{
    public class HomeController : Controller
    {        
        public ActionResult Index()
        {            
            return View();
        }

        public ActionResult Search()
        {
            List<OrderViewModel> OrderVM = new List<OrderViewModel>();
            using (NorthwindEntities db = new NorthwindEntities())
            {
                List<OrderViewModel> source = (from o in db.Orders
                                               join c in db.Customers on o.CustomerID equals c.CustomerID into oc
                                               from c in oc.DefaultIfEmpty()
                                               join e in db.Employees on o.EmployeeID equals e.EmployeeID into oe
                                               from e in oe.DefaultIfEmpty()
                                               select new OrderViewModel
                                               {
                                                   OrderID = o.OrderID,
                                                   CustomerID = o.CustomerID,
                                                   CompanyName = c.CompanyName,
                                                   EmployeeID = o.EmployeeID,
                                                   LastName = e.LastName,
                                                   FirstName = e.FirstName,
                                                   OrderDate = o.OrderDate,
                                                   ShippedDate = o.ShippedDate,
                                                   RequiredDate = o.RequiredDate
                                               }).ToList();

                return View(source);
            }                
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
