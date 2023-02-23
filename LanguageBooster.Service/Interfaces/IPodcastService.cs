using LanguageBooster.Domain.Entities;
using LanguageBooster.Service.Helpers;

namespace LanguageBooster.Service.Interfaces
{
    public interface IPodcastService
    {
        Task<Response<Podcast>> AddAsync(string filePath, Podcast podcast);
        Task<Response<Podcast>> GetAsync(Predicate<Podcast> predicate);
        Task<Response<List<Podcast>>> GetAllAsync(Predicate<Podcast> predicate = null);
        Task<Response<Podcast>> UpdateAsync(Podcast podcast);
        Task<Response<bool>> DeleteAsync(Predicate<Podcast> predicate);
        void Play(Podcast podcast);
    }
}
