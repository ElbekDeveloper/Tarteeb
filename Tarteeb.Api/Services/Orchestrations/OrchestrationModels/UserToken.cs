using System;

namespace Tarteeb.Api.Services.Orchestrations.Model
{
    public class UserToken
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
    }
}
