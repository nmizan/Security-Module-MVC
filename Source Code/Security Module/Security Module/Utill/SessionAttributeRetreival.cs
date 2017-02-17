using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Security_Module.Models;
using System.Web.Security;

namespace Security_Module.Utill
{
    public class SessionAttributeRetreival
    {
        private SecurityDbContext db = new SecurityDbContext();
        public RoleAssignUser getStoredUserPermission()
        {
            string username = FormsAuthentication.Decrypt(HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            UserRegistration appuser = db.User.SingleOrDefault(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
            try
            {
                RoleAssignUser userpermission = db.RoleAssignUser.SingleOrDefault(u => u.UserId == appuser.UserId);
                return userpermission;
            }
            catch (NullReferenceException exp)
            {
                return null;
            }

        }
    }
}