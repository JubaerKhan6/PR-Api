using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UserManagementService.Models;
using UserManagementService.Models.Authentication.Login;
using UserManagementService.Models.Authentication.Signup;
using UserManagementService.Services.EmailRepository;

namespace UserManagementService.Services.AuthenticateRepository
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly UserManager<IdentityUser> _usermanager;
        private readonly RoleManager<IdentityRole> _rolemanager;
        private readonly SignInManager<IdentityUser> _signinManager;
        private readonly ServiceDbContext _context;


        private readonly IMailService _emailService;
        public AuthenticationService(UserManager<IdentityUser> usermanager, RoleManager<IdentityRole> rolemanager, SignInManager<IdentityUser> signinManager, IMailService emailService, ServiceDbContext context)
        {
            _usermanager = usermanager;
            _rolemanager = rolemanager;
            _emailService = emailService;
            _signinManager = signinManager;
            _context = context;

        }
        public async Task<ApiFeedback<int>> RegisterUser(RegisterUser registerUser)
        {
            //CheckUserExistance
            var userExists = await _usermanager.FindByEmailAsync(registerUser.email);
            if (userExists != null)
            {
                return new ApiFeedback<int> { IsSuccess = false, StatusCode = 403, Message = "User already exists!" };
            }

            //AddUser
            IdentityUser user = new IdentityUser()
            {
                Email = registerUser.email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerUser.userame,
                TwoFactorEnabled = true
            };
            //Check Email Validity 
            if (user.Email != null && user.UserName != null)
            {
                var message = new Message(new string[] { user.Email }, "Welcome to Perky Rabbit, " + user.UserName, "This is an Automated Message sent to Validate your Email Address. Please use this Email Address to Recieve your OTP when loggin in, and Weekly Updates . ");

                try
                {
                    _emailService.SendEmail(message);

                }
                catch

                {
                    return new ApiFeedback<int> { IsSuccess = false, StatusCode = 403, Message = "Email Not Found. Please double check your Email." };

                }
                var result = await _usermanager.CreateAsync(user, registerUser.password);
                if (!result.Succeeded)
                {
                    return new ApiFeedback<int> { IsSuccess = false, StatusCode = 403, Message = "User Creation Failed!" };

                }
                else
                {  //AddingRoletoUser
                    await _usermanager.AddToRoleAsync(user, "User");
                    return new ApiFeedback<int> { IsSuccess = true, StatusCode = 200, Message = "User Created Successfully!" };


                }
            }
            else { return new ApiFeedback<int> { IsSuccess = false, StatusCode = 403, Message = "Please Enter a valid Username and Password" }; }

        }
        public async Task<ApiFeedback<int>> LoginUser(LoginModel loginModel)
        {
            var user = await _usermanager.FindByNameAsync(loginModel.UserName);

            if (user != null && await _usermanager.CheckPasswordAsync(user, loginModel.Password))
            {
                //Send OTP EMAIL 
               
                var token = await _usermanager.GenerateTwoFactorTokenAsync(user, "Email");
                var message = new Message(new string[] { user.Email }, "Your One Time Password", "Welcome, Perky Rabbit Recruiters. Your One Time Password (OTP) is : " + token);
                try
                {
                    _emailService.SendEmail(message);
                    var sp = await _context.Database.ExecuteSqlRawAsync($"EnterOTPtoDB [{token}],[{user.Id}]");

                    return new ApiFeedback<int> { IsSuccess = true, StatusCode = 200, Message = "An OTP has been sent to your Registered Email Address." };

                }
                catch (Exception ex)
                {
                    throw ex;

                }


            }
            if (user == null)
            {
                return new ApiFeedback<int> { IsSuccess = false, StatusCode = 403, Message = "This Username Does not Exist!" };

            }
            else if (!await _usermanager.CheckPasswordAsync(user, loginModel.Password))
            {
                return new ApiFeedback<int> { IsSuccess = false, StatusCode = 403, Message = "Wrong Password" };

            }
            else
            {
                return new ApiFeedback<int> { IsSuccess = false, StatusCode = 403, Message = "Something Went Wrong. Please contact your Software Developers." };

            }
        }
        public async Task<OtpResponse<int>> OTPVerification(string code, string username)
        {
            {
                var user = await _usermanager.FindByNameAsync(username);
                var id = user.Id;
                var count = 0;

              
               
                //VerifyOTP
                try
                {
                    IEnumerable<OTP> otp = _context.Otp.FromSqlRaw($"CheckOTP [{code}],[{id}]");
                    foreach (var o in otp)
                    {
                        count = 1;


                    }
                    if (count == 0)
                    {
                        return new OtpResponse<int> { IsSuccess = false, StatusCode = 403, Message = "Login Failed" };

                    }
                    else
                    {
                        return new OtpResponse<int> { IsSuccess = true, StatusCode = 200, Message = "Login Succeeded", email=user.Email, userame=user.UserName };
                    }


                }
                catch
                {
                    return new OtpResponse<int> { IsSuccess = false, StatusCode = 403, Message = "Login Failed" };

                }
            }
        }
    }
}
   
