using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;

namespace StackExchangeApiTest
{
    // TODO: Currently only GET is implemented. Expand the class to support other reuqest types (POST, PUT, DELETE).
    public class RestClient
    {
        private readonly string _apiUrl;

        public RestClient(string apiUrl)
        {
            this._apiUrl = apiUrl;
        }

        public string Execute(RestRequest request)
        {
            return GetResponseValue(ToHttpWebRequest(request, _apiUrl));
        }

        private HttpWebRequest ToHttpWebRequest(RestRequest request, string apiUrl)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}{1}?{2}", apiUrl, request.Path, ParametersAsString(request.Parameters)));
            httpWebRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
            httpWebRequest.Method = httpWebRequest.Method;
            httpWebRequest.ContentLength = 0;
            httpWebRequest.ContentType = "text/plain";
            httpWebRequest.Accept = "*/*";
            return httpWebRequest;
        }

        private static string ParametersAsString(IList<Parameter> parameters)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < parameters.Count; i++)
            {
                sb = sb.Append(parameters[i].ToString() + (i == parameters.Count - 1 ? string.Empty : "&"));
            }
            return sb.ToString();
        }

        private string GetResponseValue(HttpWebRequest request)
        {
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                var responseValue = string.Empty;

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var message = String.Format("Request failed. Received HTTP {0}", response.StatusCode);
                    throw new ApplicationException(message);
                }
                // grab the response
                using (var responseStream = GetResponseStream(response))
                {
                    if (responseStream != null)
                    {
                        using (var reader = new StreamReader(responseStream))
                        {
                            responseValue = reader.ReadToEnd();
                        }
                    }
                }
                return responseValue;
            }
        }

        private static Stream GetResponseStream(HttpWebResponse response)
        {
            var responseStream = response.GetResponseStream();
            if (responseStream != null)
            {
                if (response.ContentEncoding.ToLower().Contains("gzip"))
                    responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                else if (response.ContentEncoding.ToLower().Contains("deflate"))
                    responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);
            }
            return responseStream;
          }
    }
}
