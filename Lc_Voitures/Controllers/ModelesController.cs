using Lc_Voitures.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Lc_Voitures.Controllers
{
    [Authorize]
    public class ModelesController : Controller
    {
        private LocationDB db = new LocationDB();

        // GET: Modeles
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.Modeles.ToList());
        }

        // GET: Modeles/Details/5
        [AllowAnonymous]

        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Modele modele = db.Modeles.Find(id);
            if (modele == null)
            {
                return HttpNotFound();
            }
            return View(modele);
        }

        // GET: Modeles/Create
        public ActionResult Create()
        {
            string emailId = System.Web.HttpContext.Current.User.Identity.Name;
            if (emailId != "")
            {
                bool isAdmin = db.Users.FirstOrDefault(t => t.email == emailId).IsAdmin;
                if (isAdmin)
                {
                    return View();

                }
                
            }
            return RedirectToAction("Index");
        }

        // POST: Modeles/Create
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "modeleID,nom,serie")] Modele modele)
        {
            if (ModelState.IsValid)
            {
                db.Modeles.Add(modele);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(modele);
        }

        // GET: Modeles/Edit/5
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
                    Modele modele = db.Modeles.Find(id);
                    if (modele == null)
                    {
                        return HttpNotFound();
                    }
                    return View(modele);

                }

            }
            return RedirectToAction("Index");
           
        }

        // POST: Modeles/Edit/5
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "modeleID,nom,serie")] Modele modele)
        {
            if (ModelState.IsValid)
            {
                db.Entry(modele).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(modele);
        }

        // GET: Modeles/Delete/5
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
                    Modele modele = db.Modeles.Find(id);
                    if (modele == null)
                    {
                        return HttpNotFound();
                    }
                    return View(modele);

                }

            }
            return RedirectToAction("Index");

            

            
        }

        // POST: Modeles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Modele modele = db.Modeles.Find(id);
            db.Modeles.Remove(modele);
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
    }
}
