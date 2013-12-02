namespace HelloWorld.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        BookId = c.Int(nullable: false, identity: true),
                        BookName = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookId)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Chapters",
                c => new
                    {
                        ChapterId = c.Int(nullable: false, identity: true),
                        ChapterName = c.String(),
                        ChapterContent = c.String(),
                        BookId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ChapterId)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .Index(t => t.BookId);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        TagId = c.Int(nullable: false, identity: true),
                        TagContent = c.String(),
                    })
                .PrimaryKey(t => t.TagId);
            
            CreateTable(
                "dbo.Votes",
                c => new
                    {
                        VoteId = c.Int(nullable: false, identity: true),
                        Like = c.Byte(nullable: false),
                        Dislike = c.Byte(nullable: false),
                        ChapterId = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.VoteId)
                .ForeignKey("dbo.Chapters", t => t.ChapterId, cascadeDelete: true)
                .Index(t => t.ChapterId);
            
            CreateTable(
                "dbo.TagChapters",
                c => new
                    {
                        Tag_TagId = c.Int(nullable: false),
                        Chapter_ChapterId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_TagId, t.Chapter_ChapterId })
                .ForeignKey("dbo.Tags", t => t.Tag_TagId, cascadeDelete: true)
                .ForeignKey("dbo.Chapters", t => t.Chapter_ChapterId, cascadeDelete: true)
                .Index(t => t.Tag_TagId)
                .Index(t => t.Chapter_ChapterId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.TagChapters", new[] { "Chapter_ChapterId" });
            DropIndex("dbo.TagChapters", new[] { "Tag_TagId" });
            DropIndex("dbo.Votes", new[] { "ChapterId" });
            DropIndex("dbo.Chapters", new[] { "BookId" });
            DropIndex("dbo.Books", new[] { "UserId" });
            DropForeignKey("dbo.TagChapters", "Chapter_ChapterId", "dbo.Chapters");
            DropForeignKey("dbo.TagChapters", "Tag_TagId", "dbo.Tags");
            DropForeignKey("dbo.Votes", "ChapterId", "dbo.Chapters");
            DropForeignKey("dbo.Chapters", "BookId", "dbo.Books");
            DropForeignKey("dbo.Books", "UserId", "dbo.UserProfile");
            DropTable("dbo.TagChapters");
            DropTable("dbo.Votes");
            DropTable("dbo.Tags");
            DropTable("dbo.Chapters");
            DropTable("dbo.Books");
            DropTable("dbo.UserProfile");
        }
    }
}
