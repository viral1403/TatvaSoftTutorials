using BasicCRUD.DataAccess.Repository.IRepository;
using BasicCRUD.Models;
using BasicCRUD.Models.ViewModels;
using BasicCRUD.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;

namespace BasicCRUD.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;

        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public int OrderTotal { get; set; }

        public CartController(IUnitOfWork unitOfWork,IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM ShoppingCartVM = new()
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claims.Value, includeProperties: "Products")
            };

            foreach (var cartItem in ShoppingCartVM.ListCart)
            {
                cartItem.Price = GetPriceBasedOnQuanity(cartItem.Count, cartItem.Products.Price, cartItem.Products.Price50, cartItem.Products.Price100);
                ShoppingCartVM.CartTotal += cartItem.Price * cartItem.Count;
            }
            return View(ShoppingCartVM);
        }

        private double GetPriceBasedOnQuanity(double quantity, double Price,double Price50, double Price100)
        {
            if (quantity < 50)
            {
                return Price;
            }
            else {
                if (quantity <= 100)
                {
                    return Price50;
                }
                return Price100;
            }
        }
        public IActionResult Plus(int cartId)
        {
            var shoppingCart = _unitOfWork.ShoppingCart.FirstOrDefault(u=>u.Id == cartId);
            _unitOfWork.ShoppingCart.IncrementCount(shoppingCart,1);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(int cartId)
        {
            var shoppingCart = _unitOfWork.ShoppingCart.FirstOrDefault(u => u.Id == cartId);
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if(shoppingCart.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(shoppingCart);
            }
            else
            {
                _unitOfWork.ShoppingCart.DecrementCount(shoppingCart, 1);
            }
            _unitOfWork.Save();
            _contextAccessor.HttpContext.Session.SetInt32(SD.SessionCartCount, _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claims.Value).ToList().Count);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int cartId)
        {
            var shoppingCart = _unitOfWork.ShoppingCart.FirstOrDefault(u => u.Id == cartId);
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            
            _unitOfWork.ShoppingCart.Remove(shoppingCart);
            _unitOfWork.Save();
            _contextAccessor.HttpContext.Session.SetInt32(SD.SessionCartCount, _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claims.Value).ToList().Count);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM ShoppingCartVM = new()
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claims.Value, includeProperties: "Products"),
                OrderHeader = new()
            };
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.FirstOrDefault(u => u.Id == claims.Value);
            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;

            foreach (var cartItem in ShoppingCartVM.ListCart)
            {
                cartItem.Price = GetPriceBasedOnQuanity(cartItem.Count, cartItem.Products.Price, cartItem.Products.Price50, cartItem.Products.Price100);
                ShoppingCartVM.CartTotal += cartItem.Price * cartItem.Count;
            }
            return View(ShoppingCartVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public IActionResult SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ApplicationUser appUser = _unitOfWork.ApplicationUser.FirstOrDefault(u => u.Id == claims.Value);

            ShoppingCartVM.ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claims.Value, includeProperties: "Products");
            
            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = claims.Value;

            if (appUser.CompanyId.GetValueOrDefault() == 0)
            {
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            }
            else
            {
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
            }

            foreach (var cartItem in ShoppingCartVM.ListCart)
            {
                cartItem.Price = GetPriceBasedOnQuanity(cartItem.Count, cartItem.Products.Price, cartItem.Products.Price50, cartItem.Products.Price100);
                ShoppingCartVM.OrderHeader.OrderTotal += cartItem.Price * cartItem.Count;
            }
            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Save();

            foreach (var cartItem in ShoppingCartVM.ListCart)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cartItem.ProductId,
                    OrderId = ShoppingCartVM.OrderHeader.Id,
                    Price = cartItem.Price,
                    Count = cartItem.Count
                };
                _unitOfWork.OrderDetails.Add(orderDetail);
                _unitOfWork.Save();
            }

            if (appUser.CompanyId.GetValueOrDefault() == 0)
            {
                var domain = "https://localhost:44317/";
                var options = new SessionCreateOptions
                {
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    SuccessUrl = domain+$"Customer/Cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
                    CancelUrl = domain + $"Customer/Cart/Index",
                };
                var sessionLineItem = new List<SessionLineItemOptions>();
                foreach (var item in ShoppingCartVM.ListCart)
                {
                    sessionLineItem.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price *100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Products.Title,
                            },
                        },
                        Quantity = item.Count,
                    });
                }
                options.LineItems.AddRange(sessionLineItem);

                var service = new SessionService();
                Session session = service.Create(options);

                Response.Headers.Add("Location", session.Url);
                _unitOfWork.OrderHeader.UpdateStripePaymentId(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
                _unitOfWork.Save();
            
                return new StatusCodeResult(303);
            }
            else
            {
                return RedirectToAction("OrderConfirmation", "Cart", new { id = ShoppingCartVM.OrderHeader.Id });
            }

        }
        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.FirstOrDefault(u => u.Id == id);

            if(orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
            {
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);
                // check the stripe status
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                    _unitOfWork.Save();
                }
            }
            HttpContext.Session.Clear();
            
            List<ShoppingCart> shoppingCarts= _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();

            _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
            _unitOfWork.Save();

            return View(id);
        }
    }
}
