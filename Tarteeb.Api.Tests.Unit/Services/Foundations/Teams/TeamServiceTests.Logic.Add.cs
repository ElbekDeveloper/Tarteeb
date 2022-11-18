//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//===============================

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Tarteeb.Api.Models.Teams;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Teams
{
    public partial class TeamServiceTests
    {
        [Fact]
        public async Task ShouldAddTeamAsync()
        {
            //given
            Team randomTeam = CreateRandomTeam();
            Team inputTeam = randomTeam;
            Team persistedTeam = inputTeam;
            Team expectedTeam = persistedTeam.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertTeamAsync(inputTeam))
                 .ReturnsAsync(persistedTeam);

            //when
            Team actualTeam = await this.teamService.AddTeamAsync(inputTeam);

            //then
            actualTeam.Should().BeEquivalentTo(expectedTeam);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTeamAsync(inputTeam), Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
