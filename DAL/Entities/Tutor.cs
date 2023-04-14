using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Entities.Enums;

namespace DAL.Entities
{
    public class Tutor : IAccountEntity
    { 
        [Key]
        public int Id { get; set; }

        [MaxLength(256)]
        public string Login { get; set; }
        [MaxLength(256)]
        public string PasswordHash { get; set; }

        [MaxLength(50)]
        public string SecondName { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string? ThirdName { get; set; }

        [NotMapped]
        public string? FullName { get =>  $"{SecondName} {FirstName} {ThirdName}".Trim();}
        public DateTime? LastLogin { get; set; }

        [MinLength(12)]
        [MaxLength(12)]
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public Sex? Sex { get; set; }
        public string? Photo { get; set; }
        public string? AboutMyself { get; set; }
        public string? Region { get; set; }
        public string? HowToCommunicate { get; set; }
        public bool? IsVerifiedProfile { get; set; }





        //Relations
        public TutorEducation Education { get; set; }
        public List<LessonTemplate> LessonTemplates { get; set; }
        public List<TutorWork> WorkRecords { get; set; }
        public List<TutorAchievements> Achievements { get; set; }
        public List<Comment> Comments { get; set; }

        //
    }
}
