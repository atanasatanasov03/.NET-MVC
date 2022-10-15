using Movie.Models;

namespace Movie.ViewModels
{
    public class SingleCategoryViewModel
    {
        public Category category { get; set; }
        public IEnumerable<Film> movies { get; set; }
        public double tripletsNum { get; set; }
    }
}
