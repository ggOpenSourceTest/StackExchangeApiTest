using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StackExchangeApiTest
{
    [DataContract]
    public class QuestionViewCount
    {
        [DataMember(Name = "items")]
        public List<ViewCount> ViewCounts { get; set; }
    }
}