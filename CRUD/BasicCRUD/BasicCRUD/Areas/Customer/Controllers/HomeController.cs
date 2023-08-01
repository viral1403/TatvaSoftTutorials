using BasicCRUD.DataAccess.Repository.IRepository;
using BasicCRUD.Models;
using BasicCRUD.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Security.Claims;

namespace BasicCRUD.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            IEnumerable<Product> objAllProducts = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
            if(claims != null)
            {
                HttpContext.Session.SetInt32(SD.SessionCartCount, _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claims.Value).ToList().Count);
            }
            return View(objAllProducts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Details(int? productId)
        {
            ShoppingCart shopCart = new()
            {
                Count = 1,
                ProductId = Convert.ToInt32(productId),
                Products = _unitOfWork.Product.FirstOrDefault(u => u.Id == productId, includeProperties:"Category,CoverType")
            };
            return View(shopCart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart objCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            objCart.ApplicationUserId = claims.Value;

            ShoppingCart cartFromDB = _unitOfWork.ShoppingCart.FirstOrDefault(u=>u.ProductId == objCart.ProductId && u.ApplicationUserId == objCart.ApplicationUserId);

            if (cartFromDB != null)
            {
                _unitOfWork.ShoppingCart.IncrementCount(cartFromDB,objCart.Count);
            }
            else
            {
                _unitOfWork.ShoppingCart.Add(objCart);
            }
            _unitOfWork.Save();
            if (claims != null)
            {
                var cartItems = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claims.Value).ToList();
                HttpContext.Session.SetInt32(SD.SessionCartCount, _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claims.Value).ToList().Count);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}