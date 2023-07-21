using RazorPagesCRUD.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesCRUD.Data;

namespace RazorPagesCRUD.Pages.Categories
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }

        #region get/set properties
        // [BindProperty] - used to bind individual property
        public Category Categories { get; set; }
        #endregion
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if(ModelState.IsValid && (Categories.Name == Categories.DisplayOrder.ToString()))
            {
                ModelState.AddModelError("SimilarValueError", "Display Order and the Category Name must not be exactly same.");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await _db.Categoryies.AddAsync(Categories);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Category added successfully.";
            return RedirectToPage("Index");
        }
    }
}
