using System.Threading.Tasks;

namespace MicroserviceWorkshop
{
    public interface ICharactersProvider
    {
        Task<CharactersResponse> GetCharacters();
    }
}