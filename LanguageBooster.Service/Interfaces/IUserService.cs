using LanguageBooster.Domain.Entities;
using LanguageBooster.Service.Helpers;

namespace LanguageBooster.Service.Interfaces
{
    public interface IUserService
    {
        Task<Response<User>> AddAsync(User user);
        Task<Response<User>> UpdateAsync(long id, User user);
        Task<Response<bool>> DeleteAsync(Predicate<User> predicate);
        Task<Response<User>> GetAsync(Predicate<User> predicate = null);
        Task<Response<List<User>>> GetAllAsync(Predicate<User> predicate);
        Task<Response<bool>> AddFavouriteVocab(long userId, long wordId);
        Task<Response<bool>> AddFavouritePodcast(long userId, long podcastId);
        Task<Response<List<Word>>> GetTodaysDailyVocabs(long userId);
        Task<Response<List<Word>>> GetFavouriteWordsAsync(long userId);
        Task<Response<List<Podcast>>> GetFavouritePodcastsAsync(long userId);

    }
}
