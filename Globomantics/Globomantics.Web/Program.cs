using Globomantics.Domain.Models;
using Globomantics.Infrastructure.Data;
using Globomantics.Infrastructure.Repositories;
using Globomantics.Web.Constraints;
using Globomantics.Web.Filters;
using Globomantics.Web.Repositories;
using Globomantics.Web.Transformers;
using Globomantics.Web.ValueProviders;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore.Internal;

/// Routing to controller actions in ASP.NET Core
/// https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/routing?view=aspnetcore-9.0
/// 
// Routing in ASP.NET Core
/// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/routing?view=aspnetcore-9.0

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ///The route ha to match what you have specified on the controller or on the action
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;
                options.LoginPath = "/Login";
            });

        builder.Services.AddSession();

        // This will let us configure how the routing works. 
        builder.Services.AddRouting(options =>
        {
            // This can now be used lie the build-in constraints
            options.ConstraintMap["validateSlug"] = typeof(SlugConstraint);
            options.ConstraintMap["slugTransform"] = typeof(SlugParameterTransformer);
        });

        /// Adding a Distributed cache
        /// 
        /// The session data will be stored unencrypter without data protection enbled
        /// Adding this will store the session data in a redis cache!
        /// 
        /// NOTE: StackExchange is special NuGet package for communication with Redis
        //builder.Services.AddStackExchengeRedisChache( options =>
        //{
        //    options.Configuration = "ConnectionStringGoesHere";
        //    options.InstanceName = "GlobomanticsInstance";
        //});

        // Add services to the container.
        builder.Services.AddControllersWithViews(options =>
        {
            options.ValueProviderFactories.Add(new SessionValueProviderFactory());
        });

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddDbContext<GlobomanticsContext>(ServiceLifetime.Scoped);

        builder.Services.AddTransient<IStateRepository, SessionStateRepository>();
        builder.Services.AddTransient<IRepository<Customer>, CustomerRepository>();
        builder.Services.AddTransient<IRepository<Product>, ProductRepository>();
        builder.Services.AddTransient<IRepository<Order>, OrderRepository>();
        builder.Services.AddTransient<IRepository<Cart>, CartRepository>();
        builder.Services.AddTransient<ICartRepository, CartRepository>();

        builder.Services.AddTransient<TimerFilter>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.MapStaticAssets();

        app.UseRouting();

        /// The have to be added in this exact order
        app.UseAuthentication();

        app.UseAuthorization();

        app.UseSession();

        /// Creating a Custom Middleware
        /// 
        /// This adds a middleware to the ASP.NET Core pipeline
        /// Executes after authorization but before the endpoing
        /// 
        //app.Use(async (context, next) =>
        //{
        //    Debug.WriteLine("This executes before the endpoint");

        //    await next.Invoke();

        //    Debug.WriteLine("Endpoing executed!");
        //});

        // Alternative route to avoid attribute on the TicketDetails action
        //app.MapControllerRoute(
        //    name: "ticketDetailsRoute",
        //    defaults : new {action = "TicketDetails", controller = "Home" },
        //    pattern: "/details/{productId}/{slug?}")
        //    .WithStaticAssets();


        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();

        using (var scope = app.Services.CreateAsyncScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<GlobomanticsContext>();

            GlobomanticsContext.CreateInitialDatabase(context);
        }

        app.Run();
    }
}