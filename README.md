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
- Supports ASP.NET Core model metadata and application model conventions
- Supports simple types and collections of simple types only
- Supports specifying an endpoint name
- Supports LinkOptions
- Supports bypassable caching
- Invalidates HttpContext's ambient route values

## Binding source filter:
You can specify a predicate which can determine whether an action parameter should be included based on its binding source.

The default one is:
```C#
Func<BindingSource?, bool> bindingSourceFilter = bindingSource =>
    bindingSource == null
    || bindingSource.CanAcceptDataFrom(BindingSource.Query)
    || bindingSource.CanAcceptDataFrom(BindingSource.Path);
```
You pass null or default(T) to excluded action parameters when calling IUriGenerator.GetUriByExpression or a similar method.

For more information on binding sources, see ASP.NET Core documentation.

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

Additionally, it uses its internal Microsoft.Extensions.Caching.Memory.MemoryCache instance to cache extracted action methods' metadata.

This means that, for example, on 2017 Surface Book 2 you are able to generate 150 000 URLs in a second using a template like this: https://localhost:44339/api/invoices/{id}.

## Setup:
- Install UriGeneration via NuGet Package Manager, Package Manager Console or dotnet CLI:
```
Install-Package UriGeneration
```
```
dotnet add package UriGeneration
```
- Register UriGeneration in a service container (each method cache entry will have a size of 1):
```C#
builder.Services.AddUriGeneration();
```
```C#
builder.Services.AddUriGeneration(options =>
{
    options.MethodCacheSizeLimit = 500;
    options.MethodCacheCompactionPercentage = 0.5;    
    options.BypassMethodCache = false;
    options.BypassCachedExpressionCompiler = false;
    options.BindingSourceFilter = bindingSource =>
        bindingSource == null
        || bindingSource.CanAcceptDataFrom(BindingSource.Query)
        || bindingSource.CanAcceptDataFrom(BindingSource.Path);
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
