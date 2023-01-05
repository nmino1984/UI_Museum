using Microsoft.AspNetCore.Mvc;
using UI_Museum.Interfaces;
using UI_Museum.Models;

namespace UI_Museum.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        public async Task<IActionResult> Index()
        {
            List<ArticleResponseViewModel> listArticles = await _articleService.ListAllArticles();
            ViewBag.NewTitle = "List of All Articles";

            return View(listArticles);
        }

        public async Task<ActionResult> Article(int articleId)
        {
            ArticleResponseViewModel article = new ArticleResponseViewModel();

            ViewBag.Action = "New Article";

            if (articleId != 0)
            {
                article = await _articleService.GetArticleById(articleId); ;

                ViewBag.Action = "Edit Article";
            }

            return View(article);
        }

        public async Task<ActionResult> Delete(int articleId)
        {
            var response = await _articleService.DeleteArticle(articleId);

            if (response)
            {
                return RedirectToAction("Index");
            }
            else 
            {
                return NoContent();
            }

        }
    }
}
