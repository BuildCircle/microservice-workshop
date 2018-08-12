# microservice-workshop

Goal: To build and host .NET Core API from scratch with supporting tests.

# Hello World


## 1. Install dotnet
Chocolatey
```
choco install dotnetcore-sdk
```

or

[Download installer](https://www.microsoft.com/net/download)


## 2. Create web project
See available new project templates
```
dotnet new
```

Create a new solution
```
dotnet new sln --name MicroservicesWorkshop
```

Create a new empty web project
```
dotnet new web --name MicroservicesWorkshop
```


## 3. Run it
```
dotnet run
```
Go to [http://localhost:5000](http://localhost:5000)



# Superhero endpoint
We are going to create an API endpoint `/heros` that returns us a list of superheros.

## 1. Create test project
```
dotnet new xunit --name MicroservicesWorkshop.Tests
```

## 1. Install MVC

```
dotnet add package Microsoft.AspNetCore.Mvc
```

## 2. Add MVC package

- Open `Startup.cs`
- Add MVC module to the web pipeline in the `Configure` method
```
app.UseMvc();
```
- Add IOC dependencies for MVC in `ConfigureServices` method
```
services.AddMvc();
```

## 3. Add Heros Controller

- Create `Controllers` folder
- Add `HerosController` class inside `Controllers` folder...
```
using Microsoft.AspNetCore.Mvc;

namespace MicroserviceWorkshop
{
    [Route("heros")]
    public class HerosController : Controller
    {
        public IActionResult Get()
        {
            return Ok(new {
                items = new [] {
                    new {
                        name = "Batman",
                        score = 8.3
                    }
                }
            });
        }
    }
}
```