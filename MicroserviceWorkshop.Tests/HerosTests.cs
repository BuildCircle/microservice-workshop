using System;
using Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using MicroserviceWorkshop;
using System.Threading.Tasks;
using FluentAssertions;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace MicroservicesWorkshop.Tests
{
    public class HerosTests
    {
        [Fact]
        public async Task CanGetHeros()
        {
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

            var response = await client.GetAsync("heros");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseJson = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<JObject>(responseJson);

            var items = responseObject.Value<JArray>("items");
            items.Count.Should().Be(1);

            items[0].Value<string>("name").Should().Be("Batman");
            items[0].Value<decimal>("score").Should().Be(8.3m);
        }
    }

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
}
