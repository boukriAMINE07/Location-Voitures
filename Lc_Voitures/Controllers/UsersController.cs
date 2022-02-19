using Lc_Voitures.Models;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Lc_Voitures.Controllers
{
    public class UsersController : Controller
    {
        private LocationDB db = new LocationDB();

        // GET: Users
        [Authorize]
        public ActionResult Index()
        {
            string emailId = System.Web.HttpContext.Current.User.Identity.Name;
            if (emailId != "")
            {
                bool isAdmin = db.Users.FirstOrDefault(t => t.email == emailId).IsAdmin;
                if (isAdmin)
                {
                    var users = db.Users.ToList();
                    return View(users);

                }
                

            }
            return Redirect("/Voitures/ListeVoitures");

            
        }
        public ActionResult ImageCIN(int id)
        {
            byte[] cover = GetImageCINFromDataBase(id);
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }
        public byte[] GetImageCINFromDataBase(int Id)
        {
            var q = from temp in db.Users where temp.userID == Id select temp.image_CIN;
            byte[] cover = q.First();
            return cover;
        }

        public ActionResult ImagePermis(int id)
        {
            byte[] cover = GetImagePermisFromDataBase(id);
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }
        public byte[] GetImagePermisFromDataBase(int Id)
        {
            var q = from temp in db.Users where temp.userID == Id select temp.image_Permis;
            byte[] cover = q.First();
            return cover;
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        
        public ActionResult Create()
        {

            string emailId = System.Web.HttpContext.Current.User.Identity.Name;
            if (emailId != "")
            {
                var user_ID = db.Users.SingleOrDefault(u => u.email.ToLower() == emailId.ToLower());
                User user = db.Users.Find(user_ID.userID);

                if (user.IsAdmin)
                {
                    return RedirectToAction("CreateAdmin");
                }
                return Redirect("/Voitures/ListeVoitures");
            }
            return View();
        }
        // POST: Users/Create
        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (UserExist(user))
            {
                return View(user);
            }
            string emailId = System.Web.HttpContext.Current.User.Identity.Name;
            HttpPostedFileBase file2 = Request.Files["ImageDataPermis"];
            HttpPostedFileBase file1 = Request.Files["ImageDataCIN"];
            ContentRepository service = new ContentRepository();
            service.UploadImageInDataBase(file1, file2, user);
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                if (emailId == "")
                {
                    FormsAuthentication.SetAuthCookie(user.email, false);
                }
                User authUser = db.Users.Find(user.userID);
                SaveSession(authUser);
                return Redirect("/Users/Index");
            }

            return View(user);
        }
        [Authorize]
        
        public ActionResult CreateAdmin()
        {
            string userId = System.Web.HttpContext.Current.User.Identity.Name;
            User user = db.Users.FirstOrDefault(t => t.email == userId);
            if (user.IsAdmin)
            {
                return View();
            }
            return Redirect("/Voitures/ListeVoitures");



        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAdmin([Bind(Include = "userID,nom_Complet,date_Naissance,tele,email,password,IsAdmin")] User user)
        {

            if (UserExist(user))
            {
                return View(user);
            }
            string emailId = System.Web.HttpContext.Current.User.Identity.Name;
            
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                if (emailId == "")
                {
                    FormsAuthentication.SetAuthCookie(user.email, false);
                }
                User authUser = db.Users.Find(user.userID);
                SaveSession(authUser);
                return RedirectToAction("Index");
            }
            return View(user);

        }




        // GET: Users/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            string emailId = System.Web.HttpContext.Current.User.Identity.Name;
            if (emailId != "")
            {
                bool isAdmin = db.Users.FirstOrDefault(t => t.email == emailId).IsAdmin;
                if (isAdmin)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    User user = db.Users.Find(id);
                    if (user == null)
                    {
                        return HttpNotFound();
                    }
                    return View(user);

                }

            }
            return Redirect("/Voitures/ListeVoitures");

            
        }

        // POST: Users/Edit/5
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userID,nom_Complet,date_Naissance,tele,email,password,image_CIN,image_Permis")] User user)
        {
            HttpPostedFileBase file2 = Request.Files["ImageDataPermis"];
            HttpPostedFileBase file1 = Request.Files["ImageDataCIN"];
            ContentRepository service = new ContentRepository();
            service.UploadImageInDataBase(file1, file2, user);
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            string emailId = System.Web.HttpContext.Current.User.Identity.Name;
            if (emailId != "")
            {
                bool isAdmin = db.Users.FirstOrDefault(t => t.email == emailId).IsAdmin;
                if (isAdmin)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    User user = db.Users.Find(id);
                    if (user == null)
                    {
                        return HttpNotFound();
                    }
                    return View(user);

                }

            }
            return Redirect("/Voitures/ListeVoitures");


           
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult login(string email, string password, string returnUrl)
        {
            User user = db.Users.FirstOrDefault(t => t.email == email && t.password == password);
            if (user != null)
            {
                SaveAuthSession(email, user);
                if (returnUrl == "" || returnUrl == null)
                {
                    return Redirect("/Voitures/ListeVoitures");
                }
                return Redirect(returnUrl);

            }
            return View();
        }

        private void SaveAuthSession(string email, User user)
        {
            FormsAuthentication.SetAuthCookie(user.email, false);
            Session["authname"] = email;
            Session["userId"] = user.userID;
            Session["userName"] = user.nom_Complet;

            if (user.IsAdmin)
            {
                Session["authrole"] = "Admin";
            }

            else
            {
                Session["authrole"] = "Client";
            }
        }

        private void SaveSession(User authUser)
        {
            Session["userId"] = authUser.userID;
            Session["authname"] = authUser.email;
            Session["userName"] = authUser.nom_Complet;
            if (authUser.IsAdmin)
            {
                Session["authrole"] = "Admin";
            }

            else
            {
                Session["authrole"] = "Client";
            }
        }
        private bool UserExist(User user)
        {
            User existUser = db.Users.FirstOrDefault(t => t.email == user.email);
            if (existUser != null)
            {
                ViewBag.exist = "exist";
                return true;
            }
            return false;
        }
        public ActionResult logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return Redirect("/Voitures/ListeVoitures");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
