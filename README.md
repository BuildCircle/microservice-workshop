# microservice-workshop

Goal: To build and host .NET Core API with edge to edge unit tests using .net core TestServer and host in docker.

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

# Heros endpoint
We are going to create an API endpoint `/heros` that returns us a list of superheros.

## 1. Run tests
```
dotnet test MicroserviceWorkshop.Tests
```

## 2. Test for a 200 response
We're going to use the .net core `Microsoft.AspNetCore.Hosting.WebHostBuilder` and `Microsoft.AspNetCore.TestHost.TestServer` classes to test our API. 
- `WebHostBuilder` configures Kestrel as our web server and hooks in to the aspnetcore request processing pipeline.
- `TestServer` allows us to make in-memory requests to our web server by hooking into the request processing pipeline.

We can use them like this...

```
var startup = new WebHostBuilder().UseStartup<Startup>();
var testServer = new TestServer(startup);
HttpClient client = testServer.CreateClient();

var response = await client.GetAsync("your-uri-path");

// Write test logic here
```

Lets start by asserting that we get a 200 response. You can do this by using the same `GetAsync` method on the `HttpClient` as shown above, and asserting the status code of the response.

## 3. Return 200 response
- Create `Controllers` folder
- Add `HerosController` class inside `Controllers` folder
- Create a `Get` method with a `[Route("heros")]` attribute that returns an `Ok()` result

At this point we know our API is configured correctly and we are routing correctly to the `HerosController`. Our test should pass.

## 4. Test the heros data from the API



# Run with Docker
```
docker build .
docker run -p 5000:80 -d <container-id>
```