using Lc_Voitures.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Lc_Voitures.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private LocationDB db = new LocationDB();

        // GET: Categories
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        // GET: Categories/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categorie categorie = db.Categories.Find(id);
            if (categorie == null)
            {
                return HttpNotFound();
            }
            return View(categorie);
        }

        // GET: Categories/Create
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

        // POST: Categories/Create
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "categorieID,type")] Categorie categorie)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(categorie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(categorie);
        }

        // GET: Categories/Edit/5
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
                    Categorie categorie = db.Categories.Find(id);
                    if (categorie == null)
                    {
                        return HttpNotFound();
                    }
                    return View(categorie);

                }

            }
            return RedirectToAction("Index");

            
        }

        // POST: Categories/Edit/5
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "categorieID,type")] Categorie categorie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categorie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categorie);
        }

        // GET: Categories/Delete/5
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
                    Categorie categorie = db.Categories.Find(id);
                    if (categorie == null)
                    {
                        return HttpNotFound();
                    }
                    return View(categorie);

                }

            }
            return RedirectToAction("Index");

           
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categorie categorie = db.Categories.Find(id);
            db.Categories.Remove(categorie);
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
