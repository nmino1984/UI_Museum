using Newtonsoft.Json;
using System.Text;
using UI_Museum.Interfaces;
using UI_Museum.Models;
using UI_Museum.Models.Bases;

namespace UI_Museum.Services
{
    public class MuseumService : IMuseumService
    {
        private static string _baseUrl = "";

        public MuseumService()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

            _baseUrl = builder.GetSection("ApiSettings:baseUrl").Value;
        }

        public async Task<List<MuseumResponseViewModel>> ListAllMuseums()
        {
            var listMuseums = new List<MuseumResponseViewModel>();

            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);

            var response = await client.GetAsync("api/Museum/All");

            if (response.IsSuccessStatusCode)
            {
                var json_response = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BaseResponse<IEnumerable<MuseumResponseViewModel>>>(json_response);

                listMuseums = result!.Data!.ToList();
            }

            return listMuseums.ToList();
        }

        public async Task<MuseumResponseViewModel> GetMuseumById(int museumId)
        {
            var museum = new MuseumResponseViewModel();

            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);

            var response = await client.GetAsync($"api/Museum/{museumId}");

            if (response.IsSuccessStatusCode)
            {
                var json_response = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BaseResponse<MuseumResponseViewModel>>(json_response);

                museum = result!.Data;
            }

            return museum!;
        }

        public async Task<List<ArticleResponseViewModel>> ListArticlesByMuseum(int museumId)
        {
            var listArticles = new List<ArticleResponseViewModel>();

            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);

            var response = await client.GetAsync($"api/Museum/ArticlesByMuseum/{museumId}");

            if (response.IsSuccessStatusCode)
            {
                var json_response = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BaseResponse<List<ArticleResponseViewModel>>>(json_response);

                listArticles = result!.Data!.ToList();
            }

            return listArticles.ToList();
        }

        public async Task<List<MuseumResponseViewModel>> GetMuseumsByTheme(int theme)
        {
            var listMuseums = new List<MuseumResponseViewModel>();

            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);

            var response = await client.GetAsync($"api/Museum/GetMuseumsByTheme/{theme}");

            if (response.IsSuccessStatusCode)
            {
                var json_response = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BaseResponse<IEnumerable<MuseumResponseViewModel>>>(json_response);

                listMuseums = result!.Data!.ToList();
            }

            return listMuseums!;
        }

        public async Task<bool> RegisterMuseum(MuseumRequestViewModel museum)
        {
            bool answer = false;

            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);

            var content = new StringContent(JsonConvert.SerializeObject(museum), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/Museum/Register", content);

            if (response.IsSuccessStatusCode)
            {
                answer = true;
            }

            return answer!;
        }

        public async Task<bool> EditMuseum(int museumId, MuseumRequestViewModel museum)
        {
            bool answer = false;

            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);

            var content = new StringContent(JsonConvert.SerializeObject(museum), Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"api/Museum/Edit/{museumId}", content);

            if (response.IsSuccessStatusCode)
            {
                answer = true;
            }

            return answer!;
        }

        public async Task<bool> DeleteMuseum(int museumId)
        {
            bool answer = false;

            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);

            var response = await client.DeleteAsync($"api/Museum/Delete/{museumId}");

            if (response.IsSuccessStatusCode)
            {
                answer = true;
            }

            return answer!;
        }

        /*public Task<bool> RemoveMuseum(MuseumRequestViewModel museum)
        {
            throw new NotImplementedException();
        }*/
    }
}
