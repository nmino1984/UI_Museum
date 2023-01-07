using Microsoft.AspNetCore.Mvc;
using UI_Museum.Models;

namespace UI_Museum.Interfaces
{
    public interface IMuseumService
    {
        Task<List<MuseumResponseViewModel>> ListAllMuseums();
        Task<MuseumResponseViewModel> GetMuseumById(int museumId);
        Task<List<ArticleResponseViewModel>> ListArticlesByMuseum(int museumId);
        Task<List<MuseumResponseViewModel>> GetMuseumsByTheme(int theme);
        Task<bool> RegisterMuseum(MuseumRequestViewModel museum);
        Task<bool> EditMuseum(int museumId, MuseumRequestViewModel museum);
        Task<bool> DeleteMuseum(int museumId);
        //Task<bool> RemoveMuseum(MuseumRequestViewModel museum);
    }
}
