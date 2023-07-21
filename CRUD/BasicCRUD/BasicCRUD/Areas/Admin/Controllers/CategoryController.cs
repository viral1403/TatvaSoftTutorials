using Microsoft.AspNetCore.Mvc;
using BasicCRUD.DataAccess;
using BasicCRUD.Models;
using BasicCRUD.DataAccess.Repository.IRepository;

namespace BasicCRUD.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _db;

        public CategoryController(IUnitOfWork db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> objCategoriesList = _db.Category.GetAll();
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
                _db.Category.Add(obj);
                _db.Save();
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
                var categoryObj = _db.Category.FindEntity((int)id);
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
                _db.Category.Update(obj);
                _db.Save();
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
                var CategoryObj = _db.Category.FirstOrDefault(u => u.Id == id);
                if(CategoryObj != null) {
                    _db.Category.Remove(CategoryObj);
                    _db.Save();
                    TempData["Success"] = "Category deleted successfully";
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}
