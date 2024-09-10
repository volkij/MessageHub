namespace MessageHub.Domain.DTO
{
    /// <summary>
    /// Request to reload a template
    /// </summary>
    public class TemplateReloadRequest
    {
        public string Url { get; set; }
    }
}