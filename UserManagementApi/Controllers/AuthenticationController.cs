using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETCore.MailKit.Core;
using System.Data;
using UserManagementApi.Models;
using UserManagementService.Services.AuthenticateRepository;
using UserManagementService.Models;
using UserManagementService.Models.Authentication.Signup;
using UserManagementService.Services.EmailRepository;
using UserManagementService.Models.Authentication.Login;
using UserManagementService.Services;

namespace UserManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUnitofWork Uow;
      
        public AuthenticationController(IUnitofWork authenticationService)
        {
         Uow = authenticationService;

        }

        [HttpPost]
        [Route("Register")]

        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser)
        {
            var response = await Uow.AuthenticationService.RegisterUser(registerUser);
            return Ok(response);

        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginmodel)
        {
            var response = await Uow.AuthenticationService.LoginUser(loginmodel);
            return Ok(response);
        
        }

        [HttpPost]
        [Route("OTPVerification")]
        public async Task<ActionResult<RegisterUser>> OTPVerification(string code, string username)
        {
            var response = await Uow.AuthenticationService.OTPVerification(code, username);
            return Ok(response);
           
        }

    }
}
