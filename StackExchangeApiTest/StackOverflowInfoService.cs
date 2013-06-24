using System;
using System.Collections.Generic;
using System.Linq;
using StackExchangeApiTest.dto;

namespace StackExchangeApiTest
{
    class StackOverflowInfoService
    {
        private const int Pagesize = 100;
        private readonly RestClient _client;

        public StackOverflowInfoService(RestClient client)
        {
            _client = client;
        }

        public StackOverFlowInfo GetStackOverflowInfo(DateTime queryDate)
        {
            var totalNumberOfQuestionsAsked = GetTotalNumberOfQuestionsAsked(queryDate);
            var questions = GetAllQuestionsWithViewCountAndTags(queryDate, totalNumberOfQuestionsAsked);

            return new StackOverFlowInfo
                {
                    TotalNumberofQuestions = totalNumberOfQuestionsAsked,
                    TotalNumberofViews = questions.Sum(x=>x.Count),
                    OrderedListOfDistinctTags = string.Join(",", questions.SelectMany(x=>x.Tags).Distinct().OrderBy(x=>x))
                };
        }

        private List<QuestionItem> GetAllQuestionsWithViewCountAndTags(DateTime queryDate, int totalNumberofQuestions)
        {
            var questions = new List<QuestionItem>();
            for (int pageIndex = 1; pageIndex <= DivideRoundingUp(totalNumberofQuestions, Pagesize); pageIndex++)
            {
                questions.AddRange(GetPagedQuestionSet(pageIndex, queryDate));
            }
            return questions;
        }

        private IEnumerable<QuestionItem> GetPagedQuestionSet(int pageIndex, DateTime queryDate)
        {
            var request = GetRequest(queryDate);
            request.AddParameter("page", pageIndex.ToString());
            request.AddParameter("pagesize", Pagesize.ToString());
            request.AddParameter("filter", "!nQV7ekQxbL");

            var response = _client.Execute(request);
            var pageresult = JsonHelper.JsonDeserialize<Questions>(response);
            return pageresult.Items;
        }

        private RestRequest GetRequest(DateTime queryDate)
        {
            var request = new RestRequest("questions", HttpVerb.GET);
            request.AddParameter("site", "stackoverflow");
            request.AddParameter("fromdate", queryDate.ToUnixTimestamp().ToString());
            request.AddParameter("todate", queryDate.AddHours(24).ToUnixTimestamp().ToString());
            return request;
        }

        private int GetTotalNumberOfQuestionsAsked(DateTime queryDate)
        {
            var request = GetRequest(queryDate);
            request.AddParameter("filter", "total");
            var response = _client.Execute(request);
            var result = JsonHelper.JsonDeserialize<Total>(response);
            return result.TotalQuestions;
        }

        private static int DivideRoundingUp(int x, int y)
        {
            int remainder;
            int quotient = Math.DivRem(x, y, out remainder);
            return remainder == 0 ? quotient : quotient + 1;
        }
    }
}