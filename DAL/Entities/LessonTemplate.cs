using System.ComponentModel.DataAnnotations;
using DAL.Entities.Enums;

namespace DAL.Entities
{
    public class LessonTemplate
    {
        [Key]
        public int Id { get; set; }
        public Tutor Tutor { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string? Description { get; set; }
        public LessonFormat? LessonFormat { get; set; }
        public Sex? DesiredSex { get; set; }
        public AgeGroup? DesiredAge { get; set; }

        public List<Lesson> Lessons { get; set; }
    }
}
