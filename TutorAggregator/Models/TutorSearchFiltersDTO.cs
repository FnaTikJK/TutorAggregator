using TutorAggregator.DataEntities;

namespace TutorAggregator.Models
{
    public class TutorSearchFiltersDTO
    {
        public string? SubjectName { get; set; }
        public string? Place { get; set; }
        public LessonFormat? LessonFormat { get; set; }
        public DesiredSex? DesiredSex { get; set; }
        public DesiredAge? DesiredAge { get; set; }
    }
}
