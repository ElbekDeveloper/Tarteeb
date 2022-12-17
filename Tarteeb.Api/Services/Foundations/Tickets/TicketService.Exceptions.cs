//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Tarteeb.Api.Models.Tickets;
using Tarteeb.Api.Models.Tickets.Exceptions;
using Xeptions;

namespace Tarteeb.Api.Services.Foundations.Tickets
{
    public partial class TicketService
    {
        private delegate IQueryable<Ticket> ReturningTicketsFunction();
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
            catch(NotFoundTicketException notFoundTicketException)
            {
                throw CreateAndLogValidationException(notFoundTicketException);
            }
            catch (SqlException sqlException)
            {
                var failedTicketStorageException = new FailedTicketStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedTicketStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var failedTicketDependencyValidationException =
                     new AlreadyExistsTicketException(duplicateKeyException);

                throw CreateAndDependencyValidationException(failedTicketDependencyValidationException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedTickedException = new LockedTicketException(dbUpdateConcurrencyException);

                throw CreateAndDependencyValidationException(lockedTickedException);
            }
            catch (Exception serviceException)
            {
                var failedServiceProfileException = new FailedTicketServiceException(serviceException);

                throw CreateAndLogServiceException(failedServiceProfileException);
            }
        }

        private IQueryable<Ticket> TryCatch(ReturningTicketsFunction returningTicketsFunction)
        {
            try
            {
                return returningTicketsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedTicketServiceException = new FailedTicketServiceException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedTicketServiceException);
            }
            catch (Exception serviException)
            {
                var failedServiceTicketException = new FailedTicketServiceException(serviException);

                throw CreateAndLogServiceException(failedServiceTicketException);
            }
        }

        private TicketServiceException CreateAndLogServiceException(Xeption exception)
        {
            var ticketServiceException = new TicketServiceException(exception);
            this.loggingBroker.LogError(ticketServiceException);

            return ticketServiceException;
        }

        private TicketValidationException CreateAndLogValidationException(Xeption exception)
        {
            var ticketValidationException = new TicketValidationException(exception);
            this.loggingBroker.LogError(ticketValidationException);

            return ticketValidationException;
        }

        private TicketDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var ticketDependencyException = new TicketDependencyException(exception);
            this.loggingBroker.LogCritical(ticketDependencyException);

            return ticketDependencyException;
        }

        private TicketDependencyValidationException CreateAndDependencyValidationException(Xeption exception)
        {
            var ticketDependencyValidationException = new TicketDependencyValidationException(exception);
            this.loggingBroker.LogError(ticketDependencyValidationException);

            return ticketDependencyValidationException;
        }
    }
}