//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Tarteeb.Api.Models;

namespace Tarteeb.Api.Brokers.Tokens
{
    public class TokenBroker : ITokenBroker
    {
        private readonly IConfiguration Configuration;

        public TokenBroker(IConfiguration configuration) =>
            this.Configuration = configuration;

        public string GenerateJWT(User user)
        {
            var convertedKeyToBytes = 
                Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]);

            var securityKey = 
                new SymmetricSecurityKey(convertedKeyToBytes);

            var cridentials =
                new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var token = new JwtSecurityToken(
                Configuration["Jwt:Issuer"],
                Configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(6),
                signingCredentials: cridentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
