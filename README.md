# Uri Generation
Strongly typed, cached URL building for ASP.NET Core using lambda expressions:
```C#
_uriGenerator.GetUriByExpression<InvoicesController>(
    httpContext,
    c => c.GetInvoice(2));
```
```C#
_uriGenerator.GetUriByExpression(
    httpContext,
    (InvoicesController c) => c.GetInvoice(2));
```

## Features:
- Extracts action name, controller name, and route values from expression
- Delegates URL generation to LinkGenerator
- Supports ActionName, Area, NonAction and NonController attributes
- Supports Controller and Async suffixes
- Supports LinkOptions
- Supports bypassable caching

## Attribute routing:
If multiple URL paths match an action in your controller:
```C#
[HttpGet("api/invoices/{id}", Name = "ApiGetInvoice")]
[HttpGet("invoices/{id}", Name = "GetInvoice")]
public InvoiceResource GetInvoice(int id)
```
you can specify an endpoint name to build an endpoint-specific URL:
```C#
_uriGenerator.GetUriByExpression<InvoicesController>(
    httpContext,
    c => c.GetInvoice(2),
    "ApiGetInvoice");
```
```C#
_uriGenerator.GetUriByExpression<InvoicesController>(
    httpContext,
    c => c.GetInvoice(2),
    "GetInvoice");
```
UriGenerator will validate whether such endpoint name is defined in action's HttpMethod, AcceptVerbs, or Route attribute and build URL based on it and expression's route values.

To further improve on this, you can use classes such as Microsoft.Azure.Management.ResourceManager.Fluent.Core.ExpandableStringEnum instead of strings to define your endpoint names. That would require you to write a simple adapter class that takes a string from your class and passes it to UriGenerator.

## Performance:
Extracting values from expression trees does introduce some overhead. To work around this problem, UriGeneration uses AspNetWebStack's CachedExpressionCompiler, so that equivalent route values' values' expression trees only have to be compiled once.

Additionally, it uses its internal Microsoft.Extensions.Caching.Memory.MemoryCache instance to cache extracted controller names, action names, and route values' keys within the scope of the application lifetime.

This means that, for example, on 2017 Surface Book 2 you are able to build 100000 URLs in a second using a template like this: https://localhost:44339/api/invoices/{id}.

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
    o.SizeLimit = 100;
    o.CompactionPercentage = 0.75;
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

## Credits:
- LinkBuilder and CachedExpressionCompiler by https://github.com/aspnet/AspNetWebStack (Apache-2.0 license)