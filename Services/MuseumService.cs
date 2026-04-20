using Newtonsoft.Json;
using System.Text;
using UI_Museum.Interfaces;
using UI_Museum.Models;
using UI_Museum.Models.Bases;

namespace UI_Museum.Services
{
    /// <summary>
    /// Implements <see cref="IMuseumService"/> by calling the Museum REST API over HTTP.
    /// Uses a named <see cref="IHttpClientFactory"/> client ("MuseumApi") whose base address
    /// is configured once in <c>Program.cs</c> from <c>ApiSettings:baseUrl</c>.
    /// </summary>
    public class MuseumService : IMuseumService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MuseumService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Calls <c>GET api/Museum/All</c> and returns every active museum.
        /// Returns an empty list if the request fails.
        /// </summary>
        public async Task<List<MuseumResponseViewModel>> ListAllMuseums()
        {
            var listMuseums = new List<MuseumResponseViewModel>();

            var client = _httpClientFactory.CreateClient("MuseumApi");
            var response = await client.GetAsync("api/Museum/All");

            if (response.IsSuccessStatusCode)
            {
                var json_response = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BaseResponse<IEnumerable<MuseumResponseViewModel>>>(json_response);

                listMuseums = result!.Data!.ToList();
            }

            return listMuseums;
        }

        /// <summary>
        /// Calls <c>GET api/Museum/{museumId}</c> and returns the matching museum.
        /// Returns an empty <see cref="MuseumResponseViewModel"/> if the request fails.
        /// </summary>
        /// <param name="museumId">The museum identifier.</param>
        public async Task<MuseumResponseViewModel> GetMuseumById(int museumId)
        {
            var museum = new MuseumResponseViewModel();

            var client = _httpClientFactory.CreateClient("MuseumApi");
            var response = await client.GetAsync($"api/Museum/{museumId}");

            if (response.IsSuccessStatusCode)
            {
                var json_response = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BaseResponse<MuseumResponseViewModel>>(json_response);

                museum = result!.Data;
            }

            return museum!;
        }

        /// <summary>
        /// Calls <c>GET api/Museum/ArticlesByMuseum/{museumId}</c> and returns the articles
        /// that belong to the specified museum. Returns an empty list if the request fails.
        /// </summary>
        /// <param name="museumId">The museum identifier.</param>
        public async Task<List<ArticleResponseViewModel>> ListArticlesByMuseum(int museumId)
        {
            var listArticles = new List<ArticleResponseViewModel>();

            var client = _httpClientFactory.CreateClient("MuseumApi");
            var response = await client.GetAsync($"api/Museum/ArticlesByMuseum/{museumId}");

            if (response.IsSuccessStatusCode)
            {
                var json_response = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BaseResponse<List<ArticleResponseViewModel>>>(json_response);

                listArticles = result!.Data!.ToList();
            }

            return listArticles;
        }

        /// <summary>
        /// Calls <c>GET api/Museum/GetMuseumsByTheme/{theme}</c> and returns museums
        /// filtered by the given theme. Returns an empty list if the request fails.
        /// </summary>
        /// <param name="theme">Theme identifier: 1 = Art, 2 = Natural Sciences, 3 = History.</param>
        public async Task<List<MuseumResponseViewModel>> GetMuseumsByTheme(int theme)
        {
            var listMuseums = new List<MuseumResponseViewModel>();

            var client = _httpClientFactory.CreateClient("MuseumApi");
            var response = await client.GetAsync($"api/Museum/GetMuseumsByTheme/{theme}");

            if (response.IsSuccessStatusCode)
            {
                var json_response = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BaseResponse<IEnumerable<MuseumResponseViewModel>>>(json_response);

                listMuseums = result!.Data!.ToList();
            }

            return listMuseums;
        }

        /// <summary>
        /// Calls <c>POST api/Museum/Register</c> with the serialized museum data.
        /// </summary>
        /// <param name="museum">The museum data to register.</param>
        /// <returns><c>true</c> if the API returned a success status code; otherwise <c>false</c>.</returns>
        public async Task<bool> RegisterMuseum(MuseumRequestViewModel museum)
        {
            var client = _httpClientFactory.CreateClient("MuseumApi");
            var content = new StringContent(JsonConvert.SerializeObject(museum), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/Museum/Register", content);

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Calls <c>PUT api/Museum/Edit/{museumId}</c> with the updated museum data.
        /// </summary>
        /// <param name="museumId">The identifier of the museum to update.</param>
        /// <param name="museum">The updated museum data.</param>
        /// <returns><c>true</c> if the API returned a success status code; otherwise <c>false</c>.</returns>
        public async Task<bool> EditMuseum(int museumId, MuseumRequestViewModel museum)
        {
            var client = _httpClientFactory.CreateClient("MuseumApi");
            var content = new StringContent(JsonConvert.SerializeObject(museum), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"api/Museum/Edit/{museumId}", content);

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Calls <c>DELETE api/Museum/Delete/{museumId}</c> to permanently remove a museum.
        /// The API will reject this call (400) if the museum still has active articles.
        /// </summary>
        /// <param name="museumId">The identifier of the museum to delete.</param>
        /// <returns><c>true</c> if the API returned a success status code; otherwise <c>false</c>.</returns>
        public async Task<bool> DeleteMuseum(int museumId)
        {
            var client = _httpClientFactory.CreateClient("MuseumApi");
            var response = await client.DeleteAsync($"api/Museum/Delete/{museumId}");

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Calls <c>PUT api/Museum/Remove/{museumId}</c> to soft-delete a museum.
        /// The API cascades the soft-delete to all active articles in that museum.
        /// </summary>
        /// <param name="museumId">The identifier of the museum to soft-delete.</param>
        /// <returns><c>true</c> if the API returned a success status code; otherwise <c>false</c>.</returns>
        public async Task<bool> RemoveMuseum(int museumId)
        {
            var client = _httpClientFactory.CreateClient("MuseumApi");
            var response = await client.PutAsync($"api/Museum/Remove/{museumId}", new StringContent("", Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }
    }
}
