using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Security_Module.Models;
using Security_Module.Utill;
using System.Data.Entity;
using System.Text;
using System.Net.Mail;
using System.Net;


namespace Security_Module.Controllers
{
    public class PasswordRecoveryController : Controller
    {

        private SecurityDbContext db = new SecurityDbContext();
        private EncryptionDecryptionUtil encryptionDecryptionUtil = new EncryptionDecryptionUtil();
        public ActionResult Identify()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Identify(string Email)
        {
            var result = db.User.Where(x => x.Email == Email).Select(y => y.Email).FirstOrDefault();

            if (result != null)
            {
                return RedirectToAction("Method", new { Email = Email });
            }
            else
            {
                if (ModelState.IsValid)
                {
                    ModelState.AddModelError("", "There is no account exists!");
                }
            }
            return View();
        }
        public ActionResult Method(string Email)
        {
            ViewBag.Email = Email;
          

            return View();
        }
        [HttpPost]
        public ActionResult Method(string Email,string Option)
        {
            ViewBag.Email = Email;
            if (Option == "question")
            {
                return RedirectToAction("SecurityQuestion", new { Email=Email});
            }
            else
            {
                return RedirectToAction("byEmail", new { Email = Email });
            }

         
        }
        public ActionResult SecurityQuestion(string Email)
        {

            ViewBag.Question = db.User.Where(x => x.Email == Email).Select(y => y.SecurityQuestion).FirstOrDefault();
            return View();
        }
        public ActionResult CheckSecurityQuestion(string Answer,string Email)
        {
          
            var result = db.User.Where(x => x.Email == Email).Select(y => y.SecurityQuestionAnswer).FirstOrDefault();

            if (result==Answer)
            {
                return RedirectToAction("PasswordReset", new { Email = Email });
            }
            else
            {
                if (ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Does not match your answer !");
                }
            }

            return View("SecurityQuestion");
        }

       
        public ActionResult PasswordReset(string Email)
        {
            ViewBag.Email = Email;
            return View();
        }
        [HttpPost]
        public ActionResult PasswordReset(string Email,ResetPassword rp)
        {
            if (ModelState.IsValid)
            {

                List<UserRegistration> appusers = db.User.ToList();
                foreach (var appuser in appusers)
                {
                    if (appuser.Email.Equals(Email))
                    {


                        appuser.Password = encryptionDecryptionUtil.CreatePasswordHash(rp.NewPassword, appuser.Salt);
                        db.Entry(appuser).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Confirmation");
                    }
                }

                if (ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Something gonna wrong!");
                }
            }
            return View(rp);
       
        }

        public ActionResult Confirmation()
        {

            return View();
        }

        public ActionResult ForgetPassword()
        {
            return View();
        }


        public ActionResult byEmail(string Email)
        {
            db.Database.ExecuteSqlCommand("delete from ResetTickets where Email='" + Email.ToString() + "'");
            string code = Guid.NewGuid().ToString();
            ResetTicket RT = new ResetTicket();
            RT.Email = Email;
            RT.Expiration = DateTime.Now.AddDays(1);
            RT.TokenHash = code;
            RT.TokenUsed = false;
            db.ResetTicket.Add(RT);
        

            StringBuilder sbody = new StringBuilder();
            sbody.Append("<h1>CCL</h1>Here is your password reset link:");
            sbody.Append("<a href=http://localhost:17382/PasswordRecovery/PasswordResetbyMail?Email=" + Email);
            sbody.Append("&ticket=" + code +"&u="+ RT.TokenUsed+"&expire=" + RT.Expiration + ">Click here to change your password</a>");
            sbody.Append("<br/><br/><br/>This is for testing");

            MailMessage mail = new MailMessage();

            mail.To.Add(Email.ToString());
            mail.From = new MailAddress("hassanuzzamank@gmail.com");
            mail.Subject = "Reset Password";

            mail.Body = sbody.ToString();

            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com"; 
            smtp.Credentials = new System.Net.NetworkCredential("hassanuzzamank@gmail.com", "password"); 
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Send(mail);

            db.SaveChanges();
            return View("EmailSent");
        }
        public ActionResult EmailSent()
        {
            return View();
        }
        public ActionResult PasswordResetbyMail(string Email,   string ticket,bool ?u, string expire)
        {
            ViewBag.Email = Email;
            ViewBag.Ticket = ticket;
            ViewBag.U = u;
            ViewBag.Expire = expire;

            return View();
        }
      [HttpPost]
        public ActionResult PasswordResetbyMail(string Email, ResetPassword rp,  string Ticket,bool? U,string Expire)
        {
            if (ModelState.IsValid)
            {
                DateTime chk = DateTime.Now;
                var tokeHash = db.ResetTicket.Where(x => x.Email == Email).Select(y => y.TokenHash).FirstOrDefault();
                var IsTicketUsed = db.ResetTicket.Where(x => x.Email == Email).Select(y => y.TokenUsed).FirstOrDefault();
                var IsExpired = db.ResetTicket.Where(x => x.Email == Email).Select(y => y.Expiration).FirstOrDefault();
                if (IsExpired < chk)
                {
                    return RedirectToAction("Expired");
                }
                if (tokeHash == Ticket && IsTicketUsed == false)
                {
                    List<UserRegistration> appusers = db.User.ToList();
                    foreach (var appuser in appusers)
                    {
                        if (appuser.Email.Equals(Email))
                        {
                            ResetTicket RT = new ResetTicket();
                            RT.TokenUsed = true;

                            appuser.Password = encryptionDecryptionUtil.CreatePasswordHash(rp.NewPassword, appuser.Salt);
                            db.Entry(appuser).State = EntityState.Modified;
                            //db.Database.ExecuteSqlCommand("update ResetTickets set TokenUsed='"+true+"' where TokenHash= '" + Ticket.ToString() + "'");
                            db.Database.ExecuteSqlCommand("delete from ResetTickets where Email='" + Email.ToString() + "'");
                            db.SaveChanges();
                            return RedirectToAction("Confirmation");
                        }
                    }

                    
                }
                if (ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Something gonna wrong!");
                }



              
            }
            return View(rp);

        }

        public ActionResult Expired()
        {

            return View();
        }
	}
}