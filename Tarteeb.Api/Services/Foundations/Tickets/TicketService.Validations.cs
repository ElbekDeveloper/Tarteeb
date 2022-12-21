//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Data;
using Tarteeb.Api.Models.Tickets;
using Tarteeb.Api.Models.Tickets.Exceptions;

namespace Tarteeb.Api.Services.Foundations.Tickets
{
    public partial class TicketService
    {
        private void ValidateTicket(Ticket ticket)
        {
            ValidateTicketNotNull(ticket);

            Validate(
                (Rule: IsInvalid(ticket.Id), Parameter: nameof(Ticket.Id)),
                (Rule: IsInvalid(ticket.Title), Parameter: nameof(Ticket.Title)),
                (Rule: IsInvalid(ticket.Priority), Parameter: nameof(Ticket.Priority)),
                (Rule: IsInvalid(ticket.Deadline), Parameter: nameof(Ticket.Deadline)),
                (Rule: IsInvalid(ticket.Status), Parameter: nameof(Ticket.Status)),
                (Rule: IsInvalid(ticket.CreatedDate), Parameter: nameof(Ticket.CreatedDate)),
                (Rule: IsInvalid(ticket.UpdatedDate), Parameter: nameof(Ticket.UpdatedDate)),
                (Rule: IsInvalid(ticket.CreatedUserId), Parameter: nameof(Ticket.CreatedUserId)),
                (Rule: IsInvalid(ticket.UpdatedUserId), Parameter: nameof(Ticket.UpdatedUserId)),
                (Rule: IsNotRecent(ticket.CreatedDate), Parameter: nameof(Ticket.CreatedDate)),

                (Rule: IsNotSame(
                    firstDate: ticket.CreatedDate,
                    secondDate: ticket.UpdatedDate,
                    secondDateName: nameof(Ticket.UpdatedDate)),

                Parameter: nameof(Ticket.CreatedDate)));
        }

        private void ValidateTicketId(Guid ticketId) =>
            Validate((Rule: IsInvalid(ticketId), Parameter: nameof(Ticket.Id)));

        private static void ValidateStrogeTicket(Ticket maybeTicket, Guid ticketId)
        {
            if(maybeTicket is null)
            {
                throw new NotFoundTicketByIdException(ticketId);
            }
        }

        private void ValidateStorageTicket(Ticket maybeTicket, Guid ticketId)
        {
            if(maybeTicket is null)
            {
                throw new NotFoundTicketException(ticketId);
            }
        }
 
        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == default,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Value is required"
        };

        private static dynamic IsInvalid<T>(T value) => new
        {
            Condition = IsEnumInvalid(value),
            Message = "Value is not recognized"
        };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not same as {secondDateName}"
            };

        private static bool IsEnumInvalid<T>(T value)
        {
            bool isDefined = Enum.IsDefined(typeof(T), value);

            return isDefined is false;
        }

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime = this.dateTimeBroker.GetCurrentDateTime();
            TimeSpan timeDifference = currentDateTime.Subtract(date);

            return timeDifference.TotalSeconds is > 60 or < 0;
        }

        private static void ValidateTicketNotNull(Ticket ticket)
        {
            if (ticket is null)
            {
                throw new NullTicketException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidTicketException = new InvalidTicketException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidTicketException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidTicketException.ThrowIfContainsErrors();
        }
    }
}
