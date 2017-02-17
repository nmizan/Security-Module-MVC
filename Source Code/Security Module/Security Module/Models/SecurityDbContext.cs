using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Security_Module.Models
{
    public class SecurityDbContext:DbContext
    {
        public SecurityDbContext()
            : base("SecurityDb")
        {

        }
        public DbSet<UserRegistration> User { get; set; }

        public System.Data.Entity.DbSet<Security_Module.ViewModel.RegistrationViewModel> RegistrationViewModels { get; set; }

        public DbSet<Role> Roles { get; set; }
        public DbSet<MenuList> MenuList { get; set; }
        public DbSet<MenuPermission> MenuPermission { get; set; }
        public DbSet<RoleAssignUser> RoleAssignUser { get; set; }
        public DbSet<ResetTicket> ResetTicket { get; set; }


    }
}