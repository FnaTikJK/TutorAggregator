namespace TutorAggregator.Models
{
    public class AccountChangePasswordDTO
    {
        public string Login { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
