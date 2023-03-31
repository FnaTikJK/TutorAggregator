using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Logic.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Logic.Models
{
    public class SearchResponse
    {
        public SearchResponse(List<TutorSearchDTO> tutors, int currentPage, int pageCount)
        {
            Result = tutors;
            CurrentPage = currentPage;
            PageCount = pageCount;
        }

        public SearchResponse()
        {

        }
        public List<TutorSearchDTO> Result { get; set; }

        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
    }
}
