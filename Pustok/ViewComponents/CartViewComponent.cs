using Microsoft.AspNetCore.Mvc;
using Pustok.Database;
using Pustok.Services.Abstracts;
using Pustok.Services.Concretes;
using Pustok.ViewModels;

namespace Pustok.ViewComponents;

public class CartViewComponent : ViewComponent
{
    private readonly IFileService _fileService;
    private readonly PustokDbContext _pustokDbContext;
    private readonly IUserService _userService;


    public CartViewComponent(IFileService fileService, PustokDbContext pustokDbContext, IUserService userService)
    {
        _fileService = fileService;
        _pustokDbContext = pustokDbContext;
        _userService = userService;
    }

    public IViewComponentResult Invoke()
    {
        if (!_userService.IsCurrentUserAuthenticated())
        {
            return View(new CartViewModel());
        }

        var model = new CartViewModel
        {
            BasketItems = _pustokDbContext.BasketItems
                .Where(bi => bi.Basket.UserId == _userService.CurrentUser.Id)
                .Select(bi => new CartViewModel.BasketItemViewModel
                {
                    ProductName = bi.Product.Name,
                    ImageUrl = _fileService
                    .GetStaticFilesUrl(Contracts.CustomUploadDirectories.Products, bi.Product.PhysicalImageName),
                    ProductPrice = bi.Product.Price,
                    ProductId = bi.ProductId,
                    ProductQuantity = bi.Quantity,
                    SizeName = bi.Size.Name,
                    ColorName = bi.Color.Name,
                })
                .ToList(),

            Total = _pustokDbContext.BasketItems.Sum(bi => bi.Quantity * bi.Product.Price)
        };

        return View(model);
    }
}
