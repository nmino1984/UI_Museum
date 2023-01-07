using Microsoft.AspNetCore.Mvc;
using UI_Museum.Interfaces;
using UI_Museum.Models;
using UI_Museum.Models.Bases;

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
            var article = new ArticleResponseViewModel();

            ViewBag.Subtitle = "Page to Insert a New Article";
            ViewBag.Action = "New Article";
            ViewBag.Museum = await _articleService.GetListSelectMuseums();

            if (articleId != 0)
            {
                article = await _articleService.GetArticleById(articleId);

                ViewBag.Subtitle = "Page to Edit an Article";
                ViewBag.Action = "Edit Article";
            }

            return View(article);
        }

        public async Task<ActionResult> Edit(int articleId)
        {
            bool answer = false;

            var articleToEdit = await _articleService.GetArticleById(articleId);
            ArticleRequestViewModel article = new ArticleRequestViewModel()
            {
                Id = articleId,
                Name = articleToEdit.Name,
                IsDamaged = true,
                IdMuseum = articleToEdit.IdMuseum,
                CreatedAt= articleToEdit.CreatedAt,
                UpdatedAt= DateTime.Now
            };

            answer = await _articleService.EditArticle(articleId, article);

            if (answer)
                return RedirectToAction("Index");
            else
                return NoContent();

        }

        public async Task<ActionResult> RelocateArticle(int articleId)
        {
            var articleToEdit = await _articleService.GetArticleById(articleId);
            var article = await _articleService.GetArticleById(articleId);


            ViewBag.Subtitle = "Page to Relocate an Article from a Museum";
            ViewBag.Action = "Relocate Article";
            ViewBag.Museum = await _articleService.GetListSelectMuseums();

            return View(article);

        }

        public async Task<ActionResult> Save(ArticleRequestViewModel article)
        {
            bool answer = false;

            if (article.Id == 0)
            {
                article.CreatedAt = DateTime.Now;
                answer = await _articleService.RegisterArticle(article);
            }
            else
            {
                article.UpdatedAt = DateTime.Now;
                answer = await _articleService.EditArticle(article.Id, article);
            }

            if (answer)
                return RedirectToAction("Index");
            else
                return NoContent();
        }

        public async Task<ActionResult> Delete(int articleId)
        {
            var response = await _articleService.DeleteArticle(articleId);

            if (response)
                return RedirectToAction("Index");
            else 
                return NoContent();

        }

        public async Task<ActionResult> MarkArticleAsDamaged(int articleId)
        {
            bool answer = false;

            var articleToEdit = await _articleService.GetArticleById(articleId);
            ArticleRequestViewModel article = new ArticleRequestViewModel()
            {
                Id = articleId,
                Name = articleToEdit.Name,
                IsDamaged = true,
                IdMuseum = articleToEdit.IdMuseum,
                CreatedAt = articleToEdit.CreatedAt,
                UpdatedAt = DateTime.Now
            };

            answer = await _articleService.EditArticle(articleId, article);

            if (answer)
                return RedirectToAction("Index");
            else
                return NoContent();
        }

        public async Task<ActionResult> CreateSeveralArticles(int numberOfArticles)
        {
            var article = new ArticleResponseViewModel();

            ViewBag.Subtitle = "Page to Create Several Articles at once";
            ViewBag.Action = "Please, Enter the number of Articles to Create";
            ViewBag.NumberOfArticles = numberOfArticles;

            if (numberOfArticles != 0)
            {
                ViewBag.Museum = await _articleService.GetListSelectMuseums();
            }

            return View(article);
        }
    }
}
