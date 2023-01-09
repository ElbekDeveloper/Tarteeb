using Tarteeb.Api.Models;
using Tarteeb.Api.Services.Foundations;

namespace Tarteeb.Api.Services.Processings
{
    public class UserSecurityService
    {
        private readonly SecurityService securityService;

        public UserSecurityService(SecurityService securityService)=>
            this.securityService = securityService;
        public string CreateToken(User user)=>
            securityService.CreateToken(user);

    }
}
