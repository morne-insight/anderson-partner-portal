namespace AndersonAPI.Application.Common.ConfigOptions
{
    public class SmtpOptions
    {
        public string Host { get; init; } = string.Empty;
        public int Port { get; init; } = 25;
        public bool EnableSsl { get; init; } = false;
        public string Username { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public string FromAddress { get; init; } = string.Empty;
        public string FromName { get; init; } = string.Empty;
    }
}
