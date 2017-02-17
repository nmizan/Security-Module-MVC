namespace Security_Module.Migrations
{
    using Security_Module.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Security_Module.Models.SecurityDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Security_Module.Models.SecurityDbContext context)
        {
            context.MenuList.AddOrUpdate(m => m.MenuId,

             new MenuList { MenuId = 1, MenuName = "Core" },
             new MenuList { MenuId = 2, MenuName = "Head Office" },
             new MenuList { MenuId = 3, MenuName = "Branch Office" },
             new MenuList { MenuId = 4, MenuName = "District" },
             new MenuList { MenuId = 5, MenuName = "Sales" },
             new MenuList { MenuId = 6, MenuName = "Product Sale" },
             new MenuList { MenuId = 7, MenuName = "Product Delivary" },
             new MenuList { MenuId = 8, MenuName = "Service" },
             new MenuList { MenuId = 9, MenuName = "Job Card" },
             new MenuList { MenuId = 10, MenuName = "Customer Request" },
             new MenuList { MenuId = 11, MenuName = "Call Center" },
             new MenuList { MenuId = 12, MenuName = "Security" }

             );
        }
    }
}
