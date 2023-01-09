using Tarteeb.Api.Models;
using Tarteeb.Api.Services.Orchestrations.Model;
using Tarteeb.Api.Services.Processings;

namespace Tarteeb.Api.Services.Orchestrations
{
    public class UserOrchestrationService
    {
        private readonly UserProcessingService userProcessingService;
        private readonly UserSecurityService userSecurityService;

        public UserOrchestrationService(
            UserProcessingService userProcessingService,
            UserSecurityService userSecurityService)
        {
            this.userProcessingService =
                userProcessingService;

            this.userSecurityService = 
                userSecurityService;
        }
        public UserToken LoginUser(string email, string password)
        {
            User user =
                userProcessingService.RetriveUserByCredentials(email, password);

            UserToken userToken = new UserToken();
            userToken.UserId = user.Id;
            userToken.Token = userSecurityService.CreateToken(user);

            return userToken;
        }
    }
}
