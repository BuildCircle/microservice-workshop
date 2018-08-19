using System;
using Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using MicroserviceWorkshop;
using System.Threading.Tasks;
using FluentAssertions;
using System.Net;

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
        }
    }
}
