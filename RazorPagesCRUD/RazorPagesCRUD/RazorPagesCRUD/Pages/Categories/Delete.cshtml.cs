using RazorPagesCRUD.Model;
using RazorPagesCRUD.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesCRUD.Pages.Categories
{
    [BindProperties]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }   
        #region get/set properties
        public Category Category { get; set; }
        #endregion
        public void OnGet(int id)
        {
            Category = _db.Categoryies.Find(id);
        }

        public async Task<IActionResult> OnPost()
        {
            var categoryFromDB = _db.Categoryies.Find(Category.Id);
            if (categoryFromDB == null)
            {
                return Page();
            }
            _db.Categoryies.Remove(categoryFromDB);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Category deleted successfully.";
            return RedirectToPage("Index");
        }
    }
}
