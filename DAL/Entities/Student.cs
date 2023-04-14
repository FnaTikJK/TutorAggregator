using System.ComponentModel.DataAnnotations;
using DAL.Entities.Enums;

namespace DAL.Entities
{
    public class Student : IAccountEntity
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(256)]
        public string Login { get; set; }
        public string PasswordHash { get; set; }

        [MaxLength(50)]
        public string SecondName { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string? ThirdName { get; set; }

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



        //Relations
        public List<Comment> Comments { get; set; }
    }
}
