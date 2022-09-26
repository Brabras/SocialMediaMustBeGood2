using System.Collections.Generic;

namespace SocialMediaMustBeGood2.Models
{
    public class Publication
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string PictureLink { get; set; }
        public string Text { get; set; }

        public uint LikesCount { get; set; }
        public List<int> WhoLikedUserIds { get; set; }

        public uint CommentsCount { get; set; }
        public List<int> CommentsUsersId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
