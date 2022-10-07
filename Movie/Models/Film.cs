namespace Movie.Models
{
    public class Film
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
