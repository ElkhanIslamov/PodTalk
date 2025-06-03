using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PodTalk.DataContext.Entities;

namespace PodTalk.DataContext
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Episode> Episodes { get; set; } = null!;
        public DbSet<Speaker> Speakers { get; set; } = null!;
        public DbSet<Topic> Topics { get; set; } = null!;
        public DbSet<SpeakerTopic> SpeakerTopics { get; set; } = null!;


    }
}
