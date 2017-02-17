using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Security_Module.Models
{
    public class UserRegistration
    {
     
            [Key]
            public int UserId { get; set; }
            [Required]
            [MaxLength(155)]
            [Display(Name = "User Name")]
            public string UserName { get; set; }
            [Required]
            [MaxLength(155)]
            [DataType(DataType.Password)]
            public string Password { get; set; }
            [MaxLength(15)]
            public string Salt { get; set; }
            [Required]
            [MaxLength(255)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            [Required]
            [MaxLength(255)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
            [Required]
            [MaxLength(255)]
            public string Email { get; set; }
            [Required]
            [MaxLength(100)]
            public string Phone { get; set; }
            [Required]
            [MaxLength(255)]
            public string Address { get; set; }
            [Required]
            [MaxLength(255)]
            public string SecurityQuestion { get; set; }
            [Required]
            [MaxLength(255)]
            public string SecurityQuestionAnswer { get; set; }
            [Required]
            [Display(Name = "Is Active")]
            public Boolean IsActive { get; set; }

            [DataType(DataType.Date)]
            [Display(Name = "Last Login")]
            public DateTime ? LastLogin { get; set; }

             [Display(Name = "Full Name")]
            public string fullname
            {
                get { return FirstName+" " + LastName; }
            }
        
    }
}