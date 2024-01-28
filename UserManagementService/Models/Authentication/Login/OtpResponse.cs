using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementService.Models.Authentication.Login
{
    public class OtpResponse<T>
    {
        [Required(ErrorMessage = "Username is Required!")]
        public string? userame { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Email is Required!")]
        public string? email { get; set; }
        [Required(ErrorMessage = "Password is Required!")]
        
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public int StatusCode { get; set; }
        public T? Response { get; set; }
    }
}
