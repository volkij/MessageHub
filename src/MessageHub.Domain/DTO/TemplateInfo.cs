namespace MessageHub.Domain.DTO
{
    public class TemplateInfo
    {
        public required string Name { get; set; }

        public string Code { get; set; }

        public string Type { get; set; }

        public string Subject { get; set; }

        public string Url { get; set; }
    }
}