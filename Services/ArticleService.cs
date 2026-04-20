using UI_Museum.Interfaces;
using UI_Museum.Models;
using Newtonsoft.Json;
using UI_Museum.Models.Bases;
using System.Text;

namespace UI_Museum.Services
{
    /// <summary>
    /// Implements <see cref="IArticleService"/> by calling the Museum REST API over HTTP.
    /// Uses a named <see cref="IHttpClientFactory"/> client ("MuseumApi") whose base address
    /// is configured once in <c>Program.cs</c> from <c>ApiSettings:baseUrl</c>.
    /// </summary>
    public class ArticleService : IArticleService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ArticleService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Calls <c>GET api/Article/All</c> and returns every active article.
        /// Returns an empty list if the request fails.
        /// </summary>
        public async Task<List<ArticleResponseViewModel>> ListAllArticles()
        {
            var listaArticles = new List<ArticleResponseViewModel>();

            var client = _httpClientFactory.CreateClient("MuseumApi");
            var response = await client.GetAsync("api/Article/All");

            if (response.IsSuccessStatusCode)
            {
                var json_response = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BaseResponse<IEnumerable<ArticleResponseViewModel>>>(json_response);

                listaArticles = result!.Data!.ToList();
            }

            return listaArticles;
        }

        /// <summary>
        /// Calls <c>GET api/Article/{articleId}</c> and returns the matching article.
        /// Returns an empty <see cref="ArticleResponseViewModel"/> if the request fails.
        /// </summary>
        /// <param name="articleId">The article identifier.</param>
        public async Task<ArticleResponseViewModel> GetArticleById(int articleId)
        {
            var article = new ArticleResponseViewModel();

            var client = _httpClientFactory.CreateClient("MuseumApi");
            var response = await client.GetAsync($"api/Article/{articleId}");

            if (response.IsSuccessStatusCode)
            {
                var json_response = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BaseResponse<ArticleResponseViewModel>>(json_response);

                article = result!.Data;
            }

            return article!;
        }

        /// <summary>
        /// Calls <c>GET api/Museum/Select</c> and returns a lightweight list of museums
        /// (id + name only) intended for populating dropdown controls.
        /// Returns an empty list if the request fails.
        /// </summary>
        public async Task<List<MuseumSelectResponseViewModel>> GetListSelectMuseums()
        {
            var museums = new List<MuseumSelectResponseViewModel>();

            var client = _httpClientFactory.CreateClient("MuseumApi");
            var response = await client.GetAsync("api/Museum/Select");

            if (response.IsSuccessStatusCode)
            {
                var json_response = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BaseResponse<IEnumerable<MuseumSelectResponseViewModel>>>(json_response);

                museums = result!.Data!.ToList();
            }

            return museums;
        }

        /// <summary>
        /// Calls <c>POST api/Article/Register</c> with the serialized article data.
        /// </summary>
        /// <param name="article">The article data to register.</param>
        /// <returns><c>true</c> if the API returned a success status code; otherwise <c>false</c>.</returns>
        public async Task<bool> RegisterArticle(ArticleRequestViewModel article)
        {
            var client = _httpClientFactory.CreateClient("MuseumApi");
            var content = new StringContent(JsonConvert.SerializeObject(article), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/Article/Register", content);

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Calls <c>PUT api/Article/Edit/{articleId}</c> with the updated article data.
        /// </summary>
        /// <param name="articleId">The identifier of the article to update.</param>
        /// <param name="article">The updated article data.</param>
        /// <returns><c>true</c> if the API returned a success status code; otherwise <c>false</c>.</returns>
        public async Task<bool> EditArticle(int articleId, ArticleRequestViewModel article)
        {
            var client = _httpClientFactory.CreateClient("MuseumApi");
            var content = new StringContent(JsonConvert.SerializeObject(article), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"api/Article/Edit/{articleId}", content);

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Calls <c>DELETE api/Article/Delete/{articleId}</c> to permanently remove an article.
        /// </summary>
        /// <param name="articleId">The identifier of the article to delete.</param>
        /// <returns><c>true</c> if the API returned a success status code; otherwise <c>false</c>.</returns>
        public async Task<bool> DeleteArticle(int articleId)
        {
            var client = _httpClientFactory.CreateClient("MuseumApi");
            var response = await client.DeleteAsync($"api/Article/Delete/{articleId}");

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Calls <c>PUT api/Article/Remove/{articleId}</c> to soft-delete an article
        /// by stamping its <c>DeletedAt</c> field. The record is kept in the database.
        /// </summary>
        /// <param name="articleId">The identifier of the article to soft-delete.</param>
        /// <returns><c>true</c> if the API returned a success status code; otherwise <c>false</c>.</returns>
        public async Task<bool> RemoveArticle(int articleId)
        {
            var client = _httpClientFactory.CreateClient("MuseumApi");
            var response = await client.PutAsync($"api/Article/Remove/{articleId}", new StringContent("", Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Calls <c>POST api/Article/BulkRegister</c> with the full collection serialized as JSON.
        /// All items are validated by the API before any row is written; if one fails the entire batch is rejected.
        /// </summary>
        /// <param name="articles">The collection of articles to create.</param>
        /// <returns><c>true</c> if the API returned a success status code; otherwise <c>false</c>.</returns>
        public async Task<bool> BulkRegisterArticles(IEnumerable<ArticleRequestViewModel> articles)
        {
            var client = _httpClientFactory.CreateClient("MuseumApi");
            var content = new StringContent(JsonConvert.SerializeObject(articles), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/Article/BulkRegister", content);

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Calls <c>PUT api/Article/Relocate/{articleId}</c> with the destination museum id.
        /// Only the <c>IdMuseum</c> foreign key is updated; no other field is modified.
        /// </summary>
        /// <param name="articleId">The identifier of the article to relocate.</param>
        /// <param name="newMuseumId">The identifier of the destination museum.</param>
        /// <returns><c>true</c> if the API returned a success status code; otherwise <c>false</c>.</returns>
        public async Task<bool> RelocateArticle(int articleId, int newMuseumId)
        {
            var client = _httpClientFactory.CreateClient("MuseumApi");
            var body = new { IdMuseum = newMuseumId };
            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"api/Article/Relocate/{articleId}", content);

            return response.IsSuccessStatusCode;
        }
    }
}
