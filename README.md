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

 ## MVP
 * Demo
 * Handle Errors { expression, errors}
	Plan:	if a stament breaks, just log and skip
			How do we make it obvious that a particluar template has broken statements,? metrics?
					could be that it was a temporary item, no need to alert on that
					but we don't really want to take down the whole site for messing something simple up
					logging every error is a no go
					logging once, will be missed
					metric only on error might be ok
			if scriban breaks? 
				what are our options? should we return a default or throw
 * renamer
 * cache on input type(s)
 * allow custom Types
 * list functions

 ## TODO
 * Allow convertion in functions, e.g pass into double
 * correct folder structure
 * assignment
 * ci/cd
 * named variables
 * documentation
