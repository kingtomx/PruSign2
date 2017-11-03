namespace PruSignBackEnd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddsErrorLocationfield : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeviceLogs", "ErrorLocation", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DeviceLogs", "ErrorLocation");
        }
    }
}
