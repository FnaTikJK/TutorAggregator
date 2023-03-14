using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorAggregator.DataEntities
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
