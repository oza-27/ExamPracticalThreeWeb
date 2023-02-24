using Exam.DataAccess.Repository.IRepository;
using Exam.Models;
using Exam.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> AddOrder(RequestOrderItems orderReq)
        {
            var userdata = await _userManager.FindByNameAsync(orderReq.Username);
            var totalamount = orderReq.Price * orderReq.Quantity;
            if (userdata == null)
            {
                return BadRequest("You have To Login First");
            }
            else
            {
                var check = _unitOfWork.Order.GetFirstOrDefault(x => x.CustomerName == orderReq.Username);
                var orderdata = new Orders();
                if (check == null)
                {


                    orderdata.CustomerName = userdata.UserName;
                    orderdata.CustEmail = userdata.Email;
                    orderdata.CustomerContactNumber = userdata.PhoneNumber;
                    orderdata.StatusType = StatusType.Open.ToString();
                    orderdata.IsActive = true;
                    orderdata.DiscountAmount = 0;
                    orderdata.Note = "";
                    orderdata.TotalAmount = totalamount;


                    _unitOfWork.Order.add(orderdata);
                    _unitOfWork.Save();

                    var orderitemdata = new OrderItems()
                    {
                        Quantity = orderReq.Quantity,
                        OrderId = orderdata.OrderId,
                        ProductId = orderReq.ProductId,
                    };

                    _unitOfWork.OrderItem.add(orderitemdata);
                    _unitOfWork.Save();
                }
                else
                {
                    var orderid = _unitOfWork.Order.GetFirstOrDefault(x => x.CustomerName == orderReq.Username);
                    var orderitemdata = new OrderItems()
                    {
                        Quantity = orderReq.Quantity,
                        OrderId = orderid.OrderId,
                        ProductId = orderReq.ProductId,
                    };


                    orderid.TotalAmount = orderid.TotalAmount + totalamount;

                    _unitOfWork.Order.Update(orderid);
                    _unitOfWork.Save();



                    _unitOfWork.OrderItem.add(orderitemdata);
                    _unitOfWork.Save();
                }


            }


            return Ok(new ResponseModel()
            {
                Message = "Item added Successfully...."
            });
        }

        [Authorize]
        [HttpGet]
        [Route("GetOrder")]
        public async Task<IActionResult> GetOrder(string? statusType, string? Name_or_Email, DateTime? fromdate, DateTime? todate)
        {


            if (statusType == null && Name_or_Email == null && fromdate != null && todate == null)
            {
                var datewise = _unitOfWork.Order.GetAll();

                var finaldata = datewise.Where(x => x.OrderDate >= fromdate).ToList();
                if (finaldata.Count == 0)
                {
                    return BadRequest("Data Not Found...");

                }

                return Ok(finaldata);
            }

            if (statusType == null && Name_or_Email == null && fromdate != null && todate != null)
            {
                var datewise = _unitOfWork.Order.GetAll();

                var finaldata = datewise.Where(x => x.OrderDate >= fromdate && x.OrderDate <= todate).ToList();
                if (finaldata.Count == 0)
                {
                    return BadRequest("Data Not Found...");

                }

                return Ok(finaldata);
            }


            if (statusType != null && Name_or_Email == null)
            {
                var order = _unitOfWork.Order.GetFirstOrDefault(x => x.StatusType == statusType);
                return Ok(order);
            }

            if (statusType == null && Name_or_Email != null)
            {
                var order = _unitOfWork.Order.GetFirstOrDefault(x => x.CustomerName == Name_or_Email);


                if (order == null)
                {
                    var data_a = _unitOfWork.Order.GetFirstOrDefault(x => x.CustEmail == Name_or_Email);
                    return Ok(data_a);
                }

                return Ok(order);

            }
            var data = _unitOfWork.Order.GetAll();
            var filterdata = data.Select(x => new { x.OrderId, x.Note, x.StatusType, x.createdOn });
            return Ok(filterdata);
        }

        [Authorize]
        [HttpGet]
        [Route("GetOrderById")]
        public async Task<IActionResult> GetOrderbyId(int id)
        {

            var idData = (from o in _unitOfWork.Order.GetAll()
                          join oi in _unitOfWork.OrderItem.GetAll() 
                          on o.OrderId equals oi.OrderId
                          where o.OrderId == id
                          select new { oi }).ToList();
            return Ok(idData);
        }

        [Authorize]
        [HttpPut]
        [Route("updatestatusById")]
        public async Task<IActionResult> putstatusbyId(int id, StatusType status)
        {

            var idData = _unitOfWork.Order.GetFirstOrDefault(x => x.OrderId == id);
            if (idData.IsActive != false && idData.StatusType != StatusType.Shipped.ToString())
            {
                idData.StatusType = status.ToString();

                _unitOfWork.Order.Update(idData);
                _unitOfWork.Save();
            }
            return Ok(idData);
        }

        [Authorize]
        [HttpPut]
        [Route("updateActive")]
        public async Task<IActionResult> PutActive(int id)
        {

            var idData = _unitOfWork.Order.GetFirstOrDefault(x => x.OrderId == id);

            idData.IsActive = true;

            _unitOfWork.Order.Update(idData);
            _unitOfWork.Save();

            return Ok(idData);
        }

        // Remove orderItem By OrderId

        [Authorize]
        [HttpDelete]
        [Route("DeleteItem")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var orderitem = _unitOfWork.OrderItem.GetAll();
            var deletelist = orderitem.Where(oitem => oitem.OrderId == id).ToList();



            _unitOfWork.OrderItem.RemoveRange(deletelist);
            _unitOfWork.Save();

            var orderdata = _unitOfWork.Order.GetFirstOrDefault(x => x.OrderId == id);
            orderdata.TotalAmount = 0;
            _unitOfWork.Order.Update(orderdata);
            _unitOfWork.Save();

            return Ok(new ResponseModel() { Message = "Delete Item Successfully...." });
        }

        [Authorize]
        [HttpPut]
        [Route("EditOrderItemQuantity")]
        public async Task<IActionResult> EditQuantity(int Quantity, int orderItemId)
        {
            var upQuantity = _unitOfWork.OrderItem.GetFirstOrDefault(x => x.OrderItemId == orderItemId);

            if (Quantity >= 1 && upQuantity.IsActive != false)
            {
                upQuantity.Quantity = Quantity;
                _unitOfWork.OrderItem.Update(upQuantity);
                _unitOfWork.Save();
            }
            else
            {
                return Ok(new ResponseModel() { Message = "minimum Quantity Is must be 1" });
            }



            return Ok(new ResponseModel() { Message = "Update Quantity Successfully...." });
        }
    }
}
