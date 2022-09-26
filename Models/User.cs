using Microsoft.AspNetCore.Identity;

namespace SocialMediaMustBeGood2.Models
{
    public class User : IdentityUser<int>
    {
        public string Login { get; set; }
        public string Avatar { get; set; }
        public string UserInfo { get; set; }
        public string Sex { get; set; }
        public uint PublicationsCount { get; set; }
        public uint SubscribesCount { get; set; }
        public uint SubscribersCount { get; set; }
    }
}
