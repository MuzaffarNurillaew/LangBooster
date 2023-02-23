using LanguageBooster.Domain.Entities;
using LanguageBooster.Service.Helpers;

namespace LanguageBooster.Service.Interfaces
{
    public interface ILanguageService
    {
        Task<Response<Language>> AddAsync(Language language);
        Task<Response<Language>> GetAsync(Predicate<Language> predicate);
        Task<Response<List<Language>>> GetAllAsync(Predicate<Language> predicate = null);
        Task<Response<bool>> DeleteAsync(Predicate<Language> predicate);
    }
}
