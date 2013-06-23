using NUnit.Framework;

namespace StackExchangeApiTest.Tests
{
    [TestFixture]
    public class StackExchangeApiTestTests
    {
        [Test]
        [Category("IntegrationTest")]
        public void ApiTest()
        {
            // Arrange
            var client = new RestClient("https://api.stackexchange.com/2.0/");
            var request = new RestRequest("questions", HttpVerb.GET);
            request.AddParameter("site", "stackoverflow");
            request.AddParameter("filter", "total");

            // Act
            string response = client.Execute(request);

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(response));
        }

    }
}
