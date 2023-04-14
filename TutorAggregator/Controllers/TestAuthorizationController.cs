using DAL.Core;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestAuthorizationController : Controller
    {
        private readonly DataContext _database;
        private IAccountService _accountService;

        public TestAuthorizationController(DataContext data, IAccountService accountService)
        {
            _database = data;
            _accountService = accountService;
        }

        [Authorize(Roles = "Student")]
        [HttpGet("Students")]
        public async Task<ActionResult> GetStudent()
        {
            return Ok(_database.Students.ToList());
        }

        [Authorize(Roles = "Tutor")]
        [HttpGet("Tutors")]
        public async Task<ActionResult> GetTutors()
        {
            return Ok(_database.Tutors.ToList());
        }
    }
}
