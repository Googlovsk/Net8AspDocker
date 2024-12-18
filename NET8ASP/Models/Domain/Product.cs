﻿using System.ComponentModel.DataAnnotations;

namespace NET8ASP.Models.Domain
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image {  get; set; }
        public int Price { get; set; }
        public int Amount { get; set; }
        public int ManufId { get; set; }
        public int CategoryId { get; set; }

        public Category Category { get; set; }
        public Manufacturer Manufacturer { get; set; }

    }
}
