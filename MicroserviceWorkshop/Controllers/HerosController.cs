
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MicroserviceWorkshop
{
    [Route("heros")]
    public class HerosController : Controller
    {
        private readonly ICharactersProvider _charactersProvider;

        public HerosController(ICharactersProvider charactersProvider)
        {
            _charactersProvider = charactersProvider;
        }

        public async Task<IActionResult> Get()
        {
            return Ok(await _charactersProvider.GetCharacters());
        }
    }
}