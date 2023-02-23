using LanguageBooster.Domain.Commons;

namespace LanguageBooster.Domain.Entities
{
    public class Word : Auditable
    {
        public long ChosenLanguageId { get; set; }
        public string Uzbek { get; set; }
        public string ChosenLanguageTranslation { get; set; }
        public string UrlToCambridgeDictionary { get; set; }
    }
}
