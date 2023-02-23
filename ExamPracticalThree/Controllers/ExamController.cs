using Exam.DataAccess.Repository.IRepository;
using Exam.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracticalRazorTaskAPI.Model;

namespace ExamPracticalThree.Controllers
{
    [Authorize(Roles ="admin,user")]
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;

        public ExamController(IUnitOfWork unitOfWork)
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
    }
}
