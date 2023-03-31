using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Logic.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : Controller
    {
        private readonly ISearchService _tree;
        public SearchController(ISearchService tree)
        {
            _tree = tree;
        }

        [HttpGet("GetItems")]
        public async Task<ActionResult<SearchResponse>> GetItemsBySubstring(
            string? prefix,
            [FromQuery] TutorSearchFiltersDTO filtersDTO,
            [FromQuery] int elementsPerPage,
            [FromQuery] int pageNumber)
        {
            var result = await _tree.Search(prefix!, filtersDTO!, elementsPerPage, pageNumber);
            if (result == null)
                return BadRequest("Ќеверное количество элементов на странице или несуществующа€ страница!");
            return result;
        }
    }
}
