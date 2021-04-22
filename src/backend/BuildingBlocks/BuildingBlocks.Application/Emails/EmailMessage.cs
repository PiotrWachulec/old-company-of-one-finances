namespace BuildingBlocks.Application.Emails
{
    public struct EmailMessage
    {
        public string To { get; }

        public string Subject { get; }

        public string Content { get; }

        public EmailMessage(string to, string subject, string content)
        {
            To = to;
            Subject = subject;
            Content = content;
        }
    }
}