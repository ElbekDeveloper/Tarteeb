//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

<<<<<<< HEAD
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
=======
using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
>>>>>>> a0e20c346d83c7b0d867c9109069ab1e196ede0e
using Tarteeb.Api.Models.Tickets;
using Tarteeb.Api.Models.Tickets.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Tickets
{
    public partial class TicketServiceTests
    {
        [Fact]
<<<<<<< HEAD
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsyn()
        {
            // given
            Guid invalidTicketId = Guid.Empty;

            var invalidTicketException =
                new InvalidTicketException();

            invalidTicketException.AddData(
                key: nameof(Ticket.Id),
                values: "Id is required");

            var expectedTicketValidationException =
                new TicketValidationException(invalidTicketException);

            //when
            ValueTask<Ticket> removeTicketByIdTask =
                this.ticketService.RemoveTicketByIdAsync(invalidTicketId);
=======
        public async Task ShouldThrowNotFoundExceptionOnRemoveIfTicketIsNotFoundAndLogItAsync()
        {
            //given
            Guid randomTicketId = Guid.NewGuid();
            Guid inputTicketId = randomTicketId;
            Ticket noTicket = null;

            var notFoundTicketException =
                new NotFoundTicketException(inputTicketId);

            var expectedTicketValidationException =
                new TicketValidationException(notFoundTicketException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTicketByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noTicket);

            //when
            ValueTask<Ticket> removeTicketByIdTask =
                this.ticketService.RemoveTicketByIdAsync(inputTicketId);
>>>>>>> a0e20c346d83c7b0d867c9109069ab1e196ede0e

            TicketValidationException actualTicketValidationException =
                await Assert.ThrowsAsync<TicketValidationException>(
                    removeTicketByIdTask.AsTask);

            //then
            actualTicketValidationException.Should().BeEquivalentTo(expectedTicketValidationException);

<<<<<<< HEAD
            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs
                    (expectedTicketValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTicketAsync(It.IsAny<Ticket>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
=======
            this.storageBrokerMock.Verify(broker =>
                broker.SelectTicketByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTicketValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
              broker.DeleteTicketAsync(It.IsAny<Ticket>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
>>>>>>> a0e20c346d83c7b0d867c9109069ab1e196ede0e
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
