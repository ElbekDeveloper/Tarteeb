//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Linq;
using System.Threading.Tasks;
using Tarteeb.Api.Brokers.DateTimes;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Brokers.Storages;
using Tarteeb.Api.Models.Tickets;

namespace Tarteeb.Api.Services.Foundations.Tickets
{
    public partial class TicketService : ITicketService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public TicketService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<Ticket> AddTicketAsync(Ticket ticket) =>
        TryCatch(async () =>
        {
            ValidateTicket(ticket);

            return await this.storageBroker.InsertTicketAsync(ticket);
        });

        public IQueryable<Ticket> RetrieveAllTickets() =>
        TryCatch(() => this.storageBroker.SelectAllTickets());

        public ValueTask<Ticket> RetrieveTicketByIdAsync(Guid ticketId) =>
        TryCatch(async () =>
        {
            ValidateTicketId(ticketId);

            Ticket maybeTicket =
                await this.storageBroker.SelectTicketByIdAsync(ticketId);

            ValidateStorageTicket(maybeTicket, ticketId);

            return maybeTicket;
        });

        public ValueTask<Ticket> RemoveTicketByIdAsync(Guid ticketId) =>
            TryCatch(async () =>
            {
                Ticket maybeTicket = await this.storageBroker
                    .SelectTicketByIdAsync(ticketId);

                return await this.storageBroker
                    .DeleteTicketAsync(maybeTicket);
            });
    }
}