using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorAggregator.DataEntities
{

    public enum EducationLevel : byte
    {
        Baccalaureate,
        Magistracy,
        Specialist,
        Other
    }
    public class TutorEducation
    {
        [Key]
        [ForeignKey("Tutor")]
        public int TutorEducationId { get; set; }
        public string University{ get; set; }
        public string Faculty { get; set; }
        public string Profession { get; set; }
        public DateTime EntranceDate { get; set; }
        public DateTime GraduationDate { get; set; }
        public EducationLevel EducationLevel { get; set; }
        public Tutor Tutor{ get; set; }
    }
}
