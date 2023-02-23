using LanguageBooster.Domain.Entities;
using LanguageBooster.Service.Interfaces;
using LanguageBooster.Service.Services;

namespace LanguageBooster.Presentation
{
    public class Dashboard
    {
        private static User currentUser;
        private static long languageid;
        private static IUserService userService;
        private static IPodcastService podcastService;
        private static ILanguageService languageService = new LanguageService();

        public Dashboard(User user)
        {
            currentUser = user;
            languageid = user.NewLanguageId;
            userService = new UserService(languageid);
            podcastService = new PodcastService(new Podcast()
            {
                LanguageId = languageid
            });
        }
        public async Task PrintDashboard(User currentUser1)
        {
            currentUser = currentUser1;
            while (true)
            {
                Console.Clear();

                Console.WriteLine("\tBarcha funksionallik shu yerga qo'shilgan: COMMON DASHBOARD\n" +
                    "1. Language pack (framini) qo'shish.\n" +
                    "2. Language pack o'chirish.\n" +
                    "3. Language pack'larni ko'rish\n" +
                    "4. Podcast qo'shish.\n" +
                    "6. Podcastni favouritelarga qo'shish\n" +
                    "7. Bugungi so'zlar\n" +
                    "8. So'z qidirish\n" +
                    "9. Favourite so'zlarni ko'rsatish\n" +
                    "10. Favourite podcastlarni ko'rsatish\n");


                Console.Write("Tanlang: ");
                string ch = Console.ReadLine();

                if (ch == "1")
                {
                    await printAddLangAsync();
                }
                else if (ch == "2")
                {
                    await printRemoveLangAsync();
                }
                else if (ch == "3")
                {
                    await printLangsAsync();
                }
                else if (ch == "4")
                {
                    await printAddPodcastAsync();
                }
                else if (ch == "6")
                {
                    await printAddToFavouritesAsync();
                }
                else if (ch == "7")
                {
                    await printDailyVocabAsync();
                }
                else if (ch == "8")
                {
                    await printSearchWordAsync();
                }
                else if (ch == "9")
                {
                    await printFavouriteVocabAsync();
                }
                else if (ch == "10")
                {
                    await printFavouritePodcastsAsync();
                }

                Console.ReadKey();
            }
        }

        #region 10 finished
        private static async Task printFavouritePodcastsAsync()
        {
            var podcasts = (await userService.GetFavouritePodcastsAsync(currentUser.Id)).Result;
        
            for (int i = 0; i < podcasts.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {podcasts[i].Name}");
            }

            Console.Write("Choose: ");
            int choice = int.Parse(Console.ReadLine());

            var podcastToDo = podcasts[choice - 1];

            Console.WriteLine("1. Play\n" +
                "2. Exit\n");


            var ch1 = Console.ReadLine();

            if (ch1 == "1")
            {
                podcastService.Play(podcastToDo);
            }
            else if (ch1 == "2")
            {
                return;
            }
        }
        #endregion
        private static Task printFavouriteVocabAsync()
        {
            throw new NotImplementedException();
        }

        private static Task printSearchWordAsync()
        {
            throw new NotImplementedException();
        }

        private static Task printDailyVocabAsync()
        {
            throw new NotImplementedException();
        }

        #region 6 finished
        private async static Task printAddToFavouritesAsync()
        {
            var podcasts = (await podcastService.GetAllAsync(x => true)).Result;

            for (int i = 0; i < podcasts.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {podcasts[i].Name}");
            }

            Console.Write("Choose: ");
            int choice = int.Parse(Console.ReadLine());

            var podcastToDo = podcasts[choice - 1];

            Console.WriteLine("1. Play\n" +
                "2. Add To favourites\n");

            var ch1 = Console.ReadLine();

            if (ch1 == "1")
            {
                podcastService.Play(podcastToDo);
            }
            else if (ch1 == "2")
            {
                await userService.AddFavouritePodcast(currentUser.Id, podcastToDo.Id);
            }
        }
        #endregion
        
        #region 4 finished
        private async static Task printAddPodcastAsync()
        {
            Console.Write("Podkast qayerda joylashganini to'liq kiriting: ");
            var fullPodcastPath = Console.ReadLine();

            try
            {
                var response = await podcastService.AddAsync(fullPodcastPath, new Podcast()
                {
                    Name = fullPodcastPath.Split("\\")[fullPodcastPath.Split("\\").Length - 1]
                });


            }
            catch (Exception)
            {
                Console.WriteLine("xato input");
                return;
            }

        }
        #endregion

        private static Task printLangsAsync()
        {
            throw new NotImplementedException();

        }

        private static async Task printRemoveLangAsync()
        {
            Console.Write("Language nomini kiriting: ");
            var name = Console.ReadLine();

            var response = await languageService.DeleteAsync(x => x.Name.ToLower() == name.ToLower());
        
            if (response.StatusCode == 404)
            {
                Console.WriteLine("Bunaqa til topilmadi");
                return;
            }
            Console.WriteLine("O'chirildi");

        }

        #region 1 finished
        private async static Task printAddLangAsync()
        {
            Console.Write("Tilni Paskal-caseda kiriting: ");
            var name = Console.ReadLine();

            await languageService.AddAsync(new Language()
            {
                Name = name
            });
        }
        #endregion
    }
}
