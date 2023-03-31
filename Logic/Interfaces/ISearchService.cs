using Logic.Models;

namespace Logic.Interfaces
{
    public interface ISearchService
    {
        public Task<SearchResponse> Search(string? prefix, TutorSearchFiltersDTO? filtersDTO,
            int elementsPerPage, int pageNumber);
    }
}
