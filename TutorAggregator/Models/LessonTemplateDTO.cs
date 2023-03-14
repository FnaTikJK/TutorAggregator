using TutorAggregator.DataEntities;

namespace TutorAggregator.Models
{
    public class LessonTemplateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string? Description { get; set; }
        public LessonFormat? LessonFormat { get; set; }
        public DesiredSex? DesiredSex { get; set; }
        public DesiredAge? DesiredAge { get; set; }
    }
}
