using System.Runtime.Serialization;

namespace WebApi.Enums
{
    [DataContract]
    public enum UserStatus
    {
        [EnumMember(Value = "demo")]
        Demo,
        [EnumMember(Value = "ready")]
        Ready,
        [EnumMember(Value = "active")]
        Active,
    }
}
