using BasicCRUD.DataAccess.Repository.IRepository;
using BasicCRUD.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
            IEnumerable<Product> objAllProducts = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
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
        public IActionResult Details(int? id)
        {
            ShoppingCart shopCart = new()
            {
                Count = 1,
                Product = _unitOfWork.Product.FirstOrDefault(u => u.Id == id,includeProperties:"Category,CoverType")
            };
            return View(shopCart);
        }
    }
}