namespace SocialMediaMustBeGood2.Models
{
    public class Subscribe
    {
        public int Id { get; set; }

        public int WhoSubscribesId { get; set; }
        public User WhoSubscribes { get; set; }

        public int ToWhoSubscribesId { get; set; }
        public User ToWhoSubscribes { get; set; }

    }
}
