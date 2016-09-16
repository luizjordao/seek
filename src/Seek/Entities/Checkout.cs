using Seek.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Seek.Entities
{
    public class Checkout
    {
        private readonly GetPricingByCustomerAndSkus _getPricing;
        private readonly GetSkuById _getSkuById;
        private readonly Customer _customer;
        private readonly IList<Sku> _skus = new List<Sku>();

        protected IEnumerable<Pricing> PricingRules
        {
            get
            {
                var skus = _skus.Select(s => s.Id).ToArray();
                return _getPricing.GetResult(_customer.Id, skus);
            }
        }

        protected decimal TotalPrice
        {
            get
            {
                return _skus.Sum(s => s.Price);
            }
        }

        public Checkout(GetPricingByCustomerAndSkus getPricing, GetSkuById getSkuById, Customer customer)
        {
            _getPricing = getPricing;
            _getSkuById = getSkuById;
            _customer = customer;
        }

        public void AddItems(IEnumerable<Sku> skus)
        {
            if (skus == null)
            {
                throw new ArgumentNullException("skus", "Skus should not be null.");
            }

            foreach (var sku in skus)
            {
                var persistedSku = GetPersistedSku(sku);
                _skus.Add(persistedSku);
            }
        }

        private Sku GetPersistedSku(Sku sku)
        {
            var persistedSku = _getSkuById.GetResult(sku.Id);

            if (persistedSku == null)
            {
                throw new InvalidOperationException("Sku not found.");
            }

            return persistedSku;
        }

        public decimal GetTotal()
        {
            var totalDiscount = 0m;

            foreach (var rule in PricingRules)
            {
                totalDiscount += rule.ApplyDiscount(_skus.Count(s => s.Id == rule.Sku.Id));
            }

            return TotalPrice - totalDiscount;
        }
    }
}
