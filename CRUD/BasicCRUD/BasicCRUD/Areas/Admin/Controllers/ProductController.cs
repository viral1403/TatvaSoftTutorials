using Microsoft.AspNetCore.Mvc;
using BasicCRUD.DataAccess;
using BasicCRUD.Models;
using BasicCRUD.DataAccess.Repository.IRepository;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using BasicCRUD.Models.ViewModels;
using BasicCRUD.Utility;
using Microsoft.AspNetCore.Authorization;

namespace BasicCRUD.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IWebHostEnvironment _host;

        public ProductController(IUnitOfWork unitofwork, IWebHostEnvironment host)
        {
            _unitofwork = unitofwork;
            _host = host;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> objProductList = _unitofwork.Product.GetAll();
            return View(objProductList);
        }

        [HttpGet]
        public IActionResult Upsert(int ?id)
        {
            ProductVM productVM = new()
            {
                Product = !(id != 0 && id != null) ? new() : _unitofwork.Product.FirstOrDefault(u=> u.Id == id),
                CategoryList = _unitofwork.Category.GetAll().Select(
                                    u => new SelectListItem
                                    {
                                        Text = u.Name,
                                        Value = u.Id.ToString()
                                    }),
                CoverTypeList = _unitofwork.CoverType.GetAll().Select(
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };

            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            try { 
                if (!ModelState.IsValid)
                {
                    return View(obj);
                }

                string wwwRootPath = _host.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads =  Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    if (!string.IsNullOrEmpty(obj.Product.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath,obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath)){
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    
                    using(var fileStreams = new FileStream(Path.Combine(uploads,fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Product.ImageUrl = @"\images\products\" + fileName + extension;
                }
                if (obj.Product.Id != 0)
                {
                    _unitofwork.Product.Update(obj.Product);
                    TempData["Success"] = "Product updated successfully";
                }
                else
                {
                    _unitofwork.Product.Add(obj.Product);
                    TempData["Success"] = "Product saved successfully";
                }
                
                _unitofwork.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                throw;
            }
        }


        #region API Calls
        [HttpGet]
        public JsonResult GetAllProducts()
        {
            var productsList = _unitofwork.Product.GetAll(includeProperties:"Category,CoverType");
            return Json(new { data = productsList });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productObj = _unitofwork.Product.FirstOrDefault(u => u.Id == id);
            if(productObj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            var oldImagePath = Path.Combine(_host.WebRootPath,productObj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitofwork.Product.Remove(productObj);
            _unitofwork.Save();
            return Json(new { success = true, message = "Product deleted successfully." });
        }
        #endregion
    }
}
