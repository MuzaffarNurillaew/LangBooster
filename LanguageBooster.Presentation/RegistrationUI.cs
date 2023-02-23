using LanguageBooster.Domain.Entities;
using LanguageBooster.Service.Interfaces;
using LanguageBooster.Service.Services;

namespace LanguageBooster.Presentation
{
    public class RegistrationUI
    {
        private static IUserService userService = new UserService();
        private static User currentUser = null;
        public static async Task<User> Registration()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("1. Ro'yxatdan o'tish.\n" +
                    "2. Kirish.\n" +
                    "~. Dasturni to'xtatish");

                var choice = Console.ReadLine();

                if (choice == "1")
                {
                    await PrintSignUpAsync();
                }
                else if (choice == "2")
                {
                    await PrintLogInAsync();
                }
                else
                {
                    return null;
                }

                if (currentUser is not null)
                {
                    return currentUser;
                }

                Console.ReadKey();
            }
        }

        private static async Task PrintLogInAsync()
        {
            Console.Write("Username: ");
            var username = Console.ReadLine();
            Console.Write("Password: ");
            var password = Console.ReadLine();

            User user = (await userService.GetAsync(x => x.Username == username && x.Password == password)).Result;

            if (user is null)
            {
                Console.WriteLine("Username yoki password xato!");
                return;
            }

            currentUser = user;
            Console.WriteLine("Muvafaqqiyatli kirildi!");
        }

        private static async Task PrintSignUpAsync()
        {
            Console.Write("Ism: ");
            var name = Console.ReadLine();
            Console.Write("Familya: ");
            var lastName = Console.ReadLine();

            ILanguageService languageService = new LanguageService();
            var allLangs = (await languageService.GetAllAsync(x => true)).Result;
            
            for (int i = 1; i <= allLangs.Count; i++)
            {
                Console.WriteLine($"{i}. {allLangs[i - 1].Name}");
            }
            
            Console.Write("Ona tilini tanlang:");
            var nativeLang = long.Parse(Console.ReadLine());
            var nativeLangId = allLangs[(int)nativeLang - 1].Id;


            for (int i = 1; i <= allLangs.Count; i++)
            {
                Console.WriteLine($"{i}. {allLangs[i - 1].Name}");
            }

            Console.Write("Yangi tilini tanlang:");
            var newLang = long.Parse(Console.ReadLine());
            var newLangId = allLangs[(int)newLang - 1].Id;

            Console.Write("Username: ");
            var username = Console.ReadLine();
            
            Console.Write("Parol: ");
            var password = Console.ReadLine();

            var response = await userService.AddAsync(new User()
            {
                FirstName = name,
                LastName = lastName,
                NativeLanguageId = nativeLangId,
                NewLanguageId = newLangId,
                Username = username,
                Password = password
            });


            if (response.StatusCode == 404)
            {
                Console.WriteLine("Bunaqa username'da foydalanuvchi mavjud.");
                return;
            }

            currentUser = response.Result;
            Console.WriteLine("Muvafaqqiyatli ro'yxatdan o'tildi!");
        }
    }
}
