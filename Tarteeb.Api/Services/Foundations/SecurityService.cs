using Tarteeb.Api.Brokers.Tokens;
using Tarteeb.Api.Models;

namespace Tarteeb.Api.Services.Foundations
{
    public class SecurityService
    {
        private readonly TokenBroker tokenBroker;

        public SecurityService(TokenBroker tokenBroker)=>
            this.tokenBroker = tokenBroker;

        public string CreateToken(User user )=>
            tokenBroker.GenerateJWT(user);
    }
}
