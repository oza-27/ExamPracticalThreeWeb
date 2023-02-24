using Exam.DataAccess.Repository.IRepository;
using Exam.Models;
using Exam.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        [Route("GetOrderById/")]
        public async Task<IActionResult> GetOrderbyId(int id)
        {

            var idData = (from o in _unitOfWork.Order.GetAll()
                          join oi in _unitOfWork.OrderItem.GetAll() 
                          on o.OrderId equals oi.OrderId
                          where o.OrderId == id
                          select new { oi }).ToList();
            return Ok(idData);
        }
        
        [HttpGet]
        [Route("GetAllOrders/")]
        public async Task<IActionResult> GetAllOrder()
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

        [Authorize]
        [HttpGet]
        [Route("FilterOrders/")]
        public async Task<IActionResult> GetFilterOrder(string? statusType, string? NameorEmail, DateTime? fromDate, DateTime? toDate)
        {


            if (statusType == null && NameorEmail == null && fromDate != null && toDate == null)
            {
                var datewise = _unitOfWork.Order.GetAll();

                var finaldata = datewise.Where(x => x.OrderDate >= fromDate).ToList();
                if (finaldata.Count == 0)
                {
                    return BadRequest("Data Not Found...");

                }

                return Ok(finaldata);
            }

            if (statusType == null && NameorEmail == null && fromDate != null && toDate != null)
            {
                var datewise = _unitOfWork.Order.GetAll();

                var finaldata = datewise.Where(x => x.OrderDate >= fromDate && x.OrderDate <= toDate).ToList();
                if (finaldata.Count == 0)
                {
                    return BadRequest("Data Not Found...");

                }

                return Ok(finaldata);
            }


            if (statusType != null && NameorEmail == null)
            {
                var order = _unitOfWork.Order.GetFirstOrDefault(x => x.StatusType == statusType);
                return Ok(order);
            }

            if (statusType == null && NameorEmail != null)
            {
                var order = _unitOfWork.Order.GetFirstOrDefault(x => x.CustomerName == NameorEmail);


                if (order == null)
                {
                    var data_a = _unitOfWork.Order.GetFirstOrDefault(x => x.CustEmail == NameorEmail);
                    return Ok(data_a);
                }

                return Ok(order);

            }
            var data = _unitOfWork.Order.GetAll();
            var filterdata = data.Select(x => new { x.OrderId, x.Note, x.StatusType, x.createdOn });
            return Ok(filterdata);
        }

        [Authorize]
        [HttpPut]
        [Route("EditstatusById/")]
        public async Task<IActionResult> EditstatusbyId(int id, StatusType status)
        {

            var Data = _unitOfWork.Order.GetFirstOrDefault(x => x.OrderId == id);
            if (Data.IsActive != false && Data.StatusType != StatusType.Shipped.ToString())
            {
                Data.StatusType = status.ToString();

                _unitOfWork.Order.Update(Data);
                _unitOfWork.Save();
            }
            return Ok(Data);
        }

        [Authorize]
        [HttpPut]
        [Route("EditActive/")]
        public async Task<IActionResult> EditActive(int id)
        {

            var Data = _unitOfWork.Order.GetFirstOrDefault(x => x.OrderId == id);

            Data.IsActive = true;

            _unitOfWork.Order.Update(Data);
            _unitOfWork.Save();

            return Ok(Data);
        }

        // Remove orderItem By OrderId

        [Authorize]
        [HttpDelete]
        [Route("DeleteItem/")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var orderitem = _unitOfWork.OrderItem.GetAll();
            var deletelist = orderitem.Where(o => o.OrderId == id).ToList();



            _unitOfWork.OrderItem.RemoveRange(deletelist);
            _unitOfWork.Save();

            var orderdata = _unitOfWork.Order.GetFirstOrDefault(x => x.OrderId == id);
            orderdata.TotalAmount = 0;
            _unitOfWork.Order.Update(orderdata);
            _unitOfWork.Save();

            return Ok(new ResponseModel() { Message = "Order Deleted Successfully...." });
        }

        [Authorize]
        [HttpPut]
        [Route("EditOrderItemQuantity")]
        public async Task<IActionResult> EditQuantity(int Quantity, int orderItemId)
        {
            var upQuantity = _unitOfWork.OrderItem.GetFirstOrDefault(x => x.OrderItemId == orderItemId);

            if (Quantity >= 1 && upQuantity.IsActive != true)
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
