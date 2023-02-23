using LanguageBooster.Domain.Commons;

namespace LanguageBooster.Domain.Entities
{
    public class User : Auditable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public long NativeLanguageId { get; set; }
        public long NewLanguageId { get; set; }
        public UserDetail Details { get; set; } = new UserDetail();
    }
}
