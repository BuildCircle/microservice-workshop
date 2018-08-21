# Edge testing

To build microservices you need good tests. Ideally our tests should cover behaviour of the API without being too coupled to the implementation.

Goal: To create the unit tests that cover as much code as possible. To use the dotnet core `TestServer` and `WebHostBuilder` classes to host our API in-memory and test against it.

We are going to create an API endpoint `/heros` that pulls a list of characters from Amazon S3, filters out the villains and returns only the superheros.

## 1. Run tests

To make sure everything is configured correctly, lets start by running our single empty test.

```
dotnet test MicroserviceWorkshop.Tests
```

## 2. Test for a 200 response

We're going to use the .net core `WebHostBuilder` and `TestServer` classes to test our API. 

- `Microsoft.AspNetCore.Hosting.WebHostBuilder` configures Kestrel as our web server and hooks in to the aspnetcore request processing pipeline.
- `Microsoft.AspNetCore.TestHost.TestServer` allows us to make in-memory requests to our web server by hooking into the request processing pipeline.

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