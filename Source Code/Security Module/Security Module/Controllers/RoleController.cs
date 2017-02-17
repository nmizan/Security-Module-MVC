using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Security_Module.Models;

namespace Security_Module.Controllers
{
    public class RoleController : Controller
    {
        private SecurityDbContext db = new SecurityDbContext();

        // GET: /Roll/
        public ActionResult Index()
        {
            return View(db.Roles.ToList());
        }

        // GET: /Roll/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // GET: /Roll/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Roll/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="RoleId,RoleName")] Role role)
        {
            if (ModelState.IsValid)
            {
                db.Roles.Add(role);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(role);
        }

        // GET: /Roll/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // POST: /Roll/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="RoleId,RoleName")] Role role)
        {
            if (ModelState.IsValid)
            {
                db.Entry(role).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(role);
        }

        // GET: /Roll/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // POST: /Roll/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Role role = db.Roles.Find(id);
            db.Roles.Remove(role);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult MenuPermission(int? Id)
        {


            ViewBag.Role = db.Roles.Where(x => x.RoleId == Id).Select(s => s.RoleName).FirstOrDefault();

            return View(db.MenuList.ToList());
        }




        //save menu permission method
        [HttpPost]
        public ActionResult SaveManagePermission(MenuPermission menuPermission, bool[] IsVisible, int[] MenuId, int Id)
        {

            List<bool> list = new List<bool>(IsVisible);

            for (int i = 0; i < MenuId.Length; i++)
            {
                if (list[i] == true)
                {
                    IsVisible[i] = true;
                    list.RemoveAt(i + 1);

                }
                else
                {
                    IsVisible[i] = false;
                }
                menuPermission.IsVisible = list[i];
                menuPermission.RoleId = Id;
                menuPermission.MenuId = MenuId[i];
                db.MenuPermission.Add(menuPermission);
                db.SaveChanges();
            }

            return RedirectToAction("Index");

        }


        public ActionResult ManagePermission(int? Id, int? MenuId)
        {


            ViewBag.Role = db.Roles.Where(x => x.RoleId == Id).Select(s => s.RoleName).FirstOrDefault();
            ViewBag.RoleId = db.Roles.Where(x => x.RoleId == Id).Select(x => x.RoleId).FirstOrDefault();



            return View(db.MenuPermission.ToList());
        }
        [HttpPost]
        public ActionResult UpdateManagePermission(MenuPermission menuPermission, bool[] IsVisible, int[] MenuId, int Id)
        {


            db.Database.ExecuteSqlCommand("delete from MenuPermissions where RoleId='" + Id + "'");

            List<bool> list = new List<bool>(IsVisible);

            for (int i = 0; i < MenuId.Length; i++)
            {
                if (list[i] == true)
                {
                    IsVisible[i] = true;
                    list.RemoveAt(i + 1);

                }
                else
                {
                    IsVisible[i] = false;
                }
                menuPermission.IsVisible = list[i];
                menuPermission.RoleId = Id;
                menuPermission.MenuId = MenuId[i];
                db.MenuPermission.Add(menuPermission);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }



        public ActionResult RoleAssignToUsers(int? UserId, int? Roles)
        {

            ViewBag.Roles = new SelectList(db.Roles, "RoleId", "RoleName");
            return View(db.User.ToList());
        }
        [HttpPost]
        public ActionResult SaveRoleAssignToUsers(RoleAssignUser roleassignUser, int[] Roles, int[] UserId)
        {
            for (int i = 0; i < UserId.Length; i++)
            {
                roleassignUser.UserId = UserId[i];
                roleassignUser.RoleId = Roles[i];
                db.RoleAssignUser.Add(roleassignUser);
                db.SaveChanges();
            }


            return RedirectToAction("Index");
        }
      
    }
}
