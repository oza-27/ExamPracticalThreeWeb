using Exam.DataAccess.Repository.IRepository;
using Exam.Models;
using Exam.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PracticalRazorTaskAPI.Model;

namespace ExamPracticalThree.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private UserManager<ApplicationUser> _userManager;

        public OrdersController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("GetProducts/")]
        public async Task<IActionResult> GetProducts()
        {
            var result = _unitOfWork.Product.GetAll();
            var prodList = result.Select(x => new
            {
                x.ProductId,
                x.Name,
                x.Quantity,
                x.Price
            });

            return Ok(prodList);
        }

        [HttpGet]
        [Route("GetOrders/")]
        public async Task<IActionResult> GetOrder()
        {
            var result = _unitOfWork.Order.GetAll();
            var filtData = result.Select(x => new
            {
                x.OrderId,
                x.Note,
                x.StatusType,
                x.createdOn
            });
            return Ok(filtData);
        }

        [HttpPost]
        [Route("AddOrder/")]
        public async Task<IActionResult> AddOrder([FromBody] RequestOrderItems orderReq)
        {
            var user = await _userManager.FindByNameAsync(orderReq.Username);
            var totalAmt = orderReq.Price * orderReq.Quantity;
            if (user == null)
            {
                return BadRequest("You have to login first");
            }
            else
            {
                var check = _unitOfWork.Order.GetFirstOrDefault(x => x.CustomerName == orderReq.Username);
                var orderData = new Orders();
                if (orderData == null)
                {
                    orderData.CustomerName = user.UserName;
                    orderData.CustEmail = user.Email;
                    orderData.CustomerContactNumber = user.PhoneNumber;
                    orderData.StatusType = StatusType.Open.ToString();
                    orderData.IsActive = true;
                    orderData.DiscountAmount = 0;
                    orderData.Note = "";
                    orderData.TotalAmount = totalAmt;

                    _unitOfWork.Order.add(orderData);
                    _unitOfWork.Save();
                }
                else
                {
                    var orderId = _unitOfWork.Order.GetFirstOrDefault(x => x.CustomerName == orderReq.Username);
                    var orderItemData = new OrderItems()
                    {
                        Quantity = orderReq.Quantity,
                        OrderId = orderId.OrderId,
                        ProductId = orderReq.ProductId,
                    };

                    _unitOfWork.OrderItem.add(orderItemData);
                    _unitOfWork.Save();
                }
            }
            return Ok(new ResponseModel()
            {
                Message = "order Added successfully:"
            });
        }
    }
}
