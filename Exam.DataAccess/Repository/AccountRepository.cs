using Exam.DataAccess.Repository.IRepository;
using Exam.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Exam.DataAccess.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly RoleManager<IdentityRole> _rolemanager;
        private IConfiguration _configuration;

        public AccountRepository(UserManager<ApplicationUser> usermanager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _usermanager = usermanager;
            _rolemanager = roleManager;
            _configuration = configuration;
        }

        public async Task<ResponseModel> Login(Login appLog)
        {
            var res = new ResponseModel();
            var user = await _usermanager.FindByNameAsync(appLog.Username);
            if (user != null && await _usermanager.CheckPasswordAsync(user, appLog.Password))
            {
                var userRoles = await _usermanager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                if (userRoles.Count != 0 && userRoles != null)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRoles[0]));
                }
                var authSignKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["jwt:ValidIssuer"],
                    audience: _configuration["jwt:ValidAudience"],
                    expires: DateTime.UtcNow.AddDays(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSignKey, SecurityAlgorithms.HmacSha256)
                    );
                var getToken = new JwtSecurityTokenHandler().WriteToken(token);

                res.Data = getToken;
                res.message = "User Login successfully";
                res.Status = 200;
            }
            return res;
        }

        public async Task<ResponseModel> SignUp(Register model)
        {
            var res = new ResponseModel();
            var user = await _usermanager.FindByNameAsync(model.Username);
            if (user != null)
            {
                res.message = "User already exist";
                res.Status = 200;
                res.Data = "";
                return res;
            }

            ApplicationUser UserApp = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
            };

            res.Data = await _usermanager.CreateAsync(UserApp, model.Password);
            res.message = "User created successfully";
            res.Status = 200;
            return res;


        }

        public async Task<ResponseModel> SignUpAdmin(Register register)
        {
            var res = new ResponseModel();
            var userExists = await _usermanager.FindByNameAsync(register.Username);

            ApplicationUser user = new ApplicationUser()
            {
                Email = register.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = register.Username
            };
            if (await _rolemanager.RoleExistsAsync(UserRoles.Admin))
            {
                await _usermanager.AddToRoleAsync(user, UserRoles.Admin);
            }
            res.Data = await _usermanager.CreateAsync(user, register.Password);
            res.message = "User created successfully";
            res.Status = 200;
            return res;
        }
    }
}
