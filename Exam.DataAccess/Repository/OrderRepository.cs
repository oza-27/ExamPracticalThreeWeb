using Exam.DataAccess.Data;
using Exam.DataAccess.Repository.IRepository;
using Exam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.DataAccess.Repository
{
    public class OrderRepository : Repository<Orders> ,IOrderRepository
    {
        private ApplicationDbContext _db;

        public OrderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Orders obj)
        {
            _db.orders.Update(obj);
        }
    }
}
