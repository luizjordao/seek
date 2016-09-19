using FluentAssertions;
using Moq;
using Nancy.Testing;
using NUnit.Framework;
using Seek.Checkout.WebApi.Modules;
using Seek.Checkout.WebApi.Tests.Utils;
using Seek.Entities;
using Seek.Queries;
using System.Collections.Generic;

namespace Seek.Checkout.WebApi.Tests
{
    [TestFixture]
    public class CheckoutModuleTests
    {
        private BrowserResponse _response;
        private Browser _browser;

        private Mock<GetPricingByCustomerAndSkus> _getPricingByCustomerAndSkusMock;
        private Mock<GetSkuById> _getSkuByIdMock;

        private Sku _standout, _classic, _premium;
        private Customer _apple, _unilever, _nike;
        

        [SetUp]
        public void Setup()
        {
            _getPricingByCustomerAndSkusMock = new Mock<GetPricingByCustomerAndSkus>();
            _getSkuByIdMock = new Mock<GetSkuById>();

            _apple = new Customer { Id = "apple" };
            _unilever = new Customer { Id = "unilever" };
            _nike = new Customer { Id = "nike" };

            _standout = new Sku() { Id = "standout", Price = 322.99m };
            _classic = new Sku { Id = "premium", Price = 269.99m };
            _premium = new Sku { Id = "premium", Price = 394.99m };

            _browser = new Browser(config =>
            {
                config.Module<CheckoutModule>();
                config.Dependency(_getPricingByCustomerAndSkusMock.Object);
                config.Dependency(_getSkuByIdMock.Object);
                config.EnableAutoRegistration();
            });
        }


        private void Post(string testFile)
        {
            _response = _browser.Post("seek/checkout", with =>
            {
                with.HttpRequest();
                with.Header("Content-Type", "application/json");
                with.Body(TestFilesHelper.ReadFile(testFile));
            });
        }

        [Test]
        public void Post_GivenAPurchaseFromApple_ShouldReturnTotalPriceProperly_And_HttpStatus_Should_Be_Ok()
        {
            _getSkuByIdMock.Setup(s =>
                s.GetResult(It.IsAny<string>()))
                .Returns( (string param) => 
                    param == "standout" ? _standout : param == "premium" ? _premium : new Sku()
                );                

            _getPricingByCustomerAndSkusMock.Setup(s =>
                s.GetResult(It.IsAny<string>(), It.IsAny<string[]>()))
                .Returns(new List<Pricing>()               
                {
                    new PercentualDiscount(_standout, _apple) {MinimumQuantity = 1, Percentage = 0.07121m}
                });

            Post("post\\apple.purchase.json");

            _response.Body.AsString().Should().Be("1294.9596463");
        }

        [Test]
        public void Post_GivenAPurchaseFromNike_ShouldReturnTotalPriceProperly_And_HttpStatus_Should_Be_Ok()
        {
            _getSkuByIdMock.Setup(s =>
                s.GetResult(It.IsAny<string>()))
                .Returns(_premium);

            _getPricingByCustomerAndSkusMock.Setup(s =>
                s.GetResult(It.IsAny<string>(), It.IsAny<string[]>()))
                .Returns(new List<Pricing>()
                {
                    new PercentualDiscount(_premium, _nike) {MinimumQuantity = 4, Percentage = 0.03798m}
                });

            Post("post\\nike.purchase.json");

            _response.Body.AsString().Should().Be("1519.9531192");
        }

        [Test]
        public void Post_GivenAPurchaseFromUnilever_ShouldReturnTotalPriceProperly_And_HttpStatus_Should_Be_Ok()
        {
            _getSkuByIdMock.Setup(s =>
                 s.GetResult(It.IsAny<string>()))
                 .Returns((string param) =>
                     param == "classic" ? _classic : param == "premium" ? _premium : new Sku()
                 );

            _getPricingByCustomerAndSkusMock.Setup(s =>
                s.GetResult(It.IsAny<string>(), It.IsAny<string[]>()))
                .Returns(new List<Pricing>()
                {
                    new NextForFreeDiscount(_classic, _unilever) { Range = 3}
                });

            Post("post\\unilever.purchase.json");

            _response.Body.AsString().Should().Be("934.97");
        }

        [Test]
        public void Post_GivenADefaultPurchase_ShouldReturnTotalPriceProperly_And_HttpStatus_Should_Be_Ok()
        {
            _getSkuByIdMock.Setup(s =>
                 s.GetResult(It.IsAny<string>()))
                 .Returns((string param) =>
                     param == "classic" ? _classic : param == "premium" ? _premium : _standout
                 );

            _getPricingByCustomerAndSkusMock.Setup(s =>
                s.GetResult(It.IsAny<string>(), It.IsAny<string[]>()))
                .Returns(new List<Pricing>());

            Post("post\\default.purchase.json");

            _response.Body.AsString().Should().Be("987.97");
        }

        //TODO: Create tests for possible exceptions 

    }
}
