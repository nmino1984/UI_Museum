using Microsoft.AspNetCore.Mvc;
using UI_Museum.Interfaces;
using UI_Museum.Models;
using UI_Museum.Utiles;
using Utiles;

namespace UI_Museum.Controllers
{
    /// <summary>
    /// Handles all Museum-related views: listing, create/edit form, details, theme filter,
    /// hard-delete and soft-delete.
    /// </summary>
    public class MuseumController : Controller
    {
        public readonly IMuseumService _museumService;

        public MuseumController(IMuseumService museumService)
        {
            _museumService = museumService;
        }

        /// <summary>
        /// Displays the full list of active museums.
        /// Calls <see cref="IMuseumService.ListAllMuseums"/>.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            List<MuseumResponseViewModel> listMuseums = await _museumService.ListAllMuseums();
            ViewBag.NewTitle = "List of All Museums";

            return View(listMuseums);
        }

        /// <summary>
        /// Displays the create/edit form for a museum.
        /// When <paramref name="museumId"/> is 0 the form is shown empty (create mode).
        /// When non-zero the existing museum data is pre-filled (edit mode).
        /// </summary>
        /// <param name="museumId">The museum to edit, or 0 to create a new one.</param>
        public async Task<ActionResult> Museum(int museumId)
        {
            MuseumResponseViewModel museum = new MuseumResponseViewModel();

            ViewBag.Subtitle = "Page to Insert a New Museum";
            ViewBag.Action = "New Museum";

            if (museumId != 0)
            {
                museum = await _museumService.GetMuseumById(museumId); ;

                ViewBag.Subtitle = "Page to Edit an Museum";
                ViewBag.Action = "Edit Museum";
            }

            return View(museum);
        }

        /// <summary>
        /// Displays the read-only detail view for a specific museum.
        /// </summary>
        /// <param name="museumId">The museum identifier.</param>
        public async Task<ActionResult> Details(int museumId)
        {
            var museum = new MuseumResponseViewModel();

            ViewBag.Subtitle = "Page to Show Museum's Details";

            if (museumId != 0)
            {
                museum = await _museumService.GetMuseumById(museumId);

                ViewBag.Action = museum.Name;
            }

            return View(museum);
        }

        /// <summary>
        /// Displays the theme-filter view. When <paramref name="theme"/> is 0 the page is
        /// shown with an empty result set and the theme dropdown pre-populated.
        /// When non-zero the matching museums are fetched and displayed.
        /// </summary>
        /// <param name="theme">Theme identifier: 1 = Art, 2 = Natural Sciences, 3 = History. Pass 0 to show the empty filter page.</param>
        public async Task<ActionResult> MuseumsByTheme(int theme)
        {
            var museums = new List<MuseumResponseViewModel>();

            ViewBag.Subtitle = "Page to Get All Museum's By Theme";
            ViewBag.ListadoThemes = Utiles.Utiles.ToListSelectListItem<Themes>();
            ViewBag.NumberOfArticles = 0;

            if (theme != 0)
            {
                museums = await _museumService.GetMuseumsByTheme(theme);

                ViewBag.Action = "Museum's By Theme: " + (Themes)theme;
            }

            return View(museums);
        }

        /// <summary>
        /// Persists a museum. When <c>museum.Id</c> is 0 a new museum is registered;
        /// otherwise the existing museum is updated.
        /// Redirects to <see cref="Index"/> on success, or returns 204 on failure.
        /// </summary>
        /// <param name="museum">The form data submitted from the Museum view.</param>
        public async Task<ActionResult> Save(MuseumRequestViewModel museum)
        {
            bool answer = false;

            if (museum.Id == 0)
                answer = await _museumService.RegisterMuseum(museum);
            else
                answer = await _museumService.EditMuseum(museum.Id, museum);

            if (answer)
                return RedirectToAction("Index");
            else
                return NoContent();
        }

        /// <summary>
        /// Permanently deletes a museum. The API rejects this call if the museum still
        /// has active articles. Redirects to <see cref="Index"/> on success, or returns 204 on failure.
        /// </summary>
        /// <param name="museumId">The identifier of the museum to delete.</param>
        public async Task<ActionResult> Delete(int museumId)
        {
            var response = await _museumService.DeleteMuseum(museumId);

            if (response)
                return RedirectToAction("Index");
            else
                return NoContent();
        }

        /// <summary>
        /// Soft-deletes a museum and all of its active articles by stamping their
        /// <c>DeletedAt</c> fields. Records remain in the database.
        /// Redirects to <see cref="Index"/> on success, or returns 204 on failure.
        /// </summary>
        /// <param name="museumId">The identifier of the museum to soft-delete.</param>
        public async Task<ActionResult> Remove(int museumId)
        {
            var response = await _museumService.RemoveMuseum(museumId);

            if (response)
                return RedirectToAction("Index");
            else
                return NoContent();
        }
    }
}
