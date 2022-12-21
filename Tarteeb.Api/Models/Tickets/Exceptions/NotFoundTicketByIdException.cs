//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Tickets.Exceptions
{
    public class NotFoundTicketByIdException : Xeption
    {
        public NotFoundTicketByIdException(Guid ticketId)
            : base(message: $"Couldn't find ticked with id: {ticketId}.")
        { }
    }
}
