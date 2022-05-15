# Event Adder

## Scaffolding

### Database

~~~sh
dotnet ef migrations add --project App.DAL.EF --startup-project WebApp --context ApplicationDbContext Initial

dotnet ef migrations remove --project App.DAL.EF --startup-project WebApp --context ApplicationDbContext

dotnet ef database update --project App.DAL.EF --startup-project WebApp

dotnet ef database drop --project App.DAL.EF --startup-project WebApp
~~~

### Controllers

NB! Change the slash ("/") in "-outDir Areas/Admin/Controllers" according to your operations system - its "\" on windows and "/" on *nix.   

#### MVC razor based
~~~sh
dotnet aspnet-codegenerator controller -name CompaniesController       -actions -m  App.Domain.Company    -dc App.DAL.EF.ApplicationDbContext -outDir Areas\Admin\Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name PersonsController       -actions -m  App.Domain.Person    -dc ApplicationDbContext -outDir Areas\Admin\Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name EventsController       -actions -m  App.Domain.Event    -dc ApplicationDbContext -outDir Areas\Admin\Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
~~~