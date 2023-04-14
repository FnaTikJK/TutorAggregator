using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class TutorWork
    {
        [Key]
        public int Id { get; set; }
        public string Company { get; set; }
        public string Position { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public Tutor Tutor{ get; set; }
    }
}
