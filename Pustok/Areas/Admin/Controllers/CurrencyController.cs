using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pustok.Contracts;
using Pustok.Helpers;

namespace Pustok.Areas.Admin.Controllers;

[Area("admin")]
[Authorize(Roles = Role.Names.SuperAdmin)]
[Route("admin/currencies")]
public class CurrencyController : Controller
{
    private IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public CurrencyController(
        IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        var httpClient = _httpClientFactory.CreateClient();
        var apiBase = _configuration.GetSection("CurrencyApiBase").Value;
        var apiKey = _configuration.GetSection("CurrencyApiKey").Value;

        var uriBuilder = new UrlBuilder(apiBase);
        var endpoint = uriBuilder
            .AddQuery("access_key", apiKey)
            .Build();

        var currencyResult = await httpClient.GetFromJsonAsync<CurrencyResult>(endpoint);

        return Json(currencyResult);
    }
}
