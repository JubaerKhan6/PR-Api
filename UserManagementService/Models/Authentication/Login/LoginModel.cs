using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace UserManagementService.Models.Authentication.Login
{
    public class LoginModel
    {
        [Required(ErrorMessage ="Username is Required!")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Password is Required!")]
        public string? Password { get; set; }
    }
}
