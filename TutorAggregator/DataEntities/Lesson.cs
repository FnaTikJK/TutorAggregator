using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace TutorAggregator.DataEntities
{
    public class Lesson
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        [MinLength(1)]
        public string Place { get; set; }
        public int LengthInMinutes { get; set; }
        public Student? Student { get; set; }
        public int? ChosenTemplateId { get; set; }
        public List<LessonTemplate> AllowedTemplates { get; set; }
    }
}
