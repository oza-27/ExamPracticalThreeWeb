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
        public string Note { get; set; }
        public double? DiscountAmount { get; set; }
        public string StatusType { get; set; }
        public double? TotalAmount { get; set; }
        public string CustomerName { get; set; }
        [EmailAddress]
        public string CustEmail { get; set; }
        public string CustomerContactNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now.Date;
        public DateTime createdOn { get; set; } = DateTime.Now;
        public DateTime updatedOn { get; set; } = DateTime.Now;
    }
}
