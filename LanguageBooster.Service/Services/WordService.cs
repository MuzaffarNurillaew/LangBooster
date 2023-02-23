using LanguageBooster.Data.IRepositories;
using LanguageBooster.Data.Repositories;
using LanguageBooster.Domain.Entities;
using LanguageBooster.Service.Helpers;
using LanguageBooster.Service.Interfaces;
using System.Diagnostics;

namespace LanguageBooster.Service.Services
{
    public class WordService : IWordService
    {
        private readonly IRepository<Word> wordRepo = new Repository<Word>(new Word());
        private readonly long languageId;
        public WordService(long languageId)
        {
            this.languageId = languageId;
            wordRepo = new Repository<Word>(new Word()
            {
                ChosenLanguageId = languageId
            });
        }
        public async Task<Response<Word>> AddAsync(Word word)
        {
            await wordRepo.CreateAsync(word);

            return new Response<Word>()
            {
                StatusCode = 200,
                Message = "Ok",
                Result = word
            };
        }

        public async Task<Response<List<Word>>> GetAllAsync(Predicate<Word> predicate = null)
        {
            return new Response<List<Word>>()
            {
                StatusCode = 200,
                Message = "Ok",
                Result = await wordRepo.SelectAllAsync(x => predicate(x))
            };
        }

        public async Task<Response<Word>> GetAsync(Predicate<Word> predicate)
        {
            var entity = await wordRepo.SelectAsync(x => predicate(x));
            if (entity is null)
            {
                return new Response<Word>();
            }

            return new Response<Word>()
            {
                StatusCode = 200,
                Message = "Ok",
                Result = entity
            };
        }

        public async Task PronunceAsync(Word word)
        {
            await File.WriteAllTextAsync("..\\..\\..\\..\\LanguageBooster.Service\\Helpers\\pronunciation.vbs", $"Createobject(\"SAPI.SpVoice\").Speak\"{word.ChosenLanguageTranslation}\"");
            var p = new ProcessStartInfo("..\\..\\..\\..\\LanguageBooster.Service\\Helpers\\voiceTrigger.bat");
            p.UseShellExecute = false;
            p.CreateNoWindow = true;
            Process.Start(p);
        }

        public async Task<Response<Word>> UpdateAsync(Word word)
        {
            var entity = await wordRepo.SelectAsync(x => x.Uzbek == word.Uzbek);

            if (entity is null)
            {
                return new Response<Word>();
            }

            await wordRepo.UpdateAsync(word.Id, word);

            return new Response<Word>()
            {
                StatusCode = 200,
                Message = "Ok",
                Result = entity
            };
        }
    }
}
