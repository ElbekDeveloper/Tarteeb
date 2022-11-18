//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//===============================

using System;
using Moq;
using Tarteeb.Api.Brokers.Storages;
using Tarteeb.Api.Models.Teams;
using Tarteeb.Api.Services.Foundations.Teams;
using Tynamix.ObjectFiller;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Teams
{
    public partial class TeamServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly ITeamService teamService;

        public TeamServiceTests() 
        {
            this.storageBrokerMock= new Mock<IStorageBroker>();

            this.teamService = new TeamService(
                storageBroker: this.storageBrokerMock.Object);
        }

        private static DateTimeOffset CreateRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private static Team CreateRandomTeam()=>
            CreateTeamFiller(date: CreateRandomDateTimeOffset()).Create();

        private static Filler<Team> CreateTeamFiller(DateTimeOffset date)
        {
            var filler = new Filler<Team>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(date);

            return filler;
        }
    }
}
