using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorAggregator.DataEntities
{
    public enum LessonFormat: byte
    {
        Online,
        Offline
    }
    public enum DesiredSex : byte
    {
        Male,
        Female,
        All
    }
    public enum DesiredAge : byte
    {
        Preschoolers,
        PrimarySchoolers,
        MiddleSchoolers,
        HighSchoolers,
        Students,
        Adults,
        All
    }
    public class LessonTemplate
    {
        [Key]
        public int Id { get; set; }
        public Tutor Tutor { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string? Description { get; set; }
        public LessonFormat? LessonFormat { get; set; }
        public DesiredSex? DesiredSex { get; set; }
        public DesiredAge? DesiredAge { get; set; }

        public List<Lesson> Lessons { get; set; }
    }
}
