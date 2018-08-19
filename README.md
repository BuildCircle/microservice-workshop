# microservice-workshop

Goal: To build and host .NET Core API from scratch with supporting tests.

# Prerequesits
- Install dotnet

Chocolatey
```
choco install dotnetcore-sdk
```

or

[Download installer](https://www.microsoft.com/net/download)

- Install docker

[Download installer](https://www.docker.com/products/docker-desktop)

## 2. Create web project
See available new project templates
```
dotnet new
```

Create a new solution
```
dotnet new sln --name MicroserviceWorkshop
```

Create a new empty web project
```
dotnet new web --name MicroserviceWorkshop
dotnet sln MicroserviceWorkshop.sln add MicroserviceWorkshop/MicroserviceWorkshop.csproj
```


## 3. Run it
```
dotnet run --project MicroserviceWorkshop/MicroserviceWorkshop.csproj
```
Go to [http://localhost:5000](http://localhost:5000)



# Heros endpoint
We are going to create an API endpoint `/heros` that returns us a list of superheros.


## 1. Create test project
```
dotnet new xunit --name MicroserviceWorkshop.Tests
dotnet sln MicroserviceWorkshop.sln add MicroserviceWorkshop.Tests/MicroserviceWorkshop.Tests.csproj
dotnet add MicroserviceWorkshop.Tests reference MicroserviceWorkshop
```

## 2. Run tests
```
dotnet test MicroserviceWorkshop.Tests
```


## 3. Install MVC
```
dotnet add package Microsoft.AspNetCore.Mvc
```


## 4. Add MVC package
- Open `Startup.cs`
- Add MVC module to the web pipeline in the `Configure` method
```
app.UseMvc();
```
- Add IOC dependencies for MVC in `ConfigureServices` method
```
services.AddMvc();
```


## 5. Add Heros Controller
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

https://s3.eu-west-2.amazonaws.com/build-circle/characters.json

```
docker build .
docker run -p 5000:80 -d <container-id>
```