namespace PruSignBackEnd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDeviceLogentity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeviceLogs", "Message", c => c.String());
            AddColumn("dbo.DeviceLogs", "StackTrace", c => c.String());
            AddColumn("dbo.DeviceLogs", "FormattedDate", c => c.String());
            DropColumn("dbo.DeviceLogs", "Details");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DeviceLogs", "Details", c => c.String());
            DropColumn("dbo.DeviceLogs", "FormattedDate");
            DropColumn("dbo.DeviceLogs", "StackTrace");
            DropColumn("dbo.DeviceLogs", "Message");
        }
    }
}
