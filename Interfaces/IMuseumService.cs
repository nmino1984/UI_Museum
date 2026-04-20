using Microsoft.AspNetCore.Mvc;
using UI_Museum.Models;

namespace UI_Museum.Interfaces
{
    /// <summary>
    /// Defines the contract for museum-related HTTP operations against the Museum API.
    /// </summary>
    public interface IMuseumService
    {
        /// <summary>
        /// Retrieves all active (non-deleted) museums.
        /// </summary>
        Task<List<MuseumResponseViewModel>> ListAllMuseums();

        /// <summary>
        /// Retrieves a single museum by its identifier.
        /// </summary>
        /// <param name="museumId">The museum identifier.</param>
        Task<MuseumResponseViewModel> GetMuseumById(int museumId);

        /// <summary>
        /// Retrieves all active articles that belong to the specified museum.
        /// </summary>
        /// <param name="museumId">The museum identifier.</param>
        Task<List<ArticleResponseViewModel>> ListArticlesByMuseum(int museumId);

        /// <summary>
        /// Retrieves all active museums that match the given theme.
        /// </summary>
        /// <param name="theme">Theme identifier: 1 = Art, 2 = Natural Sciences, 3 = History.</param>
        Task<List<MuseumResponseViewModel>> GetMuseumsByTheme(int theme);

        /// <summary>
        /// Creates a new museum by posting the request data to the API.
        /// </summary>
        /// <param name="museum">The museum data to register.</param>
        /// <returns><c>true</c> if the API returned a success status code; otherwise <c>false</c>.</returns>
        Task<bool> RegisterMuseum(MuseumRequestViewModel museum);

        /// <summary>
        /// Updates an existing museum with the supplied data.
        /// </summary>
        /// <param name="museumId">The identifier of the museum to update.</param>
        /// <param name="museum">The updated museum data.</param>
        /// <returns><c>true</c> if the API returned a success status code; otherwise <c>false</c>.</returns>
        Task<bool> EditMuseum(int museumId, MuseumRequestViewModel museum);

        /// <summary>
        /// Permanently deletes a museum from the database.
        /// The API blocks this operation if the museum has active articles.
        /// </summary>
        /// <param name="museumId">The identifier of the museum to delete.</param>
        /// <returns><c>true</c> if the API returned a success status code; otherwise <c>false</c>.</returns>
        Task<bool> DeleteMuseum(int museumId);

        /// <summary>
        /// Soft-deletes a museum by stamping its <c>DeletedAt</c> field.
        /// The record is kept in the database and all its active articles are soft-deleted as well.
        /// </summary>
        /// <param name="museumId">The identifier of the museum to soft-delete.</param>
        /// <returns><c>true</c> if the API returned a success status code; otherwise <c>false</c>.</returns>
        Task<bool> RemoveMuseum(int museumId);
    }
}
