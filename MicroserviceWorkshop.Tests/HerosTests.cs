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

namespace MicroservicesWorkshop.Tests
{
    public class HerosTests
    {
        [Fact]
        public async Task CanGetHeros()
        {
            var startup = new WebHostBuilder()
                            .UseStartup<Startup>();

            var testServer = new TestServer(startup);
            var client = testServer.CreateClient();

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
}
