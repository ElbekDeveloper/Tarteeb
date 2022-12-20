﻿//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Linq;
using FluentAssertions;
using Moq;
using Tarteeb.Api.Models.Teams;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Teams
{
    public partial class TeamServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllTeams()
        {
            //given
            IQueryable<Team> randomTeams = CreateRandomTeam();
            IQueryable<Team> storageTeams = randomTeams;
            IQueryable<Team> expectedTeams = storageTeams;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTeams()).Returns(storageTeams);

            //when
            IQueryable<Team> actualTeams = this.teamService.RetrieveAllTeams();

            //then
            actualTeams.Should().BeEquivalentTo(expectedTeams);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTeams(), Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}