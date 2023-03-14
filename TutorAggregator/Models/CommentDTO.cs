using TutorAggregator.DataEntities;

namespace TutorAggregator.Models
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string TutorLogin { get; set; }
        public string StudentLogin { get; set; }
        public string? Text { get; set; }
        public int Score { get; set; }
    }
}
