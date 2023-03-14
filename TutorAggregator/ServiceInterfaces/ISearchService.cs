using TutorAggregator.Models;

namespace TutorAggregator.ServiceInterfaces
{
    public interface ISearchService
    {
        public IAsyncEnumerable<TutorSearchDTO>? Search(string? prefix, TutorSearchFiltersDTO? filtersDTO);
    }
}
