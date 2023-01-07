//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Moq;
using FluentAssertions;
using System.Threading.Tasks;
using Tarteeb.Api.Models;
using Tarteeb.Api.Models.Users.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUserIsNullAndLogItAsync()
        {
            //given
            User nullUser = null;
            var nullUserException = new NullUserException();

            var expectedUserValidationException =
                new UserValidationException(nullUserException);

            //when
            ValueTask<User> modifyUserTask =
                this.userService.ModifyUserAsync(nullUser);

            UserValidationException actualUserValidationException =
                await Assert.ThrowsAsync<UserValidationException>(modifyUserTask.AsTask);

            //then
            actualUserValidationException.Should().BeEquivalentTo(expectedUserValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))), Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateUserAsync(It.IsAny<User>()), Times.Never());

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfUserIsInvalidAndLogItAsync(string invalidText)
        {
            //given
            var invalidUser = new User
            {
                FirstName = invalidText
            };

            var invalidUserException = new InvalidUserException();

            invalidUserException.AddData(
                key: nameof(User.Id),
                values: "Id is required");

            invalidUserException.AddData(
                key: nameof(User.FirstName),
                values: "Text is required");

            invalidUserException.AddData(
                key: nameof(User.LastName),
                values: "Text is required");

            invalidUserException.AddData(
                key: nameof(User.Email),
                values: "Text is required");

            invalidUserException.AddData(
                key: nameof(User.BirthDate),
                values: "Value is required");

            invalidUserException.AddData(
                key: nameof(User.CreatedDate),
                values: "Value is required");

            invalidUserException.AddData(
                key: nameof(User.UpdatedDate),
                values: "Value is required");

            invalidUserException.AddData(
               key: nameof(User.Password),
               values: "Text is required");

            var expectedUserValidationException = new UserValidationException(
                invalidUserException);

            //when
            ValueTask<User> modifyUserTask = this.userService.ModifyUserAsync(invalidUser);

            UserValidationException actualUserValidationException =
                await Assert.ThrowsAsync<UserValidationException>(modifyUserTask.AsTask);

            //then
            actualUserValidationException.Should().BeEquivalentTo(expectedUserValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertUserAsync(It.IsAny<User>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdateDateIsSameAsCreateDateAndLogItAsync()
        {
            //given
            DateTimeOffset randomDatetime = GetRandomDateTimeOffset();
            User randomUser = CreateRandomUser(randomDatetime);
            User invalidUser = randomUser;
            var invalidUserException =new InvalidUserException();

            invalidUserException.AddData(
                key: nameof(User.UpdatedDate),
                values: $"Date is same as {nameof(User.CreatedDate)}");

            var expectedUserValidationException =
                new UserValidationException(invalidUserException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDatetime);

            //when
            ValueTask<User> modifyUserTask =
                this.userService.ModifyUserAsync(invalidUser);

            UserValidationException actualUserValidationException =
                await Assert.ThrowsAsync<UserValidationException>(modifyUserTask.AsTask);

            //then
            actualUserValidationException.Should().BeEquivalentTo(expectedUserValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                   expectedUserValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(invalidUser.Id), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutsBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(int minuts)
        {
            //given
            DateTimeOffset dateTime = GetRandomDateTimeOffset();
            User randomUser = CreateRandomUser(dateTime);
            User inputUser = randomUser;
            inputUser.UpdatedDate = dateTime.AddMinutes(minuts);
            var invalidUserException = new InvalidUserException();

            invalidUserException.AddData(
                key: nameof(User.UpdatedDate),
                values: "Date is not recent");

            var expectedUserValidationException =
                new UserValidationException(invalidUserException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(dateTime);

            //when
            ValueTask<User> modifyUserTask =
                this.userService.ModifyUserAsync(inputUser);

            UserValidationException actualUserValidationException =
                await Assert.ThrowsAsync<UserValidationException>(modifyUserTask.AsTask);

            //then
            actualUserValidationException.Should().BeEquivalentTo(expectedUserValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                   expectedUserValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUserDoesNotExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetRandomNegativeNumber();
            DateTimeOffset dateTime = GetRandomDateTimeOffset();
            User randomUser = CreateRandomModifyUser(dateTime);
            User nonExistUser = randomUser;
            nonExistUser.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            User nullUser = null;

            var notFoundUserException =
                new NotFoundUserException(nonExistUser.Id);

            var expectedUserValidationException =
                new UserValidationException(notFoundUserException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(nonExistUser.Id))
                    .ReturnsAsync(nullUser);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(dateTime);

            // when
            ValueTask<User> modifyUserTask =
                this.userService.ModifyUserAsync(nonExistUser);

            UserValidationException actualUserValidationException =
                await Assert.ThrowsAsync<UserValidationException>(modifyUserTask.AsTask);

            // then
            actualUserValidationException.Should().BeEquivalentTo(expectedUserValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(nonExistUser.Id), Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
