using ContactApi.Enums;

namespace ContactApi.Entities
{
    public class ContactDetail
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public Contact Contact { get; set; }
        public ContactDetailTypeEnum ContactDetailType { get; set; }
        public string Content { get; set; }
    }
}
