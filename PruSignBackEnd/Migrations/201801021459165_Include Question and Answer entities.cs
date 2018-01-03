namespace PruSignBackEnd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncludeQuestionandAnswerentities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        Device = c.String(),
                        Data = c.String(),
                        Question_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Questions", t => t.Question_ID)
                .Index(t => t.Question_ID);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        Code = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Answers", "Question_ID", "dbo.Questions");
            DropIndex("dbo.Answers", new[] { "Question_ID" });
            DropTable("dbo.Questions");
            DropTable("dbo.Answers");
        }
    }
}
