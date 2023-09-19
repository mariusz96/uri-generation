using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace UriGeneration.IntegrationTests.WebApplicationFactories
{
    public class SlugifyWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddUriGeneration();

                services.AddRazorPages();
                services.AddControllersWithViews(options =>
                    options.Conventions.Add(new RouteTokenTransformerConvention(
                                 new SlugifyParameterTransformer())));
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

                    endpoints.MapDefaultControllerRoute();
                    endpoints.MapRazorPages();
                });
            });

            builder.UseContentRoot(".");
        }
    }
}
