using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessageHub.Domain.Entities
{
    public class MessageAttribute
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }
        public int MessageId { get; set; }

        [ForeignKey("MessageId")]
        public Message Message { get; set; }
    }
}
