using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MicroserviceWorkshop
{
    internal class CharactersProvider : ICharactersProvider
    {
        const string charactersUri = "https://s3.eu-west-2.amazonaws.com/build-circle/characters.json";
        HttpClient _client = new HttpClient();
        

        public async Task<CharactersResponse> GetCharacters()
        {
            var response = await _client.GetAsync(charactersUri);

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CharactersResponse>(responseJson);
        }
    }
}