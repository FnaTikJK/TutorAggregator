using System.Text;
using TutorAggregator.DataEntities;
using TutorAggregator.ServiceInterfaces;
using TutorAggregator.Models;
using TutorAggregator.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace TutorAggregator.Services
{
  
    public class TreeSearchService : ISearchService
    {
        private readonly DataContext _database;
        public TreeSearchService(DataContext database)
        {
            _database = database;
        }

        public async IAsyncEnumerable<TutorSearchDTO>? Search(string? prefix, TutorSearchFiltersDTO? filtersDTO)
        {
            var tutorsInDB = await _database.Tutors
                .Include(t => t.LessonTemplates)
                .ToArrayAsync();
            var initialisedTutors = tutorsInDB
                .Where(t => t.FirstName != null && (prefix == null || FormTutorFullName(t).StartsWith(prefix,true,null))
                && t.LessonTemplates != null && t.LessonTemplates.Count > 0);
            foreach (var tutor in initialisedTutors)
            {
                var tutorDTO = new TutorSearchDTO(tutor);
                if (filtersDTO == null)
                    yield return tutorDTO;
                else if (SatisfiesAllFilters(tutorDTO, filtersDTO))
                    yield return tutorDTO;
            }
           
        }
        private bool SatisfiesAllFilters(TutorSearchDTO tutorDTO, TutorSearchFiltersDTO filtersDTO)
        {
            var templates = _database.LessonTemplates.Include(e => e.Lessons)
                 .Include(e => e.Tutor)
                 .Where(e => e.Tutor.Id == tutorDTO.Id);
            return templates
                .Any(e => (e.Name == filtersDTO.SubjectName || filtersDTO.SubjectName == null)
                && (e.LessonFormat == filtersDTO.LessonFormat || filtersDTO.LessonFormat == null)
                && (e.DesiredSex == filtersDTO.DesiredSex || filtersDTO.DesiredSex == DesiredSex.All || filtersDTO.DesiredSex == null)
                && (e.DesiredAge == filtersDTO.DesiredAge || filtersDTO.DesiredAge == DesiredAge.All || filtersDTO.DesiredAge == null)
                && (filtersDTO.Place == null || e.Lessons.Any(l => l.Place == filtersDTO.Place)));
        }
        private string FormTutorFullName(Tutor tutor) => $"{tutor.SecondName} {tutor.FirstName} {tutor.ThirdName}".Trim();
    }
}
