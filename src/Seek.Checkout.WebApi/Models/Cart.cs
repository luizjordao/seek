using Seek.Entities;
using System.Collections.Generic;

namespace Seek.Checkout.WebApi.Models
{
    public class Cart
    {
        public Customer Customer { get; set; }
        public IEnumerable<Sku> Skus { get; set; }
    }
}