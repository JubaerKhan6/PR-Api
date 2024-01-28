using Microsoft.AspNetCore.Mvc;
using UserManagementService.Models;
using UserManagementService.Models.Authentication.Login;
using UserManagementService.Models.Authentication.Signup;

namespace UserManagementService.Services.AuthenticateRepository
{
    public interface IAuthenticationService
    {
        Task<ApiFeedback<int>> RegisterUser(RegisterUser registerUser);
        Task<ApiFeedback<int>> LoginUser(LoginModel loginModel);
        Task<OtpResponse<int>> OTPVerification(string code, string username);
    }
}
