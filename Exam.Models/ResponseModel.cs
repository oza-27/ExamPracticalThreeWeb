using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Models
{
    public class ResponseModel
    {
        public string message { get; set; }
        public int Status { get; set; }
        public dynamic Data { get; set; }
    }
}
