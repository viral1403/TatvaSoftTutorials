using BasicCRUD.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using BasicCRUD.Models;
using Microsoft.AspNetCore.Authorization;
using BasicCRUD.Utility;

namespace BasicCRUD.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _db;

        public CoverTypeController(IUnitOfWork db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<CoverType> objCategoriesList = _db.CoverType.GetAll();
            return View(objCategoriesList);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CoverType obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(obj);
                }
                _db.CoverType.Add(obj);
                _db.Save();
                TempData["Success"] = "Cover Type saved successfully";
                return RedirectToAction("Index");
            }
            catch
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != 0 && id != null)
            {
                var CoverTypeObj = _db.CoverType.FindEntity((int)id);
                if (CoverTypeObj != null)
                {
                    return View(CoverTypeObj);
                }
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CoverType obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(obj);
                }
                _db.CoverType.Update(obj);
                _db.Save();
                TempData["Success"] = "Cover Type updated successfully";
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
                var CoverTypeObj = _db.CoverType.FirstOrDefault(u => u.Id == id);
                if (CoverTypeObj != null)
                {
                    _db.CoverType.Remove(CoverTypeObj);
                    _db.Save();
                    TempData["Success"] = "Cover Type deleted successfully";
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}
