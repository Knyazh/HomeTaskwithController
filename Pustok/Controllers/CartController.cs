using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pustok.Database;
using Pustok.Services.Concretes;
using Pustok.ViewModels;

namespace Pustok.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly PustokDbContext _pustokDbContext;
        private readonly UserService _userService;

        public CartController(PustokDbContext appDbContext, UserService userService)
        {
            _pustokDbContext = appDbContext;
            _userService = userService;
        }


        [HttpGet]
        public IActionResult Index()
        {
            var user = _userService.CurrentUser;
            var cartViewModel = new CartViewModel();

            var basketItems = _pustokDbContext.BasketItems
                .Where(o => o.Basket.UserId == user.Id && o.IsOrdered == false)
                .Select(o => new CartViewModel.BasketItemViewModel
                {
                    ProductId = o.ProductId,
                    ColorName = o.Color.Name,
                    ProductName = o.Product.Name,
                    SizeName = o.Size.Name,
                    ProductPrice = o.Product.Price,
                    ProductQuantity = o.Quantity,
                    TotalProductPrice = ((double)(o.Quantity * o.Product.Price))
                })
                .ToList();

            cartViewModel.BasketItems = basketItems;
            cartViewModel.Total = basketItems.Sum(p => p.ProductPrice * p.ProductQuantity);

            return View(cartViewModel);
        }
    }
}
