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

namespace MicroserviceWorkshop.Tests
{
    public class HerosTests
    {
        [Fact]
        public async Task CanGetHeros()
        {
            true.Should().BeTrue();
        }
    }
}
