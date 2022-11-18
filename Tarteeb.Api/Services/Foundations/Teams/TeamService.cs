//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using Tarteeb.Api.Brokers.Storages;
using Tarteeb.Api.Models.Teams;

namespace Tarteeb.Api.Services.Foundations.Teams
{
    public class TeamService : ITeamService
    {
        private readonly IStorageBroker storageBroker;

        public TeamService(IStorageBroker storageBroker) =>
            this.storageBroker = storageBroker;

        public ValueTask<Team> AddTeamAsync(Team team) =>
            throw new System.NotImplementedException();
       
    }
}
