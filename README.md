# Scriban Express

* Scriban Express is desgined for rendering lots of small simple templates, quickly.
* Subset of Scriban (mainly due to lack of time).
* Converts Scribans AST into Expression Trees, Compiles, then caches.
* At this point Security has not been considered, templates should be controlled by trusted individuals.

## Usage
 
* See Demo Project for working examples

Configure Asp.Net
```csharp
   services.AddScribanExpress();
```

Render
```csharp
// load via dependancy injection
IExpressTemplateManager expressTemplateManager

// pass in template, and object (in this example anonymous) 
expressTemplateManager.Render("{{ person.name }}", new { Person })
```

## Build and Test

[![Build Status](https://dev.azure.com/sevenwestmedia/Inferno/_apis/build/status/sevenwestmedia-labs.ScribanExpress?branchName=master)](https://dev.azure.com/sevenwestmedia/Inferno/_build/latest?definitionId=248&branchName=master)

### Build

```pwsh
dotnet build
```

### Tests

```pwsh
 dotnet test --logger trx  /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura   /p:Exclude="[xunit*]*%2c[Scriban]*%2c[*UnitTests*]*"
```

### Benchmarks

 ```pwsh
 cd  ScribanExpress.Benchmarks
 dotnet run -c Release --f *
 ```


 ## TODO

 * correct folder structure
 * Better error Logging
 * List functions
 * assignment
 * named variables
 * documentation
 * add scriban default libraries
 * look into truthy tests
 * improve exensibility (standard library)
 * Null propagation/null conditional
 * Raise better errors/messaging
 * More implicit conversions

## Future

* consider converting Ienumberables to Arrays on indexing
* consider using a nested ConcurentDictionary<Type,ConcurentDictionary<string,function>>  for performance
* Post benchmarks to a datasource
* Consider rich errors (copy scriban instead of exceptions)
* Make StandardLibrary more extenable e.g
    * dynamic,expando
    * interfaces with constraints
    * dictionary (because library should be know at expression compile time, it could be made typesafe)
* Look into async
* casting (named methods?, variable types?)
* better error managment (EventPipe?) for broken templates/rendering errors