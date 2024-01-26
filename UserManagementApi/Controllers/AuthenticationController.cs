using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using UserManagementApi.Models;
using UserManagementApi.Models.Authentication.Signup;
using UserManagementService.Models;
using UserManagementService.Services;

namespace UserManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _usermanager;
        private readonly RoleManager<IdentityRole> _rolemanager;
      
        private readonly IMailService _emailService;
        public AuthenticationController(UserManager<IdentityUser> usermanager, RoleManager<IdentityRole> rolemanager, IMailService emailService)
        {
                _usermanager = usermanager;
            _rolemanager = rolemanager;
            _emailService = emailService;
          
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser, string role)
        {
            //CheckUserExistance
            var userExists = await _usermanager.FindByEmailAsync(registerUser.email);
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new Response { status = "Error", message = "User already exists!" });
            }

            //AddUser
            IdentityUser user = new IdentityUser()
            {
                Email = registerUser.email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerUser.userame
            };
            //CheckRoleExistance
            if(await _rolemanager.RoleExistsAsync(role))
            {
                var result = await _usermanager.CreateAsync(user, registerUser.password);
                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new Response { status = "Error", message = "User Creation Failed!" });

                }
                else
                {  //AddingRoletoUser
                    await _usermanager.AddToRoleAsync(user, role);
                    return StatusCode(StatusCodes.Status201Created, new Response { status = "Success", message = "User Created Successfully!" });


                }
               

            }
            else
            {
                return StatusCode(StatusCodes.Status403Forbidden, new Response { status = "Error", message = "This Role Does not Exist!" });

            }




        }

        [HttpGet]
        public IActionResult TestEmail()
        {
            var message = new Message(new string[] { "zubairfaheem1503@gmail.com" }, "Testing", "<h1> Hello Perky Rabbit Recruiters! </h1>");

            _emailService.SendEmail(message);
            return StatusCode(StatusCodes.Status200OK, new Response { status = "Success", message = "Email Sent Successfully" });
        }


    }
}
