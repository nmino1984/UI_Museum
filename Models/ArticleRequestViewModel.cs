namespace UI_Museum.Models
{
    public class ArticleRequestViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool? IsDamaged { get; set; }
        public int IdMuseum { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
