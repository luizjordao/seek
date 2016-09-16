using Nancy.Testing;
using NUnit.Framework;
using Seek.Checkout.WebApi.Modules;
using Seek.Checkout.WebApi.Tests.Utils;

namespace Seek.Checkout.WebApi.Tests
{
    [TestFixture]
    public class CheckoutModuleTests
    {
        private BrowserResponse _response;
        private Browser _browser;

        [SetUp]
        public void Setup()
        {
            _browser = new Browser(config =>
            {
                config.Module<CheckoutModule>();
                config.EnableAutoRegistration();
            });
        }

        [TestCase("post\\apple.purchase.json", ExpectedResult = "1294.9596463")]
        [TestCase("post\\unilever.purchase.json", ExpectedResult = "934.97")]
        [TestCase("post\\nike.purchase.json", ExpectedResult = "1519.9531192")]
        [TestCase("post\\default.purchase.json", ExpectedResult = "987.97")]
        public string Post_GivenACart_ShouldReturnTotalPriceProperly_And_HttpStatusOk(string testFile)
        {
            _response = _browser.Post("seek/checkout", with =>
            {
                with.HttpRequest();
                with.Header("Content-Type", "application/json");
                with.Body(TestFilesHelper.ReadFile(testFile));
            });

            return _response.Body.AsString();
        }




    }
}
