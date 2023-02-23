using Exam.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.DataAccess.Repository.IRepository
{
    public interface IAccountRepository
    {
        public Task<ResponseModel> SignUp(Register appUser);
        public Task<ResponseModel> Login(Login appLog);
        public Task<ResponseModel> SignUpAdmin(Register register);
    }
}
