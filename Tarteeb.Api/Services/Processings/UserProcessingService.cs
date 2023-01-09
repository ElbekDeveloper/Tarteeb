using System.Linq;
using Tarteeb.Api.Models;
using Tarteeb.Api.Services.Foundations.Users;

namespace Tarteeb.Api.Services.Processings
{
    public class UserProcessingService
    {
        private readonly IUserService userService;

        public UserProcessingService(IUserService userService)=>
            this.userService = userService;


        //barcha uselarni olib ichidan filtr qiladi (email va password bilan)
        public User RetriveUserByCredentials(string email, string password)
        {
            IQueryable<User> allUser=this.userService.RetrieveAllUsers();
          
            User validUserOrNot=allUser.
                Where(retrievedUser => retrievedUser.Email == email
                    && retrievedUser.Password == password).FirstOrDefault();

            return validUserOrNot;
        }
    }
}
