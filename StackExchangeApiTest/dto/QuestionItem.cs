using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StackExchangeApiTest
{
    [DataContract]
    public class QuestionItem
    {
        [DataMember(Name = "view_count")]
        public int Count { get; set; }

        [DataMember(Name = "tags")]
        public List<string> Tags { get; set; }

    }
}