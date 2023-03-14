using Microsoft.AspNetCore.SignalR;
using TutorAggregator.DataEntities;

namespace TutorAggregator.Models
{
    public class LessonDTO
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Place { get; set; }
        public int LengthInMinutes { get; set; }
        public int? StudentId { get; set; }
        public int? ChosenTemplateId { get; set; }
        public List<int> AllowedTemplatesId { get; set; }
    }
}
