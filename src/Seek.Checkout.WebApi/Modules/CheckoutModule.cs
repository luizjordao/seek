using Nancy;
using Nancy.ModelBinding;
using Seek.Checkout.WebApi.Models;
using Seek.Checkout.WebApi.Modules.Base;
using Seek.Queries;

namespace Seek.Checkout.WebApi.Modules
{
    public class CheckoutModule : BaseModule
    {
        private readonly GetPricingByCustomerAndSkus _getPricingByCustomerAndSkus;
        private readonly GetSkuById _getSkuById;

        public CheckoutModule(GetPricingByCustomerAndSkus getPricingByCustomerAndSkus, GetSkuById getSkuById) :base("seek")
        {
            _getPricingByCustomerAndSkus = getPricingByCustomerAndSkus;
            _getSkuById = getSkuById;

            Post["checkout"] = parameters => HandleError(() => Purchase(this.Bind<Cart>()));
        }

        private Response Purchase(Cart cart)
        {
            var checkout = new Entities.Checkout(_getPricingByCustomerAndSkus, _getSkuById, cart.Customer);
            checkout.AddItems(cart.Skus);

            return Response.AsJson(checkout.GetTotal());            
        }
    }
}