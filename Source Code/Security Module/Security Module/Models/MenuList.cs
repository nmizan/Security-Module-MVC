using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Security_Module.Models
{
    public class MenuList
    {
        [Key]
        public int MenuId { get; set; }
        [Display(Name = "Menu Name")]
        public string MenuName { get; set; }
        public string Url { get; set; }
    }
}