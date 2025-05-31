namespace Identity.Microservice.AppCore.Options
{
    public class AmazonCognitoOptions
    {
        public string UserPoolId { get; set; } = string.Empty;

        public string ClientId { get; set; } = string.Empty;

        public string Region { get; set; } = string.Empty;
    }
}
