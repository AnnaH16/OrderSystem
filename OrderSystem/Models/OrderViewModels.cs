using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OrderSystem.Models
{
    public class OrderViewModel
    {
        
        [Display(Name ="訂單編號")]
        public int OrderID { get; set; }

        [Display(Name = "客戶編號")]
        public string CustomerID { get; set; }

        [Display(Name = "客戶")]
        public string CompanyName { get; set; }        

        [Display(Name = "員工編號")]
        public int? EmployeeID { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        [Display(Name = "員工")]
        public string EmployeeName
        {
            get
            {
                return LastName + " " + FirstName;
            }
        }

        [Display(Name = "訂購日期")]
        [DataType(DataType.Date)]
        public DateTime? OrderDate { get; set; }

        [Display(Name = "船運日期")]
        [DataType(DataType.Date)]
        public DateTime? ShippedDate { get; set; }

        [Display(Name = "送達日期")]
        [DataType(DataType.Date)]
        public DateTime? RequiredDate { get; set; }
    }
}