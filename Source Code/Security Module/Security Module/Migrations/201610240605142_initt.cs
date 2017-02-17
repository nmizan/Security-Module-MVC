namespace Security_Module.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initt : DbMigration
    {
        public override void Up()
        {
           
            
            CreateTable(
                "dbo.ResetTickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        TokenHash = c.String(),
                        Expiration = c.DateTime(nullable: false),
                        TokenUsed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
         
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RoleAssignUsers", "UserId", "dbo.UserRegistrations");
            DropForeignKey("dbo.RoleAssignUsers", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.MenuPermissions", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.MenuPermissions", "MenuId", "dbo.MenuLists");
            DropIndex("dbo.RoleAssignUsers", new[] { "RoleId" });
            DropIndex("dbo.RoleAssignUsers", new[] { "UserId" });
            DropIndex("dbo.MenuPermissions", new[] { "MenuId" });
            DropIndex("dbo.MenuPermissions", new[] { "RoleId" });
            DropTable("dbo.UserRegistrations");
            DropTable("dbo.RoleAssignUsers");
            DropTable("dbo.ResetTickets");
            DropTable("dbo.RegistrationViewModels");
            DropTable("dbo.Roles");
            DropTable("dbo.MenuPermissions");
            DropTable("dbo.MenuLists");
        }
    }
}
