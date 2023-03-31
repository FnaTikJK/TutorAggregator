namespace Logic.Interfaces
{
    public interface IJWTService
    {
        public string CreateToken(string id, string role);
    }
}
