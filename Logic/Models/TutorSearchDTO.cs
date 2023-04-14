using DAL.Entities;
using DAL.Entities.Enums;

namespace Logic.Models
{
    public class TutorSearchDTO
    {
        public TutorSearchDTO(DAL.Entities.Tutor tutor)
        {
            Id = tutor.Id;
            Login = tutor.Login;
            SecondName = tutor.SecondName;
            FirstName = tutor.FirstName;
            ThirdName = tutor.ThirdName;
            BirthDate = tutor.BirthDate;
            Sex = tutor.Sex;
            Photo = tutor.Photo;
            AboutMyself = tutor.AboutMyself;
            Region = tutor.Region;
            HowToCommunicate = tutor.HowToCommunicate;
            IsVerifiedProfile = tutor.IsVerifiedProfile;
        }
        public TutorSearchDTO()
        {

        }
        public int Id { get; set; }
        public string? Login { get; set; }
        public string? SecondName { get; set; }
        public string? FirstName { get; set; }
        public string? ThirdName { get; set; }

        public string? FullName { get => $"{SecondName} {FirstName} {ThirdName}".Trim(); }

        public DateTime? BirthDate { get; set; }
        public Sex? Sex { get; set; }
        public string? Photo { get; set; }

        public string? AboutMyself { get; set; }

        public string? Region { get; set; }
        public string? HowToCommunicate { get; set; }
        public bool? IsVerifiedProfile { get; set; }
    }
}
