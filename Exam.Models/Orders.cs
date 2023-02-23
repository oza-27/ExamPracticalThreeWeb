using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Models
{
    public class Orders
    {
        [Key]
        public int OrderId { get; set; }
        public DateOnly OrderDate { get; set; }
        public string Note { get; set; }
        public int DiscountAmount { get; set; }
        public enum StatusType
        {
            Open,
            Draft,
            Shipped,
            Paid
        };
        public double TotalAmount { get; set; }
        public string CustomerName { get; set; }
        [EmailAddress]
        public string CustEmail { get; set; }
        public int CustomerContactNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime createdOn { get; set; } = DateTime.Now;
        public DateTime updatedOn { get; set; } = DateTime.Now;
    }
}
