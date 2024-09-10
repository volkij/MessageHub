using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessageHub.Domain.Entities
{
    public class MessageAttachment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public string FileContent { get; set; }
        public int MessageId { get; set; }

        [ForeignKey("MessageId")]
        public Message Message { get; set; }
    }
}