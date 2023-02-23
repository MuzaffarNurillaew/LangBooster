namespace LanguageBooster.Domain.Entities
{
    public class UserDetail
    {
        public int NumberOfWordsPerDay { get; set; } = 5;
        public List<long> FavouriteWordIds { get; set; } = new List<long>();
        public List<long> FavouritePodcastIds { get; set; } = new List<long>();
    }
}
