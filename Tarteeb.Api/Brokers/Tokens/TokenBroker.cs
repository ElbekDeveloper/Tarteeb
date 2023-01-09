using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tarteeb.Api.Models;
using Tarteeb.Api.Services.Orchestrations.Model;

namespace Tarteeb.Api.Brokers.Tokens
{
    public class TokenBroker
    {
        private readonly IOptions<AppSettings> appSettings;

        public TokenBroker(IOptions<AppSettings> appSettings)=>
            this.appSettings = appSettings;

        public string GenerateJWT(User user)
        {
           var securityTokenHandler= new JwtSecurityTokenHandler();

           var claims =new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Hash,user.Password)
            };

            var key = Encoding.ASCII.GetBytes(appSettings.Value.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature),
            };
            var securityToken=securityTokenHandler.CreateToken(tokenDescriptor);
            return securityTokenHandler.WriteToken(securityToken);

        }
    }

}
