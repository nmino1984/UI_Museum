using UI_Museum.Interfaces;
using UI_Museum.Models;

namespace UI_Museum.Services
{
    public class MuseumService : IMuseumService
    {
        public Task<List<MuseumResponseViewModel>> ListAllMuseums()
        {
            throw new NotImplementedException();
        }

        public Task<MuseumResponseViewModel> GetMuseumById(int museumId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ArticleResponseViewModel>> ListArticlesByMuseum(int museumId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RegisterMuseum(MuseumRequestViewModel museum)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditMuseum(int museumId, MuseumRequestViewModel museum)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteMuseum(MuseumRequestViewModel museum)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveMuseum(MuseumRequestViewModel museum)
        {
            throw new NotImplementedException();
        }
    }
}
