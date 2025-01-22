﻿namespace Kafe.Models
{
    public class SpecialOffer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int IsActive { get; set; }
        public List<Order> Orders { get; } = []; 

        public List<Product> Products { get; } = [];
    }
}
