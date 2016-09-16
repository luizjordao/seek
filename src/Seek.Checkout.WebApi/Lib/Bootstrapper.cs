using LightInject;
using LightInject.Nancy;
using Nancy.Bootstrapper;
using Nancy.Json;
using Seek.Queries;

namespace Seek.Checkout.WebApi.Lib
{
    public class Bootstrapper: LightInjectNancyBootstrapper
    {
        public IServiceContainer Container { get { return ApplicationContainer; } }

        protected override void ApplicationStartup(IServiceContainer container, IPipelines pipelines)
        {
            JsonSettings.MaxJsonLength = int.MaxValue;
            base.ApplicationStartup(container, pipelines);
        }

        protected override void ConfigureApplicationContainer(IServiceContainer existingContainer)
        {
            base.ConfigureApplicationContainer(existingContainer);

            Container.Register<GetPricingByCustomerAndSkus>();
            Container.Register<GetSkuById>();
        }
    }
}