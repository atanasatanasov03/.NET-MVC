﻿namespace Movie.ViewModels
{
    public class CreateMovieViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile FilmImage { get; set; }
        public Guid CategoryId { get; set; }
    }
}
