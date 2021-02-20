﻿namespace ShopManagement.Application.Contract.Product
{
    public class ProductViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Seller { get; set; }
        public string PrimaryPicture { get; set; }
        public string Code { get; set; }
        public long CategoryId { get; set; }
        public string CreationDate { get; set; }
        public string Category { get; set; }

    }
}