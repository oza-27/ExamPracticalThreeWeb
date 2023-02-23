using PracticalRazorTaskAPI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Models
{
    public class OrderItems
    {
        [Key]
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public int Quantity { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }
    }
}
