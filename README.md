# microservice-workshop

Goal: To build and host .NET Core API with edge to edge unit tests using .net core TestServer and host in docker.


# Prerequisites
- Install dotnet

Chocolatey
```
choco install dotnetcore-sdk
```

or

[Download installer](https://www.microsoft.com/net/download)


# Hello world
Run the API
```
dotnet run MicroserviceWorkshop
```
The API should start and show you the uris to access it from. Verify the API is running by navigating there in your browser.


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

Lets start by asserting that we get a 200 response. You can do this by using the same `GetAsync` method on the `HttpClient` as shown above, and asserting the status code of the response. Run the test - it should fail.

## 3. Return 200 response
- Create `Controllers` folder
- Add `HerosController` class inside `Controllers` folder
- Create a `Get` method with a `[Route("heros")]` attribute that returns an `Ok()` result

At this point we know our API is configured correctly and we are routing correctly to the `HerosController`. Our test should pass.

## 4. Test for the expected heros data from the API
A `CharactersProvider` has been provided that you can use to get a list of characters which are loaded from Amazon S3. Navigate to [https://s3.eu-west-2.amazonaws.com/build-circle/characters.json] to see the content.

We only want to return the heros from that list. Add some more assertions to your test that verify this behaviour. We should have assertions for the amount of items in the list and that one of the items has the correct data.

Tip: you can deserialise the items like this:
```
var responseJson = await response.Content.ReadAsStringAsync();
var responseObject = JsonConvert.DeserializeObject<JObject>(responseJson);

var items = responseObject.Value<JArray>("items");

// items.Count.Should().Be(##);
// items[0].Value<string>("name").Should().Be("a-hero-name");
```

## 5. Return the heros data
Use the `CharactersProvider` class to get the characters from S3 and filter out the villains. Then run the test again to make sure it behaves as expected.

## 6. Faking the boundary
The characters data in S3 is dynamic so this level of integration test is not repeatable. To make our test more reliable, we should swap the boundary of the application for a fake or mock. We can do this by registering a fake `ICharactersProvider` which we can then take into the constructor of the `HerosController`.

The following class works as a simple fake that we can use.
```
public class FakeCharactersProvider : ICharactersProvider
{
    CharactersResponse _response;
    
    public void FakeResponse(CharactersResponse response)
    {
        _response = response;
    }

    public Task<CharactersResponse> GetCharacters()
    {
        return Task.FromResult(_response);
    }
}
```


You can register an instance of this class by adding it to the host's service collection.
```
var charactersProvider = new FakeCharactersProvider();

var startup = new WebHostBuilder()
                .UseStartup<Startup>()
                .ConfigureServices(x => 
                {
                    x.AddSingleton<ICharactersProvider>(charactersProvider);
                });
var testServer = new TestServer(startup);
var client = testServer.CreateClient();

charactersProvider.FakeResponse(new CharactersResponse
{
    Items = new []
    {
        new CharacterResponse
        {
            Name = "Batman",
            Score = 8.3,
            Type = "hero"
        },
        new CharacterResponse
        {
            Name = "Joker",
            Score = 8.2,
            Type = "villain"
        }
    }
});
```

# 7. Inject the fake
Inject the fake into the constructor of `HerosController`.

```
private readonly ICharactersProvider _charactersProvider;

public HerosController(ICharactersProvider charactersProvider)
{
    _charactersProvider = charactersProvider;
}
```
Your test should now pass.


# Run with Docker

- [Install docker](https://www.docker.com/products/docker-desktop)

## Create Dockerfile
To build a docker container to host our app we must create a [Dockerfile](https://docs.docker.com/engine/reference/builder/).

We are going to create a Dockerfile that builds and publishes the artifacts of our app from the dotnet sdk container. We will then use those artifacts in our dotnet core runtime container as our entrypoint.

### Build steps
1. Create a file named `Dockerfile` in the root of the project. Add the following line to load the dotnet sdk container.
```
FROM microsoft/dotnet:2.1-sdk AS build-env
```

2. Copy the source code from our project to the container.
```
COPY ./MicroserviceWorkshop /app/src/
```

3. Build and publish the artefacts.
```
WORKDIR /app/src
RUN dotnet publish -c Release -o ../out
```

### Run steps
1. In the same Dockerfile, load the dotnet runtime container.
```
FROM microsoft/dotnet:2.1-aspnetcore-runtime
```

2. Copy the artefacts from the dotnet sdk container into the `app` directory of the dotnet runtime container.
```
WORKDIR /app
EXPOSE 80

COPY --from=build-env /app/out .
```

3. Run
```
ENTRYPOINT ["dotnet", "MicroserviceWorkshop.dll"]
```

## Build and run the container
Now that we have a Dockerfile we can build and run it.
```
docker build .
docker run -p 5000:80 -d <container-id>
```