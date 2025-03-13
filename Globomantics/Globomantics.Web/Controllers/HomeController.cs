using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Globomantics.Web.Models;
using Globomantics.Infrastructure.Repositories;
using Globomantics.Domain.Models;
using System.ComponentModel.DataAnnotations;
using NuGet.Protocol;
using Globomantics.Web.Filters;
using Globomantics.Web.Attributes;

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

    [TimerFilter]
    //[ServiceFilter(typeof(TimerFilter))]
    public IActionResult Index()
    {
        //var products = productRepository.All();

        //logger.LogInformation($"Loaded {products.Count()} products");

        //return View(products);

        // ViewComponent involved -> 
        /// This means that our HomeController and the Index action no longer need to fetch all the products when we are 
        /// hitting the start page.
        /// So we could simplified this as well. 
        return View();
    }

    /// <summary>
    /// If you make your properties optional I would advice that you make them nullable.
    /// Otherwise, you won't know that this might cause a problem
    /// This was done with Alternative route to avoid attribute on the TicketDetails action, in Program.cs
    /// Method signature must be then
    /// public IActionResult TicketDetails(Guid productId, string? slug)
    /// 
    /// [Route("/details/{productId:guid}/{slug?}")]
    /// public IActionResult TicketDetails(Guid productId, string? slug)
    /// 
    /// When defining the route, the optional parameters are simply indicated by adding the question mark.
    /// Make sure that your parameter that you're expecting in your action is also marked with a question mark to
    /// indicate that it's a nullable type.
    /// That way the nullable reference types in .NET and C# will properly notify you if there are potential null
    /// reference exception.
    /// 
    /// If we remove validateSlug and rerun the application, we can see that he second one is also a lot more beautiful
    /// in comparison to what it was earilier.
    /// 
    /// Using the IRouteContraint to validate the slug, in our case, can cause some problems because it's also used when
    /// producing the URL with thisngs like the tag helper. 
    /// 
    /// "You can use a combination of transformers and contraints but it may have side effects!"
    /// 
    /// public IActionResult TicketDetails(Guid productId,[RegularExpression("^[a-zA-Z0-9- ]+$")] string slug)
    /// Bettete allternative so hat you're not left thinking od hot to do this in a better way.
    /// we have to check the ModelState property. 
    /// 
    /// For more complex contraints use IRouteConstraint
    /// For more complex code, I'd introduce an IRouteContraint so that you don't clutter ylur route definition.
    /// That would also make that contraint reusable. 
    /// A good example would have been to do this for a social security number where you know the exactly format for every time. 
    /// </summary>
    /// <param name="productId">We define that our productId has to be Guid</param>
    /// <param name="slug"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    //[Route("/details/{productId:guid}/{slug:validateSlug}")]
    //[Route("/details/{productId:guid}/{slug:slugTransform:validateSlug}")]
    [Route("/details/{productId:guid}/{slug:slugTransform}")]
    public IActionResult TicketDetails(Guid productId,[RegularExpression("^[a-zA-Z0-9- ]+$")] string slug)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest();
        }

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
