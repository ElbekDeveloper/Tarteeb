﻿//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Linq;
using Tarteeb.Api.Models;
using Tarteeb.Api.Models.Users.Exceptions;
using Xeptions;

namespace Tarteeb.Api.Services.Foundations.Users
{
    public partial class UserService
    {
        private delegate ValueTask<User> ReturningUserFunction();
        private delegate IQueryable<User> ReturningUsersFunction();

        private async ValueTask<User> TryCatch(ReturningUserFunction returningUserFunction)
        private IQueryable<User> TryCatch(ReturningUsersFunction returningUsersFunction)
        {
            try
            {
                return await returningUserFunction();
                return returningUsersFunction();
            }
            catch (NullUserException nullUserException)
            {
                throw CreateAndLogValidationException(nullUserException);
            }
            catch (InvalidUserException invalidUserException)
            {
                throw CreateAndLogValidationException(invalidUserException);
            }
            catch (SqlException sqlException)
            {
                var failedUserStorageException =
                    new FailedUserStorageException(sqlException);
                var failedUserStorageException = new FailedUserStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedUserStorageException);
            }
            catch (Exception exception)
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsUserException =
                    new AlreadyExistsUserException(duplicateKeyException);

                throw CreateAndDependencyValidationException(alreadyExistsUserException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedUserException = new LockedUserException(dbUpdateConcurrencyException);

                throw CreateAndDependencyValidationException(lockedUserException);
            }
            catch (Exception serviceException)
            {
                var failedUserServiceException =
                    new FailedUserServiceException(exception);
                var failedUserServiceException = new FailedUserServiceException(serviceException);

                throw CreateAndServiceException(failedUserServiceException);
                throw CreateAndLogServiceException(failedUserServiceException);
            }
        }

        private UserDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        private UserDependencyException CreateAndLogCriticalDependencyException(Xeption exeption)
        {
            var userDependencyException = new UserDependencyException(exception);
            var userDependencyException = new UserDependencyException(exeption);
            this.loggingBroker.LogCritical(userDependencyException);

            return userDependencyException;
        }

        private UserServiceException CreateAndLogServiceException(Xeption exception)
        private UserValidationException CreateAndLogValidationException(Xeption exception)
        {
            var userValidationException =
                new UserValidationException(exception);

            this.loggingBroker.LogError(userValidationException);

            return userValidationException;
        }

        private UserDependencyValidationException CreateAndDependencyValidationException(Xeption exception)
        {
            var userServiceException =
                new UserServiceException(exception);
            var userDependencyValidationException =
                new UserDependencyValidationException(exception);

            this.loggingBroker.LogError(userDependencyValidationException);

            return userDependencyValidationException;
        }

        private UserServiceException CreateAndServiceException(Xeption exception)
        {
            var userServiceException = new UserServiceException(exception);
            this.loggingBroker.LogError(userServiceException);

            return userServiceException;
        }
    }
}