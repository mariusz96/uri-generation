using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

#if NET8_0_OR_GREATER
using UriGeneration.IntegrationTests.Services;
#endif

namespace UriGeneration.IntegrationTests.WebApplicationFactories
{
    public class CommonWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
#if NET8_0_OR_GREATER
                services.AddKeyedSingleton<ITestService, TestService>("test");
#endif

                services.AddUriGeneration();

                services.AddRazorPages();
                services.AddControllersWithViews();
            });

            builder.Configure(app =>
            {
                var env = app.ApplicationServices
                    .GetRequiredService<IWebHostEnvironment>();

                if (!env.IsDevelopment())
                {
                    app.UseExceptionHandler("/Error");
                    app.UseHsts();
                }

                app.UseHttpsRedirection();
                app.UseStaticFiles();

                app.UseRouting();

                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapGet("/hi", () => "Hello!");

                    endpoints.MapControllerRoute(
                        name: "Rn1",
                        pattern: "RouteName1/Test6",
                        defaults: new
                        {
                            controller = "ConventionalRouting",
                            action = "Test6"
                        });

                    endpoints.MapControllerRoute(
                        name: "Rn2",
                        pattern: "RouteName2/Test6",
                        defaults: new
                        {
                            controller = "ConventionalRouting",
                            action = "Test6"
                        });

                    endpoints.MapDefaultControllerRoute();
                    endpoints.MapRazorPages();
                });
            });

            builder.UseContentRoot(".");
        }
    }
}
