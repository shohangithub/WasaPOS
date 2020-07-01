using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Model
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Gender : byte
    {
        Male = 1,
        Female,
        NotDisclosed
    }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum LogScreen : byte
    {
        SignUp = 1,
        LogIn,
        Transaction
    }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Status : byte
    {
        Accepted = 1,
        Rejected= 2,
        Paid,
        Transaction
    }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CustomerType : byte
    {
        Regular = 1,
        ERegular,
       
    }
}
