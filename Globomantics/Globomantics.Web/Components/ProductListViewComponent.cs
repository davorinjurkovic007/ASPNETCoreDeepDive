using Globomantics.Domain.Models;
using Globomantics.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Globomantics.Web.Components
{
    public class ProductListViewComponent : ViewComponent
    {
        private readonly IRepository<Product> productRepository;
        private readonly ILogger<ProductListViewComponent> logger;

        public ProductListViewComponent(IRepository<Product> productRepository, ILogger<ProductListViewComponent> logger)
        {
            this.productRepository = productRepository;
            this.logger = logger;
        }

        /// <summary>
        /// I'd like this ViewComponent to be in charge of loading the products
        /// and then also returning the appropriate view.
        /// </summary>
        /// <returns></returns>
        public Task<IViewComponentResult> InvokeAsync()
        {
            var products = productRepository.All();

            logger.LogInformation($"Found a total of {products.Count()} products");

            //var viewResult = View(products);

            //return Task.FromResult<IViewComponentResult>(viewResult);
           
            /// When this component is being invoked, it will look for a corresponding view in a very specific path.
            /// The view discovery is slightly different from a normal action's invocation to the view method. 
            /// This will look in the Views folder and then look in Shared, Components, and the it will look for a component
            /// that matches the component that we have crated (ProductList --> ne gleda ViewComponent, to je zanemareno. 
            /// In our case, that folder has to be called ProductList because our component is called ProductListViewComponent
            return Task.FromResult<IViewComponentResult>(View(products));
        }
    }
}
