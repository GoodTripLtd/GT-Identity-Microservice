namespace Identity.Microservice.API.Models.Request
{
    public class ChangePasswordRequest
    {
        public string AccessToken { get; set; }
        public string PreviousPassword { get; set; }
        public string ProposedPassword { get; set; }
    }
}
