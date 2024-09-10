namespace MessageHub.Shared
{
    /// <summary>
    /// Configuration for the sender
    /// </summary>
    public class SenderConfig
    {
        public string Code { get; set; }
        public string Type { get; set; }
        public string? ApiKey { get; set; }
        public string? Url { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? EmailName { get; set; }
        public string? PhoneName { get; set; }
        public string ClassName { get; set; }
        public string AssemblyName { get; set; }
        public string? ConfigFileName { get; set; }
    }
}