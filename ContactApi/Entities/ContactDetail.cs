using ContactApi.Enums;

namespace ContactApi.Entities
{
    public class ContactDetail
    {
        public Guid Id { get; set; }
        public Guid ContactId { get; set; }
        public Contact Contact { get; set; }
        public ContactDetailTypeEnum ContactDetailType { get; set; }
        public string Content { get; set; }
    }
}
