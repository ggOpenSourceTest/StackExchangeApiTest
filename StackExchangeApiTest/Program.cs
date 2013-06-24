using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace StackExchangeApiTest
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                DateTime queryDate = DateTime.ParseExact(args[0], "yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
                var client = new RestClient("https://api.stackexchange.com/2.0/");
                var result = new StackOverflowInfoService(client).GetStackOverflowInfo(queryDate);

                Console.WriteLine("Number of questions asked {0}", result.TotalNumberofQuestions);
                Console.WriteLine("Number of views are {0} across {1}", result.TotalNumberofViews, result.TotalNumberofQuestions);
                Console.WriteLine("All the tags on the questions for the day are {0} ", result.OrderedListOfDistinctTags); 
            }
            else
            {
                Console.WriteLine("Please provide date in yyyy-MM-dd format");
            }
            Console.ReadLine();
        }
    }
}
