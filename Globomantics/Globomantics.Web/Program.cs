using Globomantics.Domain.Models;
using Globomantics.Infrastructure.Data;
using Globomantics.Infrastructure.Repositories;
using Globomantics.Web.Constraints;
using Globomantics.Web.Transformers;

/// Routing to controller actions in ASP.NET Core
/// https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/routing?view=aspnetcore-9.0
/// 
// Routing in ASP.NET Core
/// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/routing?view=aspnetcore-9.0

var builder = WebApplication.CreateBuilder(args);

// This will let us configure how the routing works. 
builder.Services.AddRouting(options =>
{
    // This can now be used lie the build-in constraints
    options.ConstraintMap["validateSlug"] = typeof(SlugConstraint);
    options.ConstraintMap["slugTransform"] = typeof(SlugParameterTransformer);
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<GlobomanticsContext>(ServiceLifetime.Scoped);

builder.Services.AddTransient<IRepository<Customer>, CustomerRepository>();
builder.Services.AddTransient<IRepository<Product>, ProductRepository>();
builder.Services.AddTransient<IRepository<Order>, OrderRepository>();
builder.Services.AddTransient<IRepository<Cart>, CartRepository>();
builder.Services.AddTransient<ICartRepository, CartRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.MapStaticAssets();

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

using(var scope = app.Services.CreateAsyncScope())
{
    var context = scope.ServiceProvider.GetRequiredService<GlobomanticsContext>();

    GlobomanticsContext.CreateInitialDatabase(context);
}

app.Run();
