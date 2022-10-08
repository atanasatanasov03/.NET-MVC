using Movie.Models;

namespace Movie.ViewModels
{
    public class SingleMovieViewModel
    {
        public Film movie { get; set; }
        public Category category { get; set; }
    }
}
