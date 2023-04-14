using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class AccountControllerTests
    {
        [Test]
        public async Task Register_ReturnsToken()
        {
            await using var app = new WebApplicationFactory<Program>();
            var client = app.CreateClient();

            var response = await client.GetAsync($"/LessonTemplates/{1}");
            var txt = await response.Content.ReadAsStringAsync();
            var context = new {login = "t1", password = "s"};
            var resp2 = await client.PostAsync("/Accounts/Authenticate", n);
            var txt2 = await resp2.Content.ReadAsStringAsync();
        }
    }
}