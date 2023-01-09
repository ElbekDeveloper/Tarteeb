//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Tarteeb.Api.Models;

namespace Tarteeb.Api.Brokers.Tokens
{
    public interface ITokenBroker
    {
        string GenerateJWT(User user);
    }
}
