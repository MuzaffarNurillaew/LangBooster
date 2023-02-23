using LanguageBooster.Domain.Commons;

namespace LanguageBooster.Domain.Entities
{
    public class Podcast : Auditable
    {
        public long LanguageId { get; set; }
        public long OwnerId { get; set; }
        public string Name { get; set; }
        public string FileLocation { get; set; }
        public string Description { get; set; }
    }
}
