using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Security_Module.ViewModel;
using Security_Module.Models;
using Security_Module.Utill;

namespace Security_Module.Controllers
{
    public class RegistrationController : Controller
    {
        private SecurityDbContext db = new SecurityDbContext();
        private EncryptionDecryptionUtil encryptionDecryptionUtil = new EncryptionDecryptionUtil();
        private int saltLength = 5;

        //public string CheckUserName(string input)
        //{
        //    var ifuser = db.User.Where(x=>x.UserName==input).Select(y=>y.UserName).FirstOrDefault();
        //    if (ifuser != null)
        //    {
        //        return "Available";
        //    }
        //    else
        //    {
        //        return "Not Available";
        //    }
        //    return "";
        //}
        public ActionResult Index()
        {
        
        
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include="UserId,UserName,Password,ConfirmPassword,Salt,FirstName,LastName,Email,Phone,Address,SecurityQuestion,SecurityQuestionAnswer")] RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserRegistration user = new UserRegistration();
                user.UserName = model.UserName;
                user.Salt = encryptionDecryptionUtil.GenerateSalt(saltLength);
                user.Password = encryptionDecryptionUtil.CreatePasswordHash(model.Password, user.Salt);
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Phone = model.Phone;
                user.Address = model.Address;
                user.Email = model.Email;
                user.IsActive = false;
                user.LastLogin = DateTime.Now;
                user.SecurityQuestion = model.SecurityQuestion;
                user.SecurityQuestionAnswer = model.SecurityQuestionAnswer;

                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }


        public ActionResult ApproveList()
        {
            var list = db.User.Where(x => x.IsActive == false);
            return View(list.ToList());
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        // GET: 
        public ActionResult Action(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRegistration userregistration = db.User.Find(id);
            RoleAssignUser roleassignuser = new RoleAssignUser();
            if (userregistration == null)
            {
                return HttpNotFound();
            }

            ViewBag.RoleId = new SelectList(db.Roles, "RoleId", "RoleName", roleassignuser.RoleId);

            return View(userregistration);
        }

        // POST: /ssss/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Action([Bind(Include = "UserId,UserName,Password,Salt,FirstName,LastName,Email,Phone,Address,SecurityQuestion,SecurityQuestionAnswer,IsActive,LastLogin")] UserRegistration userregistration, int Roles)
        {
            RoleAssignUser roleassignuser = new RoleAssignUser();  
            if (ModelState.IsValid)
            {
                
                db.Entry(userregistration).State = EntityState.Modified;
                roleassignuser.RoleId = Roles;
                roleassignuser.UserId = userregistration.UserId;
                db.RoleAssignUser.Add(roleassignuser);
                db.SaveChanges();
                return RedirectToAction("ApproveList");
            }
            ViewBag.RoleId = new SelectList(db.Roles, "RoleId", "RoleName", roleassignuser.RoleId);

            return View(userregistration);
        }

        public ActionResult WhiteList()
        {
            var list = db.User.Where(x => x.IsActive == true);
            return View(list.ToList());
        }
        public ActionResult EditWhiteList(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRegistration userregistration = db.User.Find(id);
            RoleAssignUser roleassignuser = new RoleAssignUser();
            if (userregistration == null)
            {
                return HttpNotFound();
            }

            ViewBag.RoleId = new SelectList(db.Roles, "RoleId", "RoleName", roleassignuser.RoleId);

            return View(userregistration);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditWhiteList([Bind(Include = "UserId,UserName,Password,Salt,FirstName,LastName,Email,Phone,Address,SecurityQuestion,SecurityQuestionAnswer,IsActive,LastLogin")] UserRegistration userregistration, int Roles)
        {
            RoleAssignUser roleassignuser = new RoleAssignUser();
            if (ModelState.IsValid)
            {
                db.Database.ExecuteSqlCommand("delete from RoleAssignUsers where UserId='" + userregistration.UserId + "'");
                db.Entry(userregistration).State = EntityState.Modified;
                roleassignuser.RoleId = Roles;
                roleassignuser.UserId = userregistration.UserId;
                db.RoleAssignUser.Add(roleassignuser);
                db.SaveChanges();
                return RedirectToAction("WhiteList");
            }
            ViewBag.RoleId = new SelectList(db.Roles, "RoleId", "RoleName", roleassignuser.RoleId);

            return View(userregistration);
        }


    }
}
