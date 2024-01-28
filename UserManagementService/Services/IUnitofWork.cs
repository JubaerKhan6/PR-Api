using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementService.Services.AuthenticateRepository;
using UserManagementService.Services.EmailRepository;

namespace UserManagementService.Services
{
    public interface IUnitofWork
    {
        IMailService MailService { get; }
        IAuthenticationService AuthenticationService { get; }
    }
}
