using LanguageBooster.Data.IRepositories;
using LanguageBooster.Data.Repositories;
using LanguageBooster.Domain.Entities;
using LanguageBooster.Service.Helpers;
using LanguageBooster.Service.Interfaces;

namespace LanguageBooster.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> repo = new Repository<User>();
        private readonly IRepository<Word> wordRepo = new Repository<Word>();
        private readonly IRepository<Podcast> podcastRepo = new Repository<Podcast>();

        public UserService()
        {

        }

        public UserService(long langId)
        {
            wordRepo = new Repository<Word>(new Word()
            {
                ChosenLanguageId = langId
            });
            podcastRepo = new Repository<Podcast>(new Podcast()
            {
                LanguageId = langId
            });
        }

        public async Task<Response<User>> AddAsync(User user)
        {
            // checking username is unique or not
            if (await repo.SelectAsync(x => x.Username == user.Username) is not null)
            {
                return new Response<User>()
                {
                    Message = "User already exists"
                };
            }
            // adding new user to db
            await repo.CreateAsync(user);

            return new Response<User>()
            {
                StatusCode = 200,
                Message = "Ok",
                Result = user
            };
        }

        public async Task<Response<bool>> AddFavouritePodcast(long userId, long podcastId)
        {
            var user = await repo.SelectAsync(x => x.Id == userId);

            if (user.Details.FavouritePodcastIds.IndexOf(podcastId) != -1)
            {
                return new Response<bool>();
            }

            user.Details.FavouritePodcastIds.Add(podcastId);
            await repo.UpdateAsync(userId, user);

            return new Response<bool>()
            {
                StatusCode = 200,
                Message = "Ok",
                Result = true
            };
        }

        public async Task<Response<bool>> AddFavouriteVocab(long userId, long wordId)
        {
            var user = await repo.SelectAsync(x => x.Id == userId);
            
            if (user.Details.FavouriteWordIds.Contains(wordId))
            {
                return new Response<bool>();
            }

            user.Details.FavouriteWordIds.Add(wordId);
            return new Response<bool>()
            {
                StatusCode = 200,
                Message = "Ok",
                Result = true
            };
        }

        public async Task<Response<bool>> DeleteAsync(Predicate<User> predicate)
        {
            // checking user exists or not
            if (await repo.SelectAsync(x => predicate(x)) is null)
            {
                return new Response<bool>();
            }

            await repo.DeleteAsync(predicate);

            return new Response<bool>()
            {
                StatusCode = 200,
                Result = true,
                Message = "Ok"
            };
        }

        public async Task<Response<List<User>>> GetAllAsync(Predicate<User> predicate)
        {
            return new Response<List<User>>()
            {
                StatusCode = 200,
                Message = "Ok",
                Result = await repo.SelectAllAsync(x => predicate(x))
            };
        }

        public async Task<Response<User>> GetAsync(Predicate<User> predicate)
        {
            var user = await repo.SelectAsync(x => predicate(x));
            
            if (user is null)
            {
                return new Response<User>();
            }

            return new Response<User>()
            {
                StatusCode = 200,
                Message = "Ok",
                Result = user
            };
        }

        public async Task<Response<List<Podcast>>> GetFavouritePodcastsAsync(long userId)
        {
            var user = await repo.SelectAsync(x => x.Id == userId);

            var result = new List<Podcast>();

            foreach (var p in user.Details.FavouritePodcastIds)
            {
                var podcast = await podcastRepo.SelectAsync(x => x.Id == p);
                result.Add(podcast);
            }

            return new Response<List<Podcast>>()
            {
                StatusCode = 200,
                Message = "Ok",
                Result = result
            };
        }

        public async Task<Response<List<Word>>> GetFavouriteWordsAsync(long userId)
        {
            var user = await repo.SelectAsync(x => x.Id == userId);

            var result = new List<Word>();

            foreach (var w in user.Details.FavouriteWordIds)
            {
                var word = await wordRepo.SelectAsync(x => x.ChosenLanguageId == w);
                result.Add(word);
            }

            return new Response<List<Word>>()
            {
                StatusCode = 200,
                Message = "Ok",
                Result = result
            };
        }

        public async Task<Response<List<Word>>> GetTodaysDailyVocabs(long userId)
        {
            var allVocabs = await wordRepo.SelectAllAsync();
            var numberOftotalAvailableVocab = allVocabs.Count;
            var user = await repo.SelectAsync(x => x.Id == userId);
            var dayX = (DateTime.UtcNow - user.CreatedAt).TotalDays;
            var perDay = user.Details.NumberOfWordsPerDay;
            int beginningIndex = 0;

            // pagination beginning index
            if ((dayX + 1) * perDay >= numberOftotalAvailableVocab)
            {
                Random random = new Random();
                beginningIndex = random.Next(0, numberOftotalAvailableVocab - 1 - perDay);
            }
            else
            {
                beginningIndex = (int)dayX * perDay;
            }

            var result = new List<Word>();

            for (int i = beginningIndex; i < perDay; i++)
            {
                result.Add(allVocabs[i]);
            }

            return new Response<List<Word>>()
            {
                StatusCode = 200,
                Message = "Ok",
                Result = result
            };
        }


        public async Task<Response<User>> UpdateAsync(long id, User user)
        {
            var entity = await repo.SelectAsync(x => x.Id == id);

            if (entity is null)
            {
                return new Response<User>();
            }

            await repo.UpdateAsync(id, user);

            return new Response<User>()
            {
                StatusCode = 200,
                Message = "Ok",
                Result = entity
            };
        }


    }
}
