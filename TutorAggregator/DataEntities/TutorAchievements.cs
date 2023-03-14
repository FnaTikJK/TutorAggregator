using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorAggregator.DataEntities
{
    public class TutorAchievements
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfReceive { get; set; }
        public Tutor Tutor{ get; set; }
    }
}
