using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace StackExchangeApiTest
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                DateTime resultDay = DateTime.ParseExact(args[0], "yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
                var client = new RestClient("https://api.stackexchange.com/2.0/");
                var totalNumberofQuestions = NumberOfQuestionsAsked(resultDay, client);
                Console.WriteLine(string.Format("Number of questions asked {0}", totalNumberofQuestions));
                Console.WriteLine(string.Format("Number of views are {0} across {1}", TotalNumberOfViewsAcrossAllQuestions(resultDay, client, totalNumberofQuestions), totalNumberofQuestions)); 
            }
            else
            {
                Console.WriteLine("Please provide date in yyyy-MM-dd format");
            }
        }

        private static int TotalNumberOfViewsAcrossAllQuestions(DateTime resultDay, RestClient client, int totalNumberofQuestions)
        {
            var pagesize = 100;
            var viewCount = 0;
            for (int pageIndex = 1; pageIndex <= DivideRoundingUp(totalNumberofQuestions , pagesize); pageIndex++)
            {
                var request = GetRequest(resultDay);
                request.AddParameter("page", pageIndex.ToString());
                request.AddParameter("pagesize", pagesize.ToString());
                request.AddParameter("filter", "!-u2C*PHP"); // A custom filter to just load view_Counts for each question.
              
                var response = client.Execute(request);
                var pageresult = JsonHelper.JsonDeserialize<QuestionViewCount>(response);
                viewCount += pageresult.ViewCounts.Sum(x=>x.Count);
            }

            return viewCount;
            //Console.WriteLine(string.Format("QuestionsTotal Number of questions asked {0}", client.Execute(request)));
        }

        private static RestRequest GetRequest(DateTime resultDay)
        {
            var request = new RestRequest("questions", HttpVerb.GET);
            request.AddParameter("site", "stackoverflow");
            request.AddParameter("fromdate", resultDay.ToUnixTimestamp().ToString());
            request.AddParameter("todate", resultDay.AddHours(24).ToUnixTimestamp().ToString());
            return request;
        }

        private static int NumberOfQuestionsAsked(DateTime resultDay, RestClient client)
        {
            var request = GetRequest(resultDay);
            request.AddParameter("filter", "total");
            var response = client.Execute(request);
            var result = JsonHelper.JsonDeserialize<Total>(response);
            return result.TotalQuestions;
            //Console.WriteLine(string.Format("QuestionsTotal Number of questions asked {0}", client.Execute(request)));
        }

        public static int DivideRoundingUp(int x, int y)
        {
            int remainder;
            int quotient = Math.DivRem(x, y, out remainder);
            return remainder == 0 ? quotient : quotient + 1;
        }
    }

    [DataContract]
    public class Total
    {
        [DataMember(Name = "total")]
        public int TotalQuestions { get; set; }
    }

    [DataContract]
    public class QuestionViewCount
    {
        [DataMember(Name = "items")]
        public List<ViewCount> ViewCounts { get; set; }
    }

    [DataContract]
    public class ViewCount
    {
        [DataMember(Name = "view_count")]
        public int Count { get; set; }
    }

    public static class DateTimeExtensions
    {
        public static long ToUnixTimestamp(this DateTime d)
        {
            var duration = d - new DateTime(1970, 1, 1, 0, 0, 0);

            return (long)duration.TotalSeconds;
        }
    }

    public class JsonHelper
    {
        /// <summary>
        /// JSON Serialization
        /// </summary>
        public static string JsonSerializer<T>(T t)
        {
            var ser = new DataContractJsonSerializer(typeof(T));
            var ms = new MemoryStream();
            ser.WriteObject(ms, t);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }
        /// <summary>
        /// JSON Deserialization
        /// </summary>
        public static T JsonDeserialize<T>(string jsonString)
        {
            var ser = new DataContractJsonSerializer(typeof(T));
            T obj;
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                 obj = (T) ser.ReadObject(ms);
            }
            return obj;
        }
    }
}
