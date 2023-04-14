using System.ComponentModel.DataAnnotations;

namespace Logic.Models.Student
{
    public class StudentOutDTO
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string SecondName { get; set; }
        public string FirstName { get; set; }
        public string? ThirdName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Sex { get; set; }
        public string? Photo { get; set; }
    }
}
