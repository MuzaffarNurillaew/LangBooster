using LanguageBooster.Data.IRepositories;
using LanguageBooster.Data.Repositories;
using LanguageBooster.Domain.Entities;
using LanguageBooster.Service.Helpers;
using LanguageBooster.Service.Interfaces;

namespace LanguageBooster.Service.Services
{
    public class LanguageService : ILanguageService
    {
        private IRepository<Language> langRepo = new Repository<Language>();
        public async Task<Response<Language>> AddAsync(Language language)
        {

            language.LanguagePackPath = Path.GetFullPath($"..\\..\\..\\..\\LanguageBooster.Data\\Databases\\Languages\\{language.Name}");
            await langRepo.CreateAsync(language);

            Directory.CreateDirectory($"..\\..\\..\\..\\LanguageBooster.Data\\Databases\\Languages\\{language.Name}");
            await File.WriteAllTextAsync($"..\\..\\..\\..\\LanguageBooster.Data\\Databases\\Languages\\{language.Name}\\Podcasts.json", "[]");
            await File.WriteAllTextAsync($"..\\..\\..\\..\\LanguageBooster.Data\\Databases\\Languages\\{language.Name}\\Words.json", "[]");

            return new Response<Language>()
            {
                StatusCode = 200,
                Message = "Ok",
                Result = language
            };
        }

        public async Task<Response<bool>> DeleteAsync(Predicate<Language> predicate)
        {
            var isDeleted = await langRepo.DeleteAsync(x => predicate(x));
        
            if (isDeleted == false)
            {
                return new Response<bool>();
            }
            var language = await langRepo.SelectAsync(x => predicate(x));

            Directory.Delete(Path.GetFullPath($"..\\..\\..\\..\\LanguageBooster.Data\\Databases\\Languages\\{language.Name}"), true);

            return new Response<bool>()
            {
                StatusCode = 200,
                Message = "Ok",
                Result = isDeleted
            };
        }

        public async Task<Response<List<Language>>> GetAllAsync(Predicate<Language> predicate = null)
        {
            return new Response<List<Language>>()
            {
                StatusCode = 200,
                Message = "Ok",
                Result = await langRepo.SelectAllAsync(x => predicate(x))
            };
            
        }

        public async Task<Response<Language>> GetAsync(Predicate<Language> predicate)
        {
            var language = await langRepo.SelectAsync(x => predicate(x));

            if (language is null)
            {
                return new Response<Language>();
            }

            return new Response<Language>()
            {
                StatusCode = 200,
                Message = "Ok",
                Result = language
            };
        }
    }
}
