using System.Runtime.Serialization;

namespace ContactApi.Enums
{
    public enum ContactDetailTypeEnum
    {
        [EnumMember(Value = "Phone")]
        Phone = 0,
        [EnumMember(Value = "Email")]
        Email = 1,
        [EnumMember(Value = "Location")]
        Location = 2
    }
}
