namespace ContactsApi.Models
{
    public class Contact
    {
        public long Id { get; set; }
        public string LastName { get; set; }
        public bool IsFamilyMember { get; set; }
        public string MobilePhone { get; set; }
    }
}
