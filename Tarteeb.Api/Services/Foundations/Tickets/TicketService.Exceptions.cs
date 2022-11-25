﻿//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Tarteeb.Api.Models.Tickets;
using Tarteeb.Api.Models.Tickets.Exceptions;
using Xeptions;

namespace Tarteeb.Api.Services.Foundations.Tickets
{
    public partial class TicketService
    {
        private delegate ValueTask<Ticket> ReturningTicketFunction();

        private async ValueTask<Ticket> TryCatch(ReturningTicketFunction returningTicketFunction)
        {
            try
            {
                return await returningTicketFunction();
            }
            catch (NullTicketException nullTicketException)
            {
                throw CreateAndLogValidationException(nullTicketException);
            }
            catch (InvalidTicketException invalidTicketException)
            {
                throw CreateAndLogValidationException(invalidTicketException);
            }
            catch (SqlException sqlException)
            {
                var failedTicketStorageException = new FailedTicketStorageException(sqlException);

                CreateAndLogCriticalDependencyException(failedTicketStorageException);
            }
        }

        private TicketValidationException CreateAndLogValidationException(Xeption exception)
        {
            var TicketValidationException =
                new TicketValidationException(exception);

            this.loggingBroker.LogError(TicketValidationException);

            return TicketValidationException;
        }

        private TicketDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var ticketDependencyException = new TicketDependencyException(exception);
            this.loggingBroker.LogCritical(ticketDependencyException);

            return ticketDependencyException;
        }
    }
}