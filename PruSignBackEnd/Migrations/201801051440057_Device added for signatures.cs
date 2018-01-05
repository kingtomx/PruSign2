namespace PruSignBackEnd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Deviceaddedforsignatures : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Signatures", "DeviceID", c => c.Int(nullable: false));
            CreateIndex("dbo.Signatures", "DeviceID");
            AddForeignKey("dbo.Signatures", "DeviceID", "dbo.Devices", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Signatures", "DeviceID", "dbo.Devices");
            DropIndex("dbo.Signatures", new[] { "DeviceID" });
            DropColumn("dbo.Signatures", "DeviceID");
        }
    }
}
