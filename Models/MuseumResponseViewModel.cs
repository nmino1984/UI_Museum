using Utiles;

namespace UI_Museum.Models
{
    public class MuseumResponseViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int ThemeId { get; set; }
        public Themes Theme { get; set; }
        public List<ArticleResponseViewModel>? listArticles { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
