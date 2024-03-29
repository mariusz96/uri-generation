﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace UriGeneration.IntegrationTests.WebApplicationFactories
{
    public class AreasWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
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
                        name: "AreaRn",
                        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                    endpoints.MapDefaultControllerRoute();
                    endpoints.MapRazorPages();
                });
            });

            builder.UseContentRoot(".");
        }
    }
}
