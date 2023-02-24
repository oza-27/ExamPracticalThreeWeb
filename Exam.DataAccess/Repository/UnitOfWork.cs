using Exam.DataAccess.Data;
using Exam.DataAccess.Repository.IRepository;
using Exam.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using PracticalRazorTaskAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;


        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
            Order = new OrderRepository(_db);
            OrderItem = new OrderItemRepository(_db);
        }
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public IOrderRepository Order { get; private set; }
        public IOrderItemRepository OrderItem { get; private set; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
