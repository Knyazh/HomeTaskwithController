using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Contracts;
using Pustok.Database;
using Pustok.Database.Models;
using Pustok.Services;
using Pustok.Services.Concretes;
using Pustok.ViewModels;

namespace Pustok.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly UserService _userService;
        private readonly PustokDbContext _pustokDbContext;    
        private readonly OrderIdentifierGenerator _orderIdentifierGenerator;

        public CheckOutController(UserService userService, PustokDbContext pustokDbContext, OrderIdentifierGenerator orderIdentifierGenerator)
        {
            _userService = userService;
            _pustokDbContext = pustokDbContext;
            _orderIdentifierGenerator = orderIdentifierGenerator;
        }


        [HttpGet]
        public IActionResult Index()
        {


            var user = _userService.CurrentUser;
            var cartViewModel = new CartViewModel();

            var basketItems = _pustokDbContext.BasketItems
                .Where(p => p.Basket.UserId == user.Id && p.IsOrdered == false)
                .Select(p => new CartViewModel.BasketItemViewModel
                {
                    ProductId = p.ProductId,
                    ColorName = p.Color.Name,
                    ProductName = p.Product.Name,
                    SizeName = p.Size.Name,
                    ProductPrice = p.Product.Price,
                    ProductQuantity = p.Quantity,
                    TotalProductPrice = ((double)(p.Quantity * p.Product.Price))
                })
                .ToList();

            cartViewModel.BasketItems = basketItems;
            cartViewModel.Total = basketItems.Sum(t => t.ProductPrice * t.ProductQuantity);

            return View(cartViewModel);
        }

        [HttpPost]
        public IActionResult AddOrder()
        {
            var user = _userService.CurrentUser;

            Order order = new()
            {
                UserId = user.Id,
                OrderItemStatusValue = StatusOrderItem.OrderItemStatusValue.Created,
                TracingCode = _orderIdentifierGenerator.GenerateOrderIdentifier(),
            };

            _pustokDbContext.Orders.Add(order);

            var orderedBasketItems = _pustokDbContext.BasketItems
                .Include(bi => bi.Color)
                .Where(bi => bi.Basket.UserId == user.Id && bi.IsOrdered == false);

            List<OrderItem> orderItems =
               orderedBasketItems
                .Select(bi => new OrderItem
                {
                    OrderName = bi.Product.Name,
                    OrderColor = bi.Color.Name,
                    OrderSizes = bi.Size.Name,
                    BasketItem = bi,
                    Order = order,
                    OrderPhoto = bi.Product.Image,
                    OrderPrice = bi.Product.Price,
                    OrderQuantity = bi.Quantity,
                    OrderDescription = bi.Product.Description,

                }).ToList();

            _pustokDbContext.OrderItems.AddRange(orderItems);

            foreach (var item in orderedBasketItems)
            {
                item.IsOrdered = true;
            }

            _pustokDbContext.SaveChanges();
            return RedirectToAction("");
        }
    }
}
