using Exam.DataAccess.Repository.IRepository;
using Exam.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracticalRazorTaskAPI.Model;
using System.Net;

namespace ExamPracticalThree.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("GetCategory/")]
        public IEnumerable<Category> GetCategories()
        {
            return _unitOfWork.Category.GetAll();
        }

        [HttpGet]
        [Route("GetProducts/")]
        public IEnumerable<Product> GetProducts()
        {
            return _unitOfWork.Product.GetAll();
        }
        [HttpGet]
        [Route("GetOrders/")]
        public IEnumerable<Orders> GetOrder()
        {
            var result = _unitOfWork.Order.GetAll();
            return result;
        }
        [HttpPost]
        [Route("AddOrder")]

        public async Task<IActionResult> AddOrder(Orders order)
        {
            _unitOfWork.Order.add(order);
            _unitOfWork.Save();
            return Ok(order);
        }

        [HttpPost]
        [Route("AddOrderItems/")]

        public async Task<IActionResult> AddOrderItems(OrderItems orderItems)
        {
            _unitOfWork.OrderItem.add(orderItems);
            _unitOfWork.Save();
            return Ok(orderItems);
        }
    }
}
