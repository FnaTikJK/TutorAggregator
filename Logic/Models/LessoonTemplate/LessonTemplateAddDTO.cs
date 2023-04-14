using DAL.Entities.Enums;

namespace Logic.Models.LessoonTemplate
{
    public class LessonTemplateAddDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string? Description { get; set; }
        public LessonFormat? LessonFormat { get; set; }
        public Sex? DesiredSex { get; set; }
        public AgeGroup? DesiredAge { get; set; }
    }
}
