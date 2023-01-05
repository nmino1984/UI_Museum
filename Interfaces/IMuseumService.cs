using Microsoft.AspNetCore.Mvc;
using UI_Museum.Models;

namespace UI_Museum.Interfaces
{
    public interface IMuseumService
    {
        Task<List<MuseumResponseViewModel>> ListAllMuseums();
        Task<MuseumResponseViewModel> GetMuseumById(int museumId);
        Task<List<ArticleResponseViewModel>> ListArticlesByMuseum(int museumId);
        Task<bool> RegisterMuseum(MuseumRequestViewModel museum);
        Task<bool> EditMuseum(int museumId, MuseumRequestViewModel museum);
        Task<bool> DeleteMuseum(MuseumRequestViewModel museum);
        Task<bool> RemoveMuseum(MuseumRequestViewModel museum);
    }
}
