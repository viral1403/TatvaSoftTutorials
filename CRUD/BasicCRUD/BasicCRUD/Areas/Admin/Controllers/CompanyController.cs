using BasicCRUD.DataAccess.Repository.IRepository;
using BasicCRUD.Models;
using BasicCRUD.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasicCRUD.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            Company objCompany = (id == 0 || id== null) ? new() : _unitOfWork.Company.FirstOrDefault(u=>u.Id == id);
            return View(objCompany);
        }

        [HttpPost]
        public IActionResult Upsert(Company obj)
        {
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            if(obj.Id != 0)
            {
                _unitOfWork.Company.Update(obj);
                TempData["Success"] = "Company updated successsfully";
            }
            else
            {
                _unitOfWork.Company.Add(obj);
                TempData["Success"] = "Company saved successsfully";
            }
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        #region API calls
        public JsonResult GetCompanies()
        {
            IEnumerable<Company> companies = _unitOfWork.Company.GetAll();
            return Json(new { data = companies});
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var companyObj = _unitOfWork.Company.FirstOrDefault(u => u.Id == id);
            if (companyObj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Company.Remove(companyObj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Company deleted successfully." });
        }
        #endregion
    }
}
