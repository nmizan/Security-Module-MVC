using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Security_Module.Models
{
    public class RoleAssignUser
    {
        [Key]
        public int RaId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public UserRegistration User { get; set; }
    }
}