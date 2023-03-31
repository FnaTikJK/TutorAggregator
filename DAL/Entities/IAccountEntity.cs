using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public interface IAccountEntity
    {
        public int Id { get; set; }

        [MaxLength(256)]
        public string Login { get; set; }
        [MaxLength(256)]
        public string PasswordHash { get; set; }
    }
}
