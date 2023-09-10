using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pustok.Contracts;
using Pustok.Database;
using Pustok.Services.Abstracts;
using Pustok.Services.Concretes;
using Pustok.ViewModels;

namespace Pustok.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly PustokDbContext _dbContext;
    private readonly IUserService _userService;

    public AccountController(PustokDbContext dbContext, IUserService userService)
    {
        _dbContext = dbContext;
        _userService = userService;
    }

    [HttpGet]
    public IActionResult Dashboard()
    {
        var user1 = _userService.CurrentUser;



        return View();
    }
    public IActionResult Orders()
    {

        var user = _userService.CurrentUser;
        var orders = _dbContext.Orders
            .Where(x => x.UserId == user.Id)
            .Select(x => new AccountOrderViewModel
            {
                TracingCode = x.TracingCode,
                OrderStatus = x.OrderItemStatusValue.ToString(),
                CreatedOn = x.CreatedAt,
                Total = x.OrderItems.Where(oi => oi.OrderId == x.Id).Sum(oi => oi.OrderQuantity * oi.OrderPrice),
                Count = x.OrderItems.Where(oi => oi.OrderId == x.Id).Count()
            })
            .ToList();


        return View(orders);
    }

    public IActionResult Addresses()
    {
        return View();
    }

    public IActionResult AccountDetails()
    {
        return View();
    }

    public IActionResult Logout()
    {
        //logic

        return RedirectToAction("index", "home");
    }

}
