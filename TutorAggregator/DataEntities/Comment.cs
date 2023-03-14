using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorAggregator.DataEntities
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public Tutor Tutor { get; set; }

        public Student Student { get; set; }

        [MaxLength(1000)]
        public string? Text { get; set; }
        public int Score { get; set; }
    }
}
