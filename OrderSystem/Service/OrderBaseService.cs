using OrderSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderSystem.Service
{
    public class OrderBaseService
    {
        private NorthwindEntities db = new NorthwindEntities();

        /// <summary>
        /// 取得訂單清單
        /// </summary>
        /// <returns>List<OrderViewModel></returns>
        public List<OrderViewModel> getOrderList()
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

            return source;
        }

        /// <summary>
        /// 新增一筆Order資料
        /// </summary>
        /// <returns>bool值</returns>
        public bool addtOrder(OrderViewModel OrderVM)
        {
            //取DB裡最大OrderID
            int MaxOrderID = db.Orders.Select(x => x.OrderID).Max();

            Orders orders = AutoMapper.Mapper.Map<Orders>(OrderVM);
            orders.OrderID = MaxOrderID + 1;

            db.Orders.Add(orders);
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// 更新一筆Order資料
        /// </summary>
        /// <returns>bool值</returns>
        public bool editOrder(EditOrderViewModel EditOrderVM)
        {
            Orders source = db.Orders.Find(EditOrderVM.OrderID);
            if (source == null)
            {
                return false;
            }
            source.CustomerID = EditOrderVM.CustomerID;
            source.OrderDate = EditOrderVM.OrderDate;
            source.RequiredDate = EditOrderVM.RequiredDate;
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// 刪除一筆Order資料
        /// </summary>
        /// <returns>bool值</returns>
        public bool deleteOrder(int OrderID)
        {
            Orders deleteItem = db.Orders.Find(OrderID);
            if (deleteItem != null)
            {
                db.Orders.Remove(deleteItem);
                db.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}