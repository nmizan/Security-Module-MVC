using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Security_Module.Models;
using Security_Module.Utill;
using System.Web.Security;
using System.Data.Entity;

namespace Security_Module.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        private SecurityDbContext db = new SecurityDbContext();
        private EncryptionDecryptionUtil encryptionDecryptionUtil = new EncryptionDecryptionUtil();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Index(Login loginModel, string returnUrl)
        {

            if (loginModel.USERNAME == null || loginModel.PASSWARD == null || loginModel.USERNAME.Trim().Equals("") || loginModel.PASSWARD.Trim().Equals(""))
            {
                ModelState.AddModelError("", "Wrong Username or Password");
            }
            List<UserRegistration> appusers = db.User.ToList();
            foreach (var appuser in appusers)
            {
                if (appuser.UserName.Equals(loginModel.USERNAME) && encryptionDecryptionUtil.VerifyPassword(appuser.Password, loginModel.PASSWARD, appuser.Salt))
                {

                    FormsAuthentication.SetAuthCookie(loginModel.USERNAME, false);

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {

                        RoleAssignUser userpermission = db.RoleAssignUser.SingleOrDefault(u => u.UserId == appuser.UserId);
                        if (userpermission == null)
                        {
                            FormsAuthentication.SignOut();
                            return RedirectToAction("AccessDenied", "Error", null);
                        }
                        appuser.LastLogin = DateTime.Now;
                        db.Entry(appuser).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            if (ModelState.IsValid)
            {
                ModelState.AddModelError("", "Wrong Username or Password");
            }
            return View(loginModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
	}
}