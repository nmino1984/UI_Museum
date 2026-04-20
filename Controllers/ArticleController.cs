using Microsoft.AspNetCore.Mvc;
using UI_Museum.Interfaces;
using UI_Museum.Models;
using UI_Museum.Models.Bases;

namespace UI_Museum.Controllers
{
    /// <summary>
    /// Handles all Article-related views: listing, create/edit form, bulk creation,
    /// relocation, mark-as-damaged, hard-delete and soft-delete.
    /// </summary>
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        /// <summary>
        /// Displays the full list of active articles.
        /// Calls <see cref="IArticleService.ListAllArticles"/>.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            List<ArticleResponseViewModel> listArticles = await _articleService.ListAllArticles();
            ViewBag.NewTitle = "List of All Articles";

            return View(listArticles);
        }

        /// <summary>
        /// Displays the create/edit form for a single article.
        /// When <paramref name="articleId"/> is 0 the form is shown empty (create mode).
        /// When non-zero the existing article data is pre-filled (edit mode).
        /// The museum dropdown is always populated via <see cref="IArticleService.GetListSelectMuseums"/>.
        /// </summary>
        /// <param name="articleId">The article to edit, or 0 to create a new one.</param>
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

        /// <summary>
        /// Redirects to the <see cref="Article"/> action (the create/edit form) for the given article.
        /// Acts as a canonical edit entry point so that links using the word "Edit" resolve correctly.
        /// </summary>
        /// <param name="articleId">The identifier of the article to edit.</param>
        public IActionResult Edit(int articleId) => RedirectToAction("Article", new { articleId });

        /// <summary>
        /// Displays the relocation form for a specific article, pre-loading the museum
        /// dropdown so the user can pick a new destination.
        /// </summary>
        /// <param name="articleId">The identifier of the article to relocate.</param>
        public async Task<ActionResult> RelocateArticle(int articleId)
        {
            var article = await _articleService.GetArticleById(articleId);

            ViewBag.Subtitle = "Page to Relocate an Article from a Museum";
            ViewBag.Action = "Relocate Article";
            ViewBag.Museum = await _articleService.GetListSelectMuseums();

            return View(article);
        }

        /// <summary>
        /// Processes the relocation form submission by calling
        /// <see cref="IArticleService.RelocateArticle"/> with the article id and the
        /// new museum id. Only the museum foreign key is updated on the API side.
        /// Redirects to <see cref="Index"/> on success, or returns 204 on failure.
        /// </summary>
        /// <param name="Id">The identifier of the article to relocate.</param>
        /// <param name="IdMuseum">The identifier of the destination museum.</param>
        [HttpPost]
        public async Task<ActionResult> SaveRelocate(int Id, int IdMuseum)
        {
            var response = await _articleService.RelocateArticle(Id, IdMuseum);

            if (response)
                return RedirectToAction("Index");
            else
                return NoContent();
        }

        /// <summary>
        /// Persists an article. When <c>article.Id</c> is 0 a new article is registered
        /// (sets <c>CreatedAt</c>); otherwise the existing article is updated (sets <c>UpdatedAt</c>).
        /// Redirects to <see cref="Index"/> on success, or returns 204 on failure.
        /// </summary>
        /// <param name="article">The form data submitted from the Article view.</param>
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

        /// <summary>
        /// Permanently deletes an article from the database.
        /// Redirects to <see cref="Index"/> on success, or returns 204 on failure.
        /// </summary>
        /// <param name="articleId">The identifier of the article to delete.</param>
        public async Task<ActionResult> Delete(int articleId)
        {
            var response = await _articleService.DeleteArticle(articleId);

            if (response)
                return RedirectToAction("Index");
            else
                return NoContent();
        }

        /// <summary>
        /// Marks an article as damaged by setting <c>IsDamaged = true</c> and updating
        /// the <c>UpdatedAt</c> timestamp. All other fields are preserved from the current
        /// database record. Redirects to <see cref="Index"/> on success, or returns 204 on failure.
        /// </summary>
        /// <param name="articleId">The identifier of the article to mark as damaged.</param>
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

        /// <summary>
        /// Displays the bulk-creation wizard. When <paramref name="numberOfArticles"/> is 0
        /// only the quantity input is shown. When non-zero the page renders that many
        /// article forms and populates the museum dropdown.
        /// </summary>
        /// <param name="numberOfArticles">The number of article forms to render, or 0 for the initial step.</param>
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

        /// <summary>
        /// Processes the bulk-creation form submission. Sets <c>CreatedAt</c> on every
        /// item and calls <see cref="IArticleService.BulkRegisterArticles"/>.
        /// The API validates all items before writing any row — one failure rejects the whole batch.
        /// Redirects to <see cref="Index"/> on success, or returns 204 on failure.
        /// </summary>
        /// <param name="articles">The list of articles submitted from the bulk-creation form.</param>
        [HttpPost]
        public async Task<ActionResult> CreateMultipleItems(List<ArticleRequestViewModel> articles)
        {
            foreach (var article in articles)
            {
                article.CreatedAt = DateTime.Now;
            }

            var response = await _articleService.BulkRegisterArticles(articles);

            if (response)
                return RedirectToAction("Index");
            else
                return NoContent();
        }

        /// <summary>
        /// Soft-deletes an article by stamping its <c>DeletedAt</c> field.
        /// The record is kept in the database and can be recovered if needed.
        /// Redirects to <see cref="Index"/> on success, or returns 204 on failure.
        /// </summary>
        /// <param name="articleId">The identifier of the article to soft-delete.</param>
        public async Task<ActionResult> Remove(int articleId)
        {
            var response = await _articleService.RemoveArticle(articleId);

            if (response)
                return RedirectToAction("Index");
            else
                return NoContent();
        }
    }
}
