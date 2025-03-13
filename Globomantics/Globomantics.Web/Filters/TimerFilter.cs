using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace Globomantics.Web.Filters;

public class TimerFilter : IAsyncActionFilter
{
    private readonly ILogger<TimerFilter> logger;

    public TimerFilter(ILogger<TimerFilter> logger)
    {
        this.logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var stopwatch = new Stopwatch();

        stopwatch.Start();

        logger.LogInformation($"Action {context.ActionDescriptor} started");

        await next();

        stopwatch.Stop();

        logger.LogInformation($"Action {context.ActionDescriptor} completed");
        logger.LogInformation($"{context.ActionDescriptor} ran for {stopwatch.ElapsedMilliseconds} ms");
    }
}
