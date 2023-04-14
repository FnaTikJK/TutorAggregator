using DAL.Entities.Enums;

namespace Logic.Models
{
    public class TutorSearchFiltersDTO
    {
        public string? SubjectName { get; set; }
        public string? Place { get; set; }
        public LessonFormat? LessonFormat { get; set; }
        public Sex? DesiredSex { get; set; }
        public AgeGroup? DesiredAge { get; set; }
    }
}
