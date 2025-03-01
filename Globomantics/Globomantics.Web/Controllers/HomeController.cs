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

    public IActionResult TicketDetails(Guid productId, string slug)
    {
        throw new NotImplementedException();
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
