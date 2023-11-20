# Uri Generation
A strongly typed URL generation library for ASP.NET Core:
```C#
string? uri = _uriGenerator.GetUriByExpression<InvoicesController>(
    httpContext,
    c => c.GetInvoice(2));
```

## Features:
- Extracts action name, controller name, and route values from expression
- Delegates URL generation to LinkGenerator
- Supports ActionName, Area, NonAction, NonController, FromBody, FromForm, FromHeader, FromServices and FromKeyedServices attributes
- Supports IFormFile, IFormFileCollection, IEnumerable&lt;IFormFile&gt;, CancellationToken, and IFormCollection types
- Supports specifying an endpoint name
- Supports Controller and Async suffixes
- Supports LinkOptions
- Supports bypassable caching

## Specifying an endpoint name:
If you use named attribute routes:
```C#
[HttpGet("api/invoices/{id}", Name = "ApiGetInvoice")]
[HttpGet("invoices/{id}", Name = "GetInvoice")]
public InvoiceResource GetInvoice(int id)
```
or dedicated conventional routes:
```C#
app.MapControllerRoute(
    name: "ApiGetInvoice",
    pattern: "api/invoices/{id}",
    defaults: new { controller = "Invoices", action = "GetInvoice" });
```
```C#
app.MapControllerRoute(
    name: "GetInvoice",
    pattern: "invoices/{id}",
    defaults: new { controller = "Invoices", action = "GetInvoice" });
```
you can specify an endpoint name to generate an endpoint-specific URL:
```C#
string? uri = _uriGenerator.GetUriByExpression<InvoicesController>(
    httpContext,
    c => c.GetInvoice(2),
    "ApiGetInvoice");
```
```C#
string? uri = _uriGenerator.GetUriByExpression<InvoicesController>(
    httpContext,
    c => c.GetInvoice(2),
    "GetInvoice");
```
For more information on endpoint names, see ASP.NET Core documentation.

## Performance:
Extracting values from expression trees does introduce some overhead. To partially work around this problem, UriGeneration uses ASP.NET's CachedExpressionCompiler, so that equivalent route values' values' expression trees only have to be compiled once.

Additionally, it uses its internal Microsoft.Extensions.Caching.Memory.MemoryCache instance to cache extracted controller names, action names, and route values' keys within the scope of the application lifetime.

This means that, for example, on 2017 Surface Book 2 you are able to generate 200 000 URLs in a second using a template like this: https://localhost:44339/api/invoices/{id}.

## Setup:
- Install UriGeneration via NuGet Package Manager, Package Manager Console or dotnet CLI:
```
Install-Package UriGeneration
```
```
dotnet add package UriGeneration
```
- Register UriGeneration in a service container (each cache entry will have a size of 1):
```C#
builder.Services.AddUriGeneration();
```
```C#
builder.Services.AddUriGeneration(o =>
{
    o.SizeLimit = 500;
    o.CompactionPercentage = 0.5;
});
```
- Request an instance of IUriGenerator singleton service from any constructor in your app:
```C#
public class InvoicesController
{
    private readonly IUriGenerator _uriGenerator;

    public InvoicesController(IUriGenerator uriGenerator)
    {
        _uriGenerator = uriGenerator;
    }
}
```

## Debugging:
To see the default log messages, enable LogLevel.Debug in the logging configuration.

## Credits:
- LinkBuilder and CachedExpressionCompiler by https://github.com/aspnet/AspNetWebStack (Apache-2.0 license)
