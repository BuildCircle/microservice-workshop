
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