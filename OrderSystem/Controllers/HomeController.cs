using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrderSystem.Models;
using OrderSystem.Service;

namespace OrderSystem.Controllers
{
    public class HomeController : Controller
    {
        OrderBaseService OrderService = new OrderBaseService();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search()
        {            
            List<OrderViewModel> source = OrderService.getOrderList();
            return View(source);            
        }

        public ActionResult Creat()
        {
            //DropDownList
            ViewBag.CustomerID = CustomerSelectItemList("");
            ViewBag.EmployeeID = EmployeeSelectItemList();
            return View();            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creat(OrderViewModel OrderVM)
        {
            if (ModelState.IsValid)
            {
                if (OrderService.addtOrder(OrderVM))
                {
                    TempData["message"] = "新增成功";
                    return RedirectToAction("Search");
                }
                else
                {
                    TempData["message"] = "新增失敗";
                    return RedirectToAction("Creat");
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
                if (OrderService.editOrder(EditOrderVM))
                {
                    TempData["message"] = "修改成功";
                    return RedirectToAction("Search");
                }
                else
                {
                    TempData["message"] = "修改失敗";
                    return RedirectToAction("Edit");
                }
            }

            //未通過，再次返回顯示Form表單
            TempData["message"] = "修改失敗";
            return RedirectToAction("Edit");
        }

        public ActionResult Delete(int OrderID, string CompanyName, string EmployeeName, DateTime? OrderDate, DateTime? RequiredDate)
        {
            OrderViewModel OrderVM = new OrderViewModel();
            OrderVM.OrderID = OrderID;
            OrderVM.CompanyName = CompanyName;
            OrderVM.EmployeeNameCopy = EmployeeName;
            OrderVM.OrderDate = OrderDate;
            OrderVM.RequiredDate = RequiredDate;

            return View(OrderVM);
        }


        [HttpPost]
        public ActionResult Delete([Bind(Include = "OrderID")] int OrderID)
        {
            if (OrderService.deleteOrder(OrderID))
            {
                TempData["message"] = "刪除成功";
            }
            else
            {
                TempData["message"] = "資料有誤，刪除失敗";
            }
            return RedirectToAction("Search");
        }

        /// <summary>
        /// 客戶DropDownList
        /// </summary>
        /// <returns>List<SelectListItem></returns>
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
        /// <returns>List<SelectListItem></returns>
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




        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}


    }
}
