using System.ComponentModel.DataAnnotations;
using DAL.Entities.Enums;

namespace Logic.Models
{
    public class ProfileDTO
    {
        [MinLength(1)]
        public string? SecondName { get; set; }
        [MinLength(1)]
        public string? FirstName { get; set; }
        public string? ThirdName { get; set; }
        [RegularExpression(@"8\d{10}")]
        public string? PhoneNumber { get; set; }
        //[RegularExpression(@"/^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/")]
        public string? Email { get; set; }
        public DateTime? BirthDate { get; set; }
        [RegularExpression(@"(Male)|(Female)")]
        public Sex? Sex { get; set; }
        public string? Photo { get; set; }
        public string? AboutMyself { get; set; }
        public string? Region { get; set; }
        public string? HowToCommunicate { get; set; }
    }
}
