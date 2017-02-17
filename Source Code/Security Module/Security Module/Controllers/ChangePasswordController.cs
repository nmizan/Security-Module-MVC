using Security_Module.Models;
using Security_Module.Utill;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Security_Module.Controllers
{
    public class ChangePasswordController : Controller
    {
        private SecurityDbContext db = new SecurityDbContext();
        private EncryptionDecryptionUtil encryptionDecryptionUtil = new EncryptionDecryptionUtil();
        public ActionResult Index()
        {
            ViewBag.UserName = User.Identity.Name;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Index(ChangePassword changePassword, string returnUrl)
        {

            if (changePassword.UserName == null || changePassword.CurrentPassword == null || changePassword.NewPassword == null || changePassword.ConfirmPassword == null || changePassword.UserName.Trim().Equals("") || changePassword.CurrentPassword.Trim().Equals("") || changePassword.NewPassword.Trim().Equals("") || changePassword.ConfirmPassword.Trim().Equals(""))
            {
                ModelState.AddModelError("", "Wrong Username or Password");
            }
            List<UserRegistration> appusers = db.User.ToList();
            foreach (var appuser in appusers)
            {
                if (appuser.UserName.Equals(changePassword.UserName) && encryptionDecryptionUtil.VerifyPassword(appuser.Password, changePassword.CurrentPassword, appuser.Salt))
                {


                    appuser.Password = encryptionDecryptionUtil.CreatePasswordHash(changePassword.NewPassword, appuser.Salt);
                    db.Entry(appuser).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
            }
            if (ModelState.IsValid)
            {
                ModelState.AddModelError("", "Wrong Username or Password");
            }
            return View(changePassword);
        }
	}
}