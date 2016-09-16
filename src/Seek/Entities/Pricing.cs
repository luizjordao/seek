using System;

namespace Seek.Entities
{
    public abstract class Pricing
    {
        public Pricing(Sku sku, Customer customer)
        {
            Sku = sku;
            Customer = customer;
        }

        public Sku Sku { get; set; }
        public Customer Customer { get; set; }

        public abstract decimal ApplyDiscount(int quantidade);
    }

    public class PercentualDiscount : Pricing
    {
        public PercentualDiscount(Sku sku, Customer customer):base(sku,customer)
        { }

        public byte MinimumQuantity { get; set; }
        public decimal Percentage { get; set; }

        public override decimal ApplyDiscount(int quantidade)
        {
            if (quantidade < MinimumQuantity)
            {
                return 0;
            }

            return quantidade * (Sku.Price * Percentage);
        }
    }

    public class NextForFreeDiscount : Pricing
    {
        public NextForFreeDiscount(Sku sku, Customer customer):base(sku,customer)
        { }

        public byte Range { get; set; }

        public override decimal ApplyDiscount(int quantidade)
        {
            return (quantidade / Range) * Sku.Price;
        }
    }
}
