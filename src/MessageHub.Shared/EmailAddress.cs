namespace MessageHub.Shared
{
    public class EmailAddress
    {
        public EmailAddress(string email, string name)
        {
            Email = email;
            Name = name;
        }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
