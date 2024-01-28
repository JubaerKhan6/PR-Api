using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementService.Models.Authentication.Login;

namespace UserManagementService.Models
{
    public class ServiceDbContext :IdentityDbContext<IdentityUser>
    {
        public ServiceDbContext(DbContextOptions<ServiceDbContext> options) : base(options) 
        {
                
        }
        public DbSet<OTP>? Otp { get; set; } 
    }
}
