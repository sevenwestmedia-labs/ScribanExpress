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