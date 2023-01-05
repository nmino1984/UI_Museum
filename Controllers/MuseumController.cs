using Microsoft.AspNetCore.Mvc;
using UI_Museum.Interfaces;

namespace UI_Museum.Controllers
{
    public class MuseumController : Controller
    {
        public readonly IMuseumService _museumService;

        public MuseumController(IMuseumService museumService)
        {
            _museumService = museumService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
