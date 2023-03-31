using DAL.Entities;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ResultOfTask;
using Logic.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ControllerBase
    {
        private IAccountService _accountService;

        public AccountsController(IAccountService accountService)
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

        [HttpPost("Authenticate")]
        public async Task<ActionResult> AuthenticateAccount([FromBody] AccountAuthDTO accountAuthDTO)
        {
            Result<string> response;
            response = await _accountService.Authenticate(accountAuthDTO);

            return response.IsSuccess ?
                Ok(response.Value)
                : Unauthorized(response.Error);
        }

        [HttpPost("ChangePassword")]
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
