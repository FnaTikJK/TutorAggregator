using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResultOfTask;
using TutorAggregator.Data;
using TutorAggregator.DataEntities;
using TutorAggregator.Models;
using TutorAggregator.ServiceInterfaces;

namespace TutorAggregator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> RegisterAccount(AccountRegDTO accountRegDTO)
        {
            Result<string> response;
            if (accountRegDTO.Role == "Student")
                response = (await _accountService.Register<Student>(accountRegDTO))!;
            else if (accountRegDTO.Role == "Tutor")
                response = (await _accountService.Register<Tutor>(accountRegDTO))!;
            else
                return Unauthorized("Incorrect Role");

            return response.IsSuccess ? 
                Ok(response.Value)
                : Unauthorized(response.Error);
        }

        [HttpGet("Authenticate")]
        public async Task<ActionResult> AuthenticateAccount([FromQuery] AccountAuthDTO accountAuthDTO)
        {
            Result<string> response;
            response = await _accountService.Authenticate(accountAuthDTO);

            return response.IsSuccess ?
                Ok(response.Value)
                : Unauthorized(response.Error);
        }

        [HttpPut("ChangePassword")]
        public async Task<ActionResult> ChangePassword(AccountChangePasswordDTO accountChangePasswordDTO)
        {
            Result<string> response ;
            response = await _accountService.ChangePassword(accountChangePasswordDTO);

            return response.IsSuccess ?
                Ok(response.Value)
                : BadRequest(response.Error);
        }
    }
}
