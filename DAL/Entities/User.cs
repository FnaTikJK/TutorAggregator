using DAL.Entities.Enums;

namespace DAL.Entities
{
    public class User
    {
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public Role Role { get; set; }
    }
}
