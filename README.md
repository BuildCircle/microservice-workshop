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

We only want to return the heros from that list. Add some more assertions to your test that verify this behaviour.

We should have assertions for the amount of items in the list and that one of the items has the correct data.

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
The characters data in S3 is dynamic so this level of integration test is not repeatable. To make our test more reliable, we should swap the boundary of the application for a fake or mock.



# Run with Docker
```
docker build .
docker run -p 5000:80 -d <container-id>
```