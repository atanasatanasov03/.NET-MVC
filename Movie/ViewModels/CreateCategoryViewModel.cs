namespace Movie.ViewModels
{
    public class CreateCategoryViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile CatImage { get; set; }
    }
}
