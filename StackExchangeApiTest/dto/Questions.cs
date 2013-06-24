using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StackExchangeApiTest.dto
{
    [DataContract]
    public class Questions
    {
        [DataMember(Name = "items")]
        public List<QuestionItem> Items { get; set; }
    }


}