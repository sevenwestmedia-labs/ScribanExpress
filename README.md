# Scriban Express

* Scriban Express is desgined for rendering lots of small simple templates, quickly
* Is is a subset of Scriban (mainly due to lack of time)
* Compiles Scribans AST into Cached Expression Trees
* At this point Security has not been considered, templates should be controlled by trusted individuals

## Build and Test

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
 * ci/cd
 * named variables
 * documentation
 * add scriban default library
 * look into truthy tests
 * improve exensibility (standard library)
 * Null propagation/null conditional
 * Dictionary Support
 * Raise better errors/messaging
 * More implicit conversion

## Future
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