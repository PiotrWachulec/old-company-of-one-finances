namespace BuildingBlocks.Infrastructure.Emails
{
    public class EmailsConfiguration
    {
        public string FromEmail { get; }
        
        public EmailsConfiguration(string fromEmail)
        {
            FromEmail = fromEmail;
        }
    }
}