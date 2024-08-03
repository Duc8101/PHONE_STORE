using System.ComponentModel;

namespace Common.Enums
{
    public enum UserInfo
    {
        Phone_Length = 10,
        Min_Length_Username = 6,
        Max_Length_Username = 50,

        [Description(@"^[a-zA-Z][\w-]+@([\w]+.[\w]+|[\w]+.[\w]{2,}.[\w]{2,})")]
        Format_Email
    }
}
