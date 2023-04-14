using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
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
