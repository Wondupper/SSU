using System;
using System.ComponentModel.DataAnnotations;

namespace ShawarmaShop.Models
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Customer()
        {
            Id = Guid.NewGuid();
        }
    }

    public class Seller
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Seller()
        {
            Id = Guid.NewGuid();
        }
    }

    public class Shawarma
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Shawarma()
        {
            Id = Guid.NewGuid();
        }
    }

    public class Order
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }
        public Guid SellerId { get; set; }
        public Guid ShawarmaId { get; set; }

        public Order()
        {
            Id = Guid.NewGuid();
        }
    }
}
