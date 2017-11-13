namespace PruSignBackEnd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgregacolumnadeErrorLocationenSystemLogs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SystemLogs", "ErrorLocation", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SystemLogs", "ErrorLocation");
        }
    }
}
