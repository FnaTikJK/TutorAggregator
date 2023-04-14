using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Logic.Helpers;
using Logic.Models.Account;

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
            Result<string> response = await _accountService.RegisterAsync(accountRegDTO);

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
