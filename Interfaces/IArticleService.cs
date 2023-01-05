using UI_Museum.Models;

namespace UI_Museum.Interfaces
{
    public interface IArticleService
    {
        Task<List<ArticleResponseViewModel>> ListAllArticles();
        Task<ArticleResponseViewModel> GetArticleById(int articleId);
        Task<bool> RegisterArticle(ArticleRequestViewModel article);
        Task<bool> EditArticle(int articleId, ArticleRequestViewModel article);
        Task<bool> DeleteArticle(int articleId);
        //Task<bool> RemoveArticle(int articleId);

    }
}
