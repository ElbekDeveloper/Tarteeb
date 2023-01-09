using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using System.Threading.Tasks;
using Tarteeb.Api.Services.Orchestrations;

namespace Tarteeb.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : RESTFulController
    {
        private readonly UserOrchestrationService _userOrchestrationService;
        public AccountsController(UserOrchestrationService userOrchestrationService)=>
            this._userOrchestrationService= userOrchestrationService;

        [HttpGet("login")]
        public  IActionResult Login(string email,string password)
        {
            var result=_userOrchestrationService.LoginUser(email, password);

            return Ok(result);
        }
    }
}
