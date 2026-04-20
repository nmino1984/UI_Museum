using UI_Museum.Models;

namespace UI_Museum.Interfaces
{
    /// <summary>
    /// Defines the contract for article-related HTTP operations against the Museum API.
    /// </summary>
    public interface IArticleService
    {
        /// <summary>
        /// Retrieves all active (non-deleted) articles.
        /// </summary>
        Task<List<ArticleResponseViewModel>> ListAllArticles();

        /// <summary>
        /// Retrieves a single article by its identifier.
        /// </summary>
        /// <param name="articleId">The article identifier.</param>
        Task<ArticleResponseViewModel> GetArticleById(int articleId);

        /// <summary>
        /// Retrieves a lightweight list of museums (id + name) used to populate dropdown controls.
        /// </summary>
        Task<List<MuseumSelectResponseViewModel>> GetListSelectMuseums();

        /// <summary>
        /// Creates a new article by posting the request data to the API.
        /// </summary>
        /// <param name="article">The article data to register.</param>
        /// <returns><c>true</c> if the API returned a success status code; otherwise <c>false</c>.</returns>
        Task<bool> RegisterArticle(ArticleRequestViewModel article);

        /// <summary>
        /// Updates an existing article with the supplied data.
        /// </summary>
        /// <param name="articleId">The identifier of the article to update.</param>
        /// <param name="article">The updated article data.</param>
        /// <returns><c>true</c> if the API returned a success status code; otherwise <c>false</c>.</returns>
        Task<bool> EditArticle(int articleId, ArticleRequestViewModel article);

        /// <summary>
        /// Permanently deletes an article from the database.
        /// </summary>
        /// <param name="articleId">The identifier of the article to delete.</param>
        /// <returns><c>true</c> if the API returned a success status code; otherwise <c>false</c>.</returns>
        Task<bool> DeleteArticle(int articleId);

        /// <summary>
        /// Soft-deletes an article by stamping its <c>DeletedAt</c> field.
        /// The record is kept in the database and can be recovered if needed.
        /// </summary>
        /// <param name="articleId">The identifier of the article to soft-delete.</param>
        /// <returns><c>true</c> if the API returned a success status code; otherwise <c>false</c>.</returns>
        Task<bool> RemoveArticle(int articleId);

        /// <summary>
        /// Creates multiple articles in a single atomic request.
        /// All items are validated before any row is written; if one fails the entire batch is rejected.
        /// </summary>
        /// <param name="articles">The collection of articles to create.</param>
        /// <returns><c>true</c> if the API returned a success status code; otherwise <c>false</c>.</returns>
        Task<bool> BulkRegisterArticles(IEnumerable<ArticleRequestViewModel> articles);

        /// <summary>
        /// Moves an article to a different museum by updating its foreign key.
        /// Only the museum assignment changes; no other field is modified.
        /// </summary>
        /// <param name="articleId">The identifier of the article to relocate.</param>
        /// <param name="newMuseumId">The identifier of the destination museum.</param>
        /// <returns><c>true</c> if the API returned a success status code; otherwise <c>false</c>.</returns>
        Task<bool> RelocateArticle(int articleId, int newMuseumId);
    }
}
