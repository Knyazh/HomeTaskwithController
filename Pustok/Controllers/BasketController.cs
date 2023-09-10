using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pustok.Contracts;
using Pustok.Database;
using Pustok.Database.Models;
using Pustok.Services.Abstracts;
using Pustok.ViewModels;

namespace Pustok.Controllers;

[Route("basket")]
public class BasketController : Controller
{
    private readonly PustokDbContext _dbContext;
    private readonly IUserService _userService;

    public BasketController(PustokDbContext dbContext, IUserService userService)
    {
        _dbContext = dbContext;
        _userService = userService;
    }

    [HttpPost("add-product/{productId}")]
    public IActionResult AddProduct(AddProductToBasketViewModel model)
    {
        var product = _dbContext.Products.SingleOrDefault(p => p.Id == model.ProductId);
        if (product == null)
        {
            return NotFound();
        }

        var basket = _dbContext.Baskets.SingleOrDefault(b => b.UserId == _userService.CurrentUser.Id);
        if (basket == null)
        {
            basket = new Basket
            {
                UserId = _userService.CurrentUser.Id
            };

            _dbContext.Baskets.Add(basket);
        }

        var productColor = _dbContext.ProductColors
            .FirstOrDefault(pc =>
                pc.ProductId == product.Id
                && (model.ColorId != null ? pc.ColorId == model.ColorId : true));
        if (productColor == null)
        {
            return NotFound();
        }

        var productSize = _dbContext.ProductSizes
            .FirstOrDefault(ps =>
                ps.ProductId == product.Id &&
                model.SizeId != null ? ps.SizeId == model.SizeId : true);
        if (productSize == null)
        {
            return NotFound();
        }

        var basketItem = _dbContext.BasketItems.SingleOrDefault(bi =>
            bi.Basket == basket &&
            bi.ProductId == product.Id &&
            bi.SizeId == productSize.SizeId &&
            bi.ColorId == productColor.ColorId);

        if (basketItem == null)
        {
            basketItem = new BasketItem
            {
                ProductId = product.Id,
                ColorId = productColor.ColorId,
                SizeId = productSize.SizeId,
                Basket = basket,
                Quantity = 1,
            };

            _dbContext.BasketItems.Add(basketItem);
        }
        else
        {
            basketItem.Quantity++;
        }

        _dbContext.SaveChanges();

        return Ok();
    }

    public IActionResult Index()
    {
        return View();
    }
}
