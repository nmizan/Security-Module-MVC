using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Security_Module.Models
{
    public class ResetTicket
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string TokenHash { get; set; }
        public DateTime Expiration { get; set; }
        public bool TokenUsed { get; set; }
    }
}