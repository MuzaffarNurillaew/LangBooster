using LanguageBooster.Data.IRepositories;
using LanguageBooster.Data.Repositories;
using LanguageBooster.Domain.Entities;
using LanguageBooster.Service.Interfaces;
using LanguageBooster.Service.Services;

namespace LanguageBooster.Presentation
{
    class Program
    {
        private static User currentUser;
        //private static IRepository<Word> enRepo = new Repository<Word>(new Word() { ChosenLanguageId = 1 });
        //private static IRepository<Word> uzRepo = new Repository<Word>(new Word() { ChosenLanguageId = 0 });
        static async Task Main()
        {
            currentUser = await RegistrationUI.Registration();

            var dashboard = new Dashboard(currentUser);
            await dashboard.PrintDashboard(currentUser);
        }
    }
}