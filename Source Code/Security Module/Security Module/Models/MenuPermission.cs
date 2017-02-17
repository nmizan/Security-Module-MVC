using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Security_Module.Models
{
    public class MenuPermission
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int MenuId { get; set; }
        public bool? IsVisible { get; set; }
        public Role Role { get; set; }
        public MenuList MenuList { get; set; }
    }
}