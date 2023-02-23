using Exam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.DataAccess.Repository.IRepository
{
    public interface IOrderRepository : IRepository<Orders>
    {
        void Update(Orders obj);
    }
}
