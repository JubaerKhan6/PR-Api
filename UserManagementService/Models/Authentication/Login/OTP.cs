namespace UserManagementService.Models.Authentication.Login
{
    public class OTP
    {
        public int id { get; set; }
        public string Userid { get; set; }
        public string Otp { get; set; }
        public int Status { get; set; }
        public DateTime Generation_Time { get; set; }
    }
}
