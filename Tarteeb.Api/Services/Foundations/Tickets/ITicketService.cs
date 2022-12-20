//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Linq;
using System.Threading.Tasks;
using Tarteeb.Api.Models.Tickets;

namespace Tarteeb.Api.Services.Foundations.Tickets
{
    public interface ITicketService
    {
        ValueTask<Ticket> AddTicketAsync(Ticket ticket);
        IQueryable<Ticket> RetrieveAllTickets();
        ValueTask<Ticket> RetrieveTicketByIdAsync(Guid ticketId);
        ValueTask<Ticket> RemoveTicketByIdAsync(Guid ticketId);
    }
}