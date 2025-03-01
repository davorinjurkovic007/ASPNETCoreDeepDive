using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Globomantics.Web.Models;
using Globomantics.Infrastructure.Repositories;
using Globomantics.Domain.Models;

namespace Globomantics.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> logger;
    private readonly IRepository<Product> productRepository;

    public HomeController(IRepository<Product> productRepository, ILogger<HomeController> logger)
    {
        this.productRepository = productRepository;
        this.logger = logger;
    }

    public IActionResult Index()
    {
        var products = productRepository.All();

        logger.LogInformation($"Loaded {products.Count()} products");

        return View(products);
    }

    /// <summary>
    /// If you make your properties optional I would advice that you make them nullable.
    /// Otherwise, you won't know that this might cause a problem
    /// This was done with Alternative route to avoid attribute on the TicketDetails action, in Program.cs
    /// Method signature must be then
    /// public IActionResult TicketDetails(Guid productId, string? slug)
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="slug"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [Route("/details/{productId}/{slug}")]
    public IActionResult TicketDetails(Guid productId, string slug)
    {
        var product = productRepository.Get(productId);

        return View(product);
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
}
