namespace MessageHub.Domain.Entities
{
    public class PushToken : Entity
    {
        public DateTime CreateDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public string Value { get; set; }

        public string Account { get; set; }
        public string? ExternalClientID { get; set; }

    }
}
