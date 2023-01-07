using Microsoft.AspNetCore.Mvc;
using UI_Museum.Interfaces;
using UI_Museum.Models;
using Utiles;

namespace UI_Museum.Controllers
{
    public class MuseumController : Controller
    {
        public readonly IMuseumService _museumService;

        public MuseumController(IMuseumService museumService)
        {
            _museumService = museumService;
        }

        public async Task<IActionResult> Index()
        {
            List<MuseumResponseViewModel> listMuseums = await _museumService.ListAllMuseums();
            ViewBag.NewTitle = "List of All Museums";

            return View(listMuseums);
        }

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

        public async Task<ActionResult> Details(int museumId)
        {
            var museum = new MuseumResponseViewModel();

            ViewBag.Subtitle = "Page to Show Museum’s Details";

            if (museumId != 0)
            {
                museum = await _museumService.GetMuseumById(museumId);

                ViewBag.Action = museum.Name;
            }

            return View(museum);
        }

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

        public async Task<ActionResult> Delete(int museumId)
        {
            var response = await _museumService.DeleteMuseum(museumId);

            if (response)
                return RedirectToAction("Index");
            else
                return NoContent();

        }
    }
}
