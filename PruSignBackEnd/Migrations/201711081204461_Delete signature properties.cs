namespace PruSignBackEnd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Deletesignatureproperties : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PointWhens", "Signature_ID", "dbo.Signatures");
            DropIndex("dbo.PointWhens", new[] { "Signature_ID" });
            AddColumn("dbo.DeviceLogs", "LogDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Signatures", "SignatureObject", c => c.String());
            DropColumn("dbo.DeviceLogs", "FormattedDate");
            DropColumn("dbo.PointWhens", "Signature_ID");
            DropColumn("dbo.Signatures", "Image");
            DropColumn("dbo.Signatures", "Hash");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Signatures", "Hash", c => c.String());
            AddColumn("dbo.Signatures", "Image", c => c.String());
            AddColumn("dbo.PointWhens", "Signature_ID", c => c.Int());
            AddColumn("dbo.DeviceLogs", "FormattedDate", c => c.String());
            DropColumn("dbo.Signatures", "SignatureObject");
            DropColumn("dbo.DeviceLogs", "LogDate");
            CreateIndex("dbo.PointWhens", "Signature_ID");
            AddForeignKey("dbo.PointWhens", "Signature_ID", "dbo.Signatures", "ID");
        }
    }
}
