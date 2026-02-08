using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlatformaKursy.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty; // Identity user id

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public decimal Total { get; set; }

        public List<OrderItem> Items { get; set; } = new();
    }
}