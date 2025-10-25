using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Scalar.AspNetCore;

namespace Host.Web.Api.Infrastructure.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseCustomHealthChecks(this IApplicationBuilder app)
    {
        Dictionary<HealthStatus, int> statusCodes = new()
        {
            [HealthStatus.Healthy] = StatusCodes.Status200OK,
            [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
        };

        app
            .UseHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResultStatusCodes = statusCodes,
            })
            .UseHealthChecks("/healthz", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                ResultStatusCodes = statusCodes,
            });

        return app;
    }

    public static IApplicationBuilder UseHealthCheckDashboard(this IApplicationBuilder app)
    {
        app.UseHealthChecksUI(options =>
         {
             options.UIPath = "/ui/health";
             options.ApiPath = "/api/health";

             options.UseRelativeApiPath = false;
             options.UseRelativeResourcesPath = false;
             options.UseRelativeWebhookPath = false;
         });

        return app;
    }

    public static IApplicationBuilder ConfigureScalarApi(this WebApplication app)
    {
        app.MapScalarApiReference(opt =>
        {
            opt.WithDownloadButton(true)
                .WithTestRequestButton(true)
                .WithTitle("Modular Medium API")
                .WithSidebar(true)
                .WithDarkModeToggle(false)
                .WithLayout(ScalarLayout.Modern)
                .WithModels(false)
                .WithDefaultHttpClient(ScalarTarget.CSharp,ScalarClient.HttpClient);
        });

        return app;
    }
}