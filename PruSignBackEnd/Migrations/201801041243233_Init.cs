namespace PruSignBackEnd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
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
                        QuestionID = c.Int(nullable: false),
                        DeviceID = c.Int(nullable: false),
                        Data = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Devices", t => t.DeviceID, cascadeDelete: true)
                .ForeignKey("dbo.Questions", t => t.QuestionID, cascadeDelete: true)
                .Index(t => t.QuestionID)
                .Index(t => t.DeviceID);
            
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        Imei = c.String(),
                        User = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
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
            
            CreateTable(
                "dbo.DeviceLogs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        DeviceID = c.Int(nullable: false),
                        Message = c.String(),
                        StackTrace = c.String(),
                        ErrorLocation = c.String(),
                        LogDate = c.DateTime(nullable: false),
                        FormattedDate = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Devices", t => t.DeviceID, cascadeDelete: true)
                .Index(t => t.DeviceID);
            
            CreateTable(
                "dbo.Points",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        IsEmpty = c.Boolean(nullable: false),
                        X = c.Single(nullable: false),
                        Y = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PointWhens",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        When = c.Long(nullable: false),
                        Point_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Points", t => t.Point_ID)
                .Index(t => t.Point_ID);
            
            CreateTable(
                "dbo.Signatures",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        CustomerName = c.String(),
                        CustomerId = c.String(),
                        DocumentId = c.String(),
                        ApplicationId = c.String(),
                        SignatureObject = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SystemLogs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        StackTrace = c.String(),
                        ErrorLocation = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PointWhens", "Point_ID", "dbo.Points");
            DropForeignKey("dbo.DeviceLogs", "DeviceID", "dbo.Devices");
            DropForeignKey("dbo.Answers", "QuestionID", "dbo.Questions");
            DropForeignKey("dbo.Answers", "DeviceID", "dbo.Devices");
            DropIndex("dbo.PointWhens", new[] { "Point_ID" });
            DropIndex("dbo.DeviceLogs", new[] { "DeviceID" });
            DropIndex("dbo.Answers", new[] { "DeviceID" });
            DropIndex("dbo.Answers", new[] { "QuestionID" });
            DropTable("dbo.SystemLogs");
            DropTable("dbo.Signatures");
            DropTable("dbo.PointWhens");
            DropTable("dbo.Points");
            DropTable("dbo.DeviceLogs");
            DropTable("dbo.Questions");
            DropTable("dbo.Devices");
            DropTable("dbo.Answers");
        }
    }
}
