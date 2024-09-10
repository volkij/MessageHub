using System.ComponentModel.DataAnnotations;

namespace MessageHub.Domain.Entities
{
    public class Template : Entity
    {

        [Required]
        public string Type { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        public string Url { get; set; }

        public string Content { get; set; }

        public string Subject { get; set; }

        public DateTime DateMod { get; set; }
    }
}