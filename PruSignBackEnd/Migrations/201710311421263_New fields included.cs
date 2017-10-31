namespace PruSignBackEnd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Newfieldsincluded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Points", "IsEmpty", c => c.Boolean(nullable: false));
            AddColumn("dbo.Points", "X", c => c.Single(nullable: false));
            AddColumn("dbo.Points", "Y", c => c.Single(nullable: false));
            AddColumn("dbo.PointWhens", "When", c => c.Long(nullable: false));
            AddColumn("dbo.PointWhens", "Point_ID", c => c.Int());
            AddColumn("dbo.PointWhens", "Signature_ID", c => c.Int());
            AddColumn("dbo.Signatures", "Image", c => c.String());
            AddColumn("dbo.Signatures", "CustomerName", c => c.String());
            AddColumn("dbo.Signatures", "CustomerId", c => c.String());
            AddColumn("dbo.Signatures", "DocumentId", c => c.String());
            AddColumn("dbo.Signatures", "ApplicationId", c => c.String());
            AddColumn("dbo.Signatures", "Hash", c => c.String());
            CreateIndex("dbo.PointWhens", "Point_ID");
            CreateIndex("dbo.PointWhens", "Signature_ID");
            AddForeignKey("dbo.PointWhens", "Point_ID", "dbo.Points", "ID");
            AddForeignKey("dbo.PointWhens", "Signature_ID", "dbo.Signatures", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PointWhens", "Signature_ID", "dbo.Signatures");
            DropForeignKey("dbo.PointWhens", "Point_ID", "dbo.Points");
            DropIndex("dbo.PointWhens", new[] { "Signature_ID" });
            DropIndex("dbo.PointWhens", new[] { "Point_ID" });
            DropColumn("dbo.Signatures", "Hash");
            DropColumn("dbo.Signatures", "ApplicationId");
            DropColumn("dbo.Signatures", "DocumentId");
            DropColumn("dbo.Signatures", "CustomerId");
            DropColumn("dbo.Signatures", "CustomerName");
            DropColumn("dbo.Signatures", "Image");
            DropColumn("dbo.PointWhens", "Signature_ID");
            DropColumn("dbo.PointWhens", "Point_ID");
            DropColumn("dbo.PointWhens", "When");
            DropColumn("dbo.Points", "Y");
            DropColumn("dbo.Points", "X");
            DropColumn("dbo.Points", "IsEmpty");
        }
    }
}
