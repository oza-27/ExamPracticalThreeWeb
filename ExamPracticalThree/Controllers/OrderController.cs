using Exam.DataAccess.Repository.IRepository;
using Exam.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

namespace ExamPracticalThree.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IAccountRepository _accountRepository;
        private UserManager<ApplicationUser> _UserManager;
        private IConfiguration _Configuration;

        public OrderController(IAccountRepository accountRepository, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _Configuration = configuration;
            _UserManager = userManager;
        }
        [HttpPost]
        [Route("AddUser/")]
        public async Task<ResponseModel> SignUp([FromBody] Register model)
        {
            try
            {
                var result = _accountRepository.SignUp(model);
                return await result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        [HttpPost]
        [Route("login/")]
        public async Task<ResponseModel> Login([FromBody] Login login)
        {
            var result = _accountRepository.Login(login);
            return await result;
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<ResponseModel> RegisterAdmin([FromBody] Register model)
        {
            try
            {
                var result = _accountRepository.SignUpAdmin(model);
                return await result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw;
            }
        }
    }
}
