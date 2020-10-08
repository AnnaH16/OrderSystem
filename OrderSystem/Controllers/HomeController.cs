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
                                                   RequiredDate = o.RequiredDate
                                               }).ToList();

                return View(source);
            }                
        }

        public ActionResult Creat()
        {
            using (NorthwindEntities db = new NorthwindEntities())
            {
                //DropDownList
                ViewBag.CustomerID = CustomerSelectItemList("");                           
                ViewBag.EmployeeID = EmployeeSelectItemList();
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creat(OrderViewModel OrderVM)
        {
            if (ModelState.IsValid)
            {
                using (NorthwindEntities db = new NorthwindEntities())
                {
                    //取DB裡最大OrderID
                    int MaxOrderID = db.Orders.Select(x => x.OrderID).Max();

                    Orders orders = AutoMapper.Mapper.Map<Orders>(OrderVM);
                    orders.OrderID = MaxOrderID + 1;

                    db.Orders.Add(orders);
                    db.SaveChanges();

                    TempData["message"] = "新增成功";
                    return RedirectToAction("Search");
                }
            }

            //未通過，再次返回顯示Form表單
            TempData["message"] = "新增失敗";
            return RedirectToAction("Creat");
        }

        public ActionResult Edit(int OrderID, string CustomerID, int? EmployeeID, DateTime? OrderDate, DateTime? RequiredDate)
        {
            //DropDownList
            ViewBag.CustomerID = CustomerSelectItemList(CustomerID);

            EditOrderViewModel OrderVM = new EditOrderViewModel();
            OrderVM.OrderID = OrderID;
            OrderVM.EmployeeID = EmployeeID;
            OrderVM.OrderDate = OrderDate.HasValue ? OrderDate.Value : DateTime.Today;
            OrderVM.RequiredDate = RequiredDate.HasValue ? RequiredDate.Value : DateTime.Today;

            return View(OrderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID, CustomerID, OrderDate, RequiredDate")] EditOrderViewModel EditOrderVM)
        {
            if (ModelState.IsValid)
            {
                using (NorthwindEntities db = new NorthwindEntities())
                {
                    Orders source = db.Orders.Find(EditOrderVM.OrderID);
                    if(source == null)
                    {
                        return HttpNotFound();
                    }
                    source.CustomerID = EditOrderVM.CustomerID;
                    source.OrderDate = EditOrderVM.OrderDate;
                    source.RequiredDate = EditOrderVM.RequiredDate;
                    db.SaveChanges();

                    TempData["message"] = "修改成功";
                    return RedirectToAction("Search");
                }
            }

            //未通過，再次返回顯示Form表單
            TempData["message"] = "修改失敗";
            return RedirectToAction("Edit");
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

        /// <summary>
        /// 客戶DropDownList
        /// </summary>
        /// <returns></returns>
        private List<SelectListItem> CustomerSelectItemList(string CustomerID)
        {
            using (NorthwindEntities db = new NorthwindEntities())
            {
                var Customerlist = db.Customers.ToList().Distinct();
                List<SelectListItem> CustomerSelectItemList = new List<SelectListItem>();
                foreach (var item in Customerlist)
                {
                    CustomerSelectItemList.Add(new SelectListItem()
                    {
                        Text = item.CompanyName,
                        Value = item.CustomerID,
                        Selected = false
                    });
                }

                //設定預選值
                if (string.IsNullOrWhiteSpace(CustomerID))
                {
                    CustomerSelectItemList.FirstOrDefault().Selected = true;
                }
                else
                {
                    var selectItem = CustomerSelectItemList.Where(x => x.Value == CustomerID).Count();
                    if (selectItem > 0)
                    {
                        CustomerSelectItemList.Where(x => x.Value == CustomerID).FirstOrDefault().Selected = true;
                    }
                    else
                    {
                        CustomerSelectItemList.FirstOrDefault().Selected = true;
                    }
                }

                return CustomerSelectItemList;
            }
        }

        /// <summary>
        /// 員工DropDownList 
        /// </summary>
        /// <returns></returns>
        private List<SelectListItem> EmployeeSelectItemList()
        {
            using (NorthwindEntities db = new NorthwindEntities())
            {
                var Employeelist = db.Employees.ToList().Distinct();
                List<SelectListItem> EmployeeSelectItemList = new List<SelectListItem>();
                foreach (var item in Employeelist)
                {
                    EmployeeSelectItemList.Add(new SelectListItem()
                    {
                        Text = item.FirstName.ToString() + " " + item.LastName.ToString(),
                        Value = item.EmployeeID.ToString(),
                        Selected = false
                    });
                }

                EmployeeSelectItemList.FirstOrDefault().Selected = true;
                return EmployeeSelectItemList;
            }
        }
    }
}
