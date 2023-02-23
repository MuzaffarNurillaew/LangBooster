using LanguageBooster.Data.IRepositories;
using LanguageBooster.Data.Repositories;
using LanguageBooster.Domain.Entities;
using LanguageBooster.Service.Helpers;
using LanguageBooster.Service.Interfaces;
using System.Diagnostics;
using System.Text;

namespace LanguageBooster.Service.Services
{
    public class PodcastService : IPodcastService
    {
        private IRepository<Podcast> podcastRepo = new Repository<Podcast>(new Podcast());
        private long languaId;
        public PodcastService(Podcast podcast)
        {
            podcastRepo = new Repository<Podcast>(new Podcast()
            {
                LanguageId = podcast.LanguageId
            });
            languaId = podcast.LanguageId;
        }
        public async Task<Response<Podcast>> AddAsync(string filePath, Podcast podcast)
        {
            var fileName = filePath.Split("\\")[filePath.Split("\\").Length - 1];


            
            var podcastsPath = Path.GetFullPath($"..\\..\\..\\..\\LanguageBooster.Data\\Databases\\Podcasts\\{fileName}");
            await File.WriteAllTextAsync("..\\..\\..\\..\\LanguageBooster.Service\\Helpers\\FileCopier.bat", $"copy {filePath} {podcastsPath}");
            
            podcast.FileLocation = podcastsPath;
            podcast.LanguageId = languaId;
            await podcastRepo.CreateAsync(podcast);


            ProcessStartInfo p = new ProcessStartInfo("..\\..\\..\\..\\LanguageBooster.Service\\Helpers\\FileCopier.bat");
            p.UseShellExecute = false;
            p.CreateNoWindow = true;

            Process.Start(p);

            return new Response<Podcast>()
            {
                StatusCode = 200,
                Message = "Ok",
                Result = podcast,
            };
        }

        public async Task<Response<bool>> DeleteAsync(Predicate<Podcast> predicate)
        {
            bool isDeleted = await podcastRepo.DeleteAsync(x => predicate(x));
            if (!isDeleted)
            {
                return new Response<bool>();
            }

            return new Response<bool>()
            {
                StatusCode = 200,
                Message = "Ok",
                Result = true
            };
        }

        public async Task<Response<List<Podcast>>> GetAllAsync(Predicate<Podcast> predicate = null)
        {
            var podcasts = await podcastRepo.SelectAllAsync(x => predicate(x));

            return new Response<List<Podcast>>()
            {
                StatusCode = 200,
                Message = "Ok",
                Result = podcasts
            };
        }

        public async Task<Response<Podcast>> GetAsync(Predicate<Podcast> predicate)
        {
            var entity = await podcastRepo.SelectAsync(x => predicate(x));

            if (entity is null)
            {
                return new Response<Podcast>();
            }

            return new Response<Podcast>()
            {
                StatusCode = 200,
                Message = "Ok",
                Result = entity
            };
        }

        public void Play(Podcast podcast)
        {
            string mediaPlayerPath = "C:\\Program Files (x86)\\Windows Media Player\\wmplayer.exe";
            string podcastPath = podcast.FileLocation;

            var p = new ProcessStartInfo(mediaPlayerPath, podcastPath);

            Process.Start(p);
        }

        public async Task<Response<Podcast>> UpdateAsync(Podcast podcast)
        {
            var entity = await podcastRepo.SelectAsync(x => x.Id == podcast.Id);

            if (entity is null)
            {
                return new Response<Podcast>();
            }

            var oldEntity = await podcastRepo.UpdateAsync(podcast.Id, podcast);

            return new Response<Podcast>()
            {
                StatusCode = 200,
                Message = "Ok",
                Result = oldEntity
            };
        }


    }
}
