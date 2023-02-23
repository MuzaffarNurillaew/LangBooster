using LanguageBooster.Domain.Entities;
using LanguageBooster.Service.Helpers;

namespace LanguageBooster.Service.Interfaces
{
    public interface IWordService
    {
        Task<Response<Word>> AddAsync(Word word);
        Task<Response<Word>> GetAsync(Predicate<Word> predicate);
        Task<Response<List<Word>>> GetAllAsync(Predicate<Word> predicate = null);
        Task<Response<Word>> UpdateAsync(Word word);
        Task PronunceAsync(Word word);
    }
}
