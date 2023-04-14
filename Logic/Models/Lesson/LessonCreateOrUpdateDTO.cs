using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Logic.Models.Lesson
{
    public class LessonCreateOrUpdateDTO
    {
        public int Id { get; set; }
        public DayOfWeek Day { get; set; }
        [RegularExpression(@"^(?:[01]?[0-9]|2[0-3]):[0-5][0-9]$")]
        public TimeOnly Start { get; set; }
        [RegularExpression(@"^(?:[01]?[0-9]|2[0-3]):[0-5][0-9]$")]
        public TimeOnly End { get; set; }
        public string Place { get; set; }
        public int? ChosenTemplateId { get; set; }
        public List<int> AllowedTemplatesId { get; set; }
    }
}
