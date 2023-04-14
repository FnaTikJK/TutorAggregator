using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using DAL.Entities;
using Logic.Models;
using Logic.Interfaces;
using DAL.Core;
using DAL.Entities.Enums;

namespace Logic.Services
{

    public class TreeSearchService : ISearchService
    {
        private readonly DataContext _database;
        public TreeSearchService(DataContext database)
        {
            _database = database;
        }

        public async Task<SearchResponse> Search(string? prefix,
            TutorSearchFiltersDTO? filtersDTO, int elementsPerPage, int pageNumber)
        {
            var tutorsInDB = await _database.Tutors
                .Include(t => t.LessonTemplates)
                .ToArrayAsync();
            var initialisedTutors = tutorsInDB
                .Where(t => t.FirstName != null && (prefix == null || FormTutorFullName(t).StartsWith(prefix, true, null))
                /*&& t.LessonTemplates != null && t.LessonTemplates.Count > 0*/);
            var allSatisfyingTutorsList = new List<TutorSearchDTO>();
            foreach (var tutor in initialisedTutors)
            {
                var tutorDTO = new TutorSearchDTO(tutor);
                /*if (filtersDTO == null || SatisfiesAllFilters(tutorDTO, filtersDTO))*/
                    allSatisfyingTutorsList.Add(tutorDTO);
            }
            if (allSatisfyingTutorsList.Count == 0)
                return new SearchResponse();
            if (elementsPerPage == 0 || pageNumber == 0 || elementsPerPage < 0 || pageNumber < 0)
                return null!;
            var pageCount = (int)Math.Ceiling(allSatisfyingTutorsList.Count / (double)elementsPerPage);
            if (pageCount == 0 || pageCount < pageNumber)
                return null!;
            var pageTutorsList = new List<TutorSearchDTO>();
            for (int i = elementsPerPage * (pageNumber - 1); i < elementsPerPage * pageNumber && i < allSatisfyingTutorsList.Count; i++)
                pageTutorsList.Add(allSatisfyingTutorsList[i]);
            return new SearchResponse(pageTutorsList, pageNumber, pageCount);
        }
        private bool SatisfiesAllFilters(TutorSearchDTO tutorDTO, TutorSearchFiltersDTO filtersDTO)
        {
            var templates = _database.LessonTemplates.Include(e => e.Lessons)
                 .Include(e => e.Tutor)
                 .Where(e => e.Tutor.Id == tutorDTO.Id);
            return templates
                .Any(e => (e.Name == filtersDTO.SubjectName || filtersDTO.SubjectName == null)
                && (e.LessonFormat == filtersDTO.LessonFormat || filtersDTO.LessonFormat == null)
                && (e.DesiredSex == filtersDTO.DesiredSex || filtersDTO.DesiredSex == Sex.Any || filtersDTO.DesiredSex == null)
                && (e.DesiredAge == filtersDTO.DesiredAge || filtersDTO.DesiredAge == AgeGroup.Any || filtersDTO.DesiredAge == null)
                && (filtersDTO.Place == null || e.Lessons.Any(l => l.Place == filtersDTO.Place)));
        }
        private string FormTutorFullName(Tutor tutor) => $"{tutor.SecondName} {tutor.FirstName} {tutor.ThirdName}".Trim();
    }
}
