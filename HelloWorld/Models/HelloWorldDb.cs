using System.Data.Entity;
using HelloWorld.Migrations;

namespace HelloWorld.Models
{
    public class HelloWorldDb : DbContext
    {
        public HelloWorldDb()
            : base("name=DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<TagChapters> TagChapterses { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<HelloWorldDb, Configuration>());
        }

    }
}