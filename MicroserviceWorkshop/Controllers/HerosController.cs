
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
            var characters = await _charactersProvider.GetCharacters();
            
            return Ok(new {
                Items = characters.Items.Where(x => x.Type == "hero")
            });
        }
    }
}