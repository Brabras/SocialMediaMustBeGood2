using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace SocialMediaMustBeGood2.Models
{
    public class SocialMediaContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<Publication> Publications { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Subscribe> Subscriptions { get; set; }

        public SocialMediaContext(DbContextOptions<SocialMediaContext> options) : base(options) { }

    }
}
