using UI_Museum.Interfaces;
using UI_Museum.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using UI_Museum.Models.Bases;
using System.Text;

namespace UI_Museum.Services
{
    public class ArticleService : IArticleService
    { 
        private static string _baseUrl = "";

        public ArticleService()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

            _baseUrl = builder.GetSection("ApiSettings:baseUrl").Value;
        }

        public async Task<List<ArticleResponseViewModel>> ListAllArticles()
        {
            var listaArticles = new List<ArticleResponseViewModel>();

            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);

            var response = await client.GetAsync("api/Article/All");

            if (response.IsSuccessStatusCode)
            {
                var json_response = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BaseResponse<IEnumerable<ArticleResponseViewModel>>>(json_response);

                listaArticles = result!.Data!.ToList();
            }

            return listaArticles.ToList();

        }

        public async Task<ArticleResponseViewModel> GetArticleById(int articleId)
        {
            var article = new ArticleResponseViewModel();

            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);

            var response = await client.GetAsync($"api/Article/{articleId}");

            if (response.IsSuccessStatusCode)
            {
                var json_response = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BaseResponse<ArticleResponseViewModel>>(json_response);

                article = result!.Data;
            }

            return article!;
        }

        public async Task<List<MuseumSelectResponseViewModel>> GetListSelectMuseums()
        {
            var museums = new List<MuseumSelectResponseViewModel>();

            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);

            var response = await client.GetAsync("api/Museum/Select");

            if (response.IsSuccessStatusCode)
            {
                var json_response = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BaseResponse<IEnumerable<MuseumSelectResponseViewModel>>>(json_response);

                museums = result!.Data!.ToList();
            }

            return museums!;
        }

        public async Task<bool> RegisterArticle(ArticleRequestViewModel article)
        {
            bool answer = false;

            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);

            var content = new StringContent(JsonConvert.SerializeObject(article), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/Article/Register", content);

            if (response.IsSuccessStatusCode)
            {
                answer = true;
            }

            return answer!;
        }

        public async Task<bool> EditArticle(int articleId, ArticleRequestViewModel article)
        {
            bool answer = false;

            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);

            var content = new StringContent(JsonConvert.SerializeObject(article), Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"api/Article/Edit/{articleId}", content);

            if (response.IsSuccessStatusCode)
            {
                answer = true;
            }

            return answer!;
        }

        public async Task<bool> DeleteArticle(int articleId)
        {
            bool answer = false;

            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);

            var response = await client.DeleteAsync($"api/Article/Delete/{articleId}");

            if (response.IsSuccessStatusCode)
            {
                answer = true;
            }

            return answer!;
        }

        /*public async Task<bool> RemoveArticle(int articleId)
        {
            bool answer = false;

            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);

            var response = await client.PutAsync($"api/Article/Remove/{articleId}");

            if (response.IsSuccessStatusCode)
            {
                answer = true;
            }

            return answer!;
        }*/
    }
}
