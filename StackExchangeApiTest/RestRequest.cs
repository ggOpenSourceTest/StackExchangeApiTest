using System.Collections.Generic;

namespace StackExchangeApiTest
{
   public class RestRequest
    {
        public RestRequest(string path, HttpVerb requestType)
        {
            Path = path;
            RequstType = requestType;
            Parameters = new List<Parameter>();
        }

        public IList<Parameter> Parameters { get; set; }

        public RestRequest AddParameter(string name, string value)
        {
            Parameters.Add(new Parameter { Name = name, Value = value });
            return this;
        }

        public string Path { get; set; }

        public HttpVerb RequstType { get; set; }
    }
}
