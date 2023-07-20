using Microsoft.AspNetCore.Mvc;
using BasicCRUD.Data;
using BasicCRUD.Models;

namespace BasicCRUD.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> objCategoriesList = _db.Categories.ToList();
            return View(objCategoriesList);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category obj)
        {
            try { 
                if(obj.Name == obj.DisplayOrder.ToString() && !string.IsNullOrEmpty(obj.Name))
                {
                    ModelState.AddModelError("CustomErr", "The DisplayOrder cannot exactly match the Name.");
                }
                if (!ModelState.IsValid)
                {
                    return View(obj);
                }
                obj.CreatedDate = DateTime.Now;
                _db.Categories.Add(obj);
                _db.SaveChanges();
                TempData["Success"] = "Category saved successfully";
                return RedirectToAction("Index");
            }
            catch
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int ?id)
        {
            if(id != 0 && id != null)
            {
                var categoryObj = _db.Categories.Find(id);
                if(categoryObj != null)
                {
                    return View(categoryObj);
                }
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category obj)
        {
            try { 
                if (obj.Name == obj.DisplayOrder.ToString() && !string.IsNullOrEmpty(obj.Name))
                {
                    ModelState.AddModelError("CustomErr", "The DisplayOrder cannot exactly match the Name.");
                }
                if (!ModelState.IsValid)
                {
                    return View(obj);
                }
                obj.ModifiedDate = DateTime.Now;
                _db.Categories.Update(obj);
                _db.SaveChanges();
                TempData["Success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != 0 && id != null)
            {
                var CategoryObj = _db.Categories.Find(id);
                if(CategoryObj != null) {
                    _db.Categories.Remove(CategoryObj);
                    _db.SaveChanges();
                    TempData["Success"] = "Category deleted successfully";
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}
