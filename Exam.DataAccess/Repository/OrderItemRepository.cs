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
    public class OrderItemRepository : Repository<OrderItems>, IOrderItemRepository
    {
        private ApplicationDbContext _db;

        public OrderItemRepository(ApplicationDbContext db):base(db)
        {
            _db = db;   
        }
        public void Update(OrderItems obj)
        {
            _db.orderItems.Update(obj);
        }
    }
}
