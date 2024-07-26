using Microsoft.AspNetCore.Mvc;

namespace JadiPametno.Controllers
{
    public class AdminController : Controller
    {

        private readonly MyDbContext _context;

        public AdminController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("admin/ApproveRecipe")]
        public IActionResult Approve()
        {
            var unapprovedRecipes = _context.Recipe.Where(r => r.Status == 0).ToList();
            TempData["UnapprovedRecipes"] = unapprovedRecipes;
            return View("~/Views/Admin/ApproveRecipe.cshtml");
        }

        [HttpPost]
        [Route("admin/approveRecipe")]
        public IActionResult ApproveRecipe(int recipeId)
        {
            var recipe = _context.Recipe.Find(recipeId);
            if (recipe != null)
            {
                recipe.Status = 1; // Assuming 1 means approved
                _context.SaveChanges();
            }
            return RedirectToAction("Approve");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
