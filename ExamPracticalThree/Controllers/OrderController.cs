using Exam.DataAccess.Repository.IRepository;
using Exam.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExamPracticalThree.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private UserManager<ApplicationUser> _UserManager;
        private IConfiguration _Configuration;

        public OrderController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _Configuration = configuration;
            _unitOfWork = unitOfWork;   
            _UserManager = userManager;
        }
    }
}
