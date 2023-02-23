using LanguageBooster.Domain.Commons;

namespace LanguageBooster.Domain.Entities
{
    public class Language : Auditable
    {
        public string Name { get; set; }
        public string LanguagePackPath { get; set; }
    }
}
