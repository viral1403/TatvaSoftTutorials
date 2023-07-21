using RazorPagesCRUD.Model;
using RazorPagesCRUD.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesCRUD.Pages.Categories
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public EditModel(ApplicationDbContext db)
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
            if(ModelState.IsValid && (Category.Name == Category.DisplayOrder.ToString()))
            {
                ModelState.AddModelError("ErrEditCategory", "Display Order and the  Category Name must not be exactly same.");
            }
            if(!ModelState.IsValid)
            {
                return Page();
            }
            _db.Categoryies.Update(Category);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Category updated successfully.";
            return RedirectToPage("Index");
        }
    }
}
