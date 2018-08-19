using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MicroserviceWorkshop
{
    internal class CharactersProvider : ICharactersProvider
    {
        const string charactersUri = "http://localhost:8000/build-circle/characters.json";
        HttpClient _client = new HttpClient();
        

        public async Task<CharactersResponse> GetCharacters()
        {
            var response = await _client.GetAsync(charactersUri);

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CharactersResponse>(responseJson);
        }
    }
}