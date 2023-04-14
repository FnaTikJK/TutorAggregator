using DAL.Entities.Enums;

namespace Logic.Interfaces
{
    public interface IJWTService
    {
        public string CreateToken(string id, Role role);
    }
}
