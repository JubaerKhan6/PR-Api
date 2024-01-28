using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementService.Models;
using UserManagementService.Services.AuthenticateRepository;
using UserManagementService.Services.EmailRepository;

namespace UserManagementService.Services
{
    public class UnitofWork :IUnitofWork
    {
        private readonly UserManager<IdentityUser> _usermanager;
        private readonly RoleManager<IdentityRole> _rolemanager;
        private readonly SignInManager<IdentityUser> _signinManager;
        private readonly ServiceDbContext _context;
        private readonly EmailConfiguration _emailconfiguration;
        private readonly IMailService _emailService;
        private IMailService _mailservice;
        private IAuthenticationService _authenticationservice;
        public UnitofWork(UserManager<IdentityUser> usermanager, RoleManager<IdentityRole> rolemanager, SignInManager<IdentityUser> signinManager, IMailService emailService, ServiceDbContext context, IAuthenticationService authenticationservice)
        {
            _usermanager = usermanager;
            _rolemanager = rolemanager;
            _emailService = emailService;
            _signinManager = signinManager;
            _context = context;
            _authenticationservice = authenticationservice;
        }
        public IAuthenticationService AuthenticationService => new AuthenticationService(_usermanager,_rolemanager,_signinManager,_emailService, _context);
        public IMailService MailService => new EmailService(_emailconfiguration);


    }
}
