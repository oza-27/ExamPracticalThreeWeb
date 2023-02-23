using Exam.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PracticalRazorTaskAPI.Model;

namespace Exam.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<Orders> orders { get; set; }
        public DbSet<OrderItems> orderItems { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Category> categories { get; set; }
    }
}
