using System.Runtime.Serialization;

namespace StackExchangeApiTest
{
    [DataContract]
    public class Total
    {
        [DataMember(Name = "total")]
        public int TotalQuestions { get; set; }
    }
}