using AutoMapper;
using DAL.Core;
using DAL.Entities;
using DAL.Interfaces;

namespace Logic.Helpers.Mapper.Config
{
    internal class LessonTemplateConverter : IValueConverter<List<int>, List<LessonTemplate>>, IValueConverter<List<LessonTemplate>, List<int>>
    {
        private readonly ILessonTemplatesRepository templatesRepository;

        public LessonTemplateConverter(ILessonTemplatesRepository templatesRepository, DataContext dataContext)
        {
            this.templatesRepository = templatesRepository;
        }

        public List<LessonTemplate> Convert(List<int> sourceMember, ResolutionContext context)
        {
            if (sourceMember == null)
                return null;

            var result = new List<LessonTemplate>(sourceMember.Count);
            foreach (var id in sourceMember)
            {
                var template = templatesRepository.GetById(id);
                if (template != null)
                    result.Add(template);
            }

            return result;
        }

        public List<int> Convert(List<LessonTemplate> sourceMember, ResolutionContext context)
        {
            return sourceMember.Select(e => e.Id).ToList();
        }
    }
}
