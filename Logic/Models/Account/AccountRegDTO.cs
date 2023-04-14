using System.ComponentModel.DataAnnotations;
using DAL.Entities.Enums;

namespace Logic.Models.Account
{
    public class AccountRegDTO
    {
        [RegularExpression(@"\w{4,15}")]
        public string Login { get; set; }
        [RegularExpression(@"(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{5,15})$")]
        public string Password { get; set; }
        [RegularExpression(@"(Student)|(Tutor)")]
        public Role Role { get; set; }
        [MinLength(1)]
        public string SecondName { get; set; }
        [MinLength(1)]
        public string FirstName { get; set; }
        [RegularExpression(@"8\d{10}")]
        public string PhoneNumber { get; set; }
    }
}
