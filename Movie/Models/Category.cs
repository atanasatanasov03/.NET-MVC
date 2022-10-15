﻿namespace Movie.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? CatImage { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
