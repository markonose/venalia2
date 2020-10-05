using System.Runtime.Serialization;

namespace WebApi.Enums
{
    [DataContract]
    public enum UserType
    {
        [EnumMember(Value = "administrator")]
        Administrator,
        [EnumMember(Value = "business")]
        Business,
        [EnumMember(Value = "installer")]
        Installer
    }
}
