//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Tarteeb.Api.Models.Tickets;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Tickets
{
    public partial class TicketServiceTests
    {
        [Fact]
        public async Task ShouldRemoveTicketByIdAsync()
        {
            //given
            Guid randomId = Guid.NewGuid();
            Guid inputTicketId = randomId;
            Ticket randomTicket = CreateRandomTicket();
            Ticket storageTicket = randomTicket;
            Ticket expectedInputTicket = storageTicket;
            Ticket deletedTicket = expectedInputTicket;
            Ticket expectedTicket = deletedTicket.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTicketByIdAsync(inputTicketId))
                    .ReturnsAsync(storageTicket);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteTicketAsync(expectedInputTicket))
                    .ReturnsAsync(deletedTicket);

            //when
            Ticket actualTicket = await this.ticketService
                .RemoveTicketByIdAsync(inputTicketId);

            //then
            actualTicket.Should().BeEquivalentTo(expectedTicket);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTicketByIdAsync(inputTicketId),
                    Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTicketAsync(expectedInputTicket),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}