using System.Runtime.Serialization;

namespace StackExchangeApiTest
{
    [DataContract]
    public class ViewCount
    {
        [DataMember(Name = "view_count")]
        public int Count { get; set; }
    }
}