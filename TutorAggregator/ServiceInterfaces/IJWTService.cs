namespace TutorAggregator.ServiceInterfaces
{
    public interface IJWTService
    {
        public string CreateToken(string id, string role);
    }
}
