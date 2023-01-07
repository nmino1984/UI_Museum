namespace UI_Museum.Models
{
    public class MuseumRequestViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Theme { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
