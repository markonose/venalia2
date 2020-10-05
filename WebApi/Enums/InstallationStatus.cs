using System.Runtime.Serialization;

namespace WebApi.Enums
{
    [DataContract]
    public enum InstallationStatus
    {
        [EnumMember(Value = "cancelled")]
        Cancelled = -1,

        [EnumMember(Value = "unassigned")]
        Unassigned = 0,

        [EnumMember(Value = "assigned")]
        Assigned = 1,

        [EnumMember(Value = "inProgress")]
        InProgress = 50,

        [EnumMember(Value = "completed")]
        Completed = 90,

        [EnumMember(Value = "approved")]
        Approved = 100,
    }
}
