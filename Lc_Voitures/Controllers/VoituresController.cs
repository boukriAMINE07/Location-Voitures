using Lc_Voitures.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Lc_Voitures.Controllers
{
    public class VoituresController : Controller
    {
        private LocationDB db = new LocationDB();

        // GET: Voitures
        public ActionResult Index()
        {
            var voitures = db.Voitures.Include(v => v.Categorie).Include(v => v.Modele);
            return View(voitures.ToList());
        }

        public ActionResult ImageVoiture(int id)
        {
            byte[] cover = GetImageVoitureFromDataBase(id);
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }
        public byte[] GetImageVoitureFromDataBase(int Id)
        {
            var q = from temp in db.Voitures where temp.voitureID == Id select temp.image;
            byte[] cover = q.First();
            return cover;
        }

        // GET: Voitures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Voiture voiture = db.Voitures.Find(id);
            if (voiture == null)
            {
                return HttpNotFound();
            }
            return View(voiture);
        }

        // GET: Voitures/Create
        public ActionResult Create()
        {
            ViewBag.categorieID = new SelectList(db.Categories, "categorieID", "type");
            ViewBag.modeleID = new SelectList(db.Modeles, "modeleID", "nom").Distinct();
            ViewBag.modeleSerie = new SelectList(db.Modeles, "modeleID", "serie");
            return View();
        }

        // POST: Voitures/Create
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "voitureID,matricule,date_Mise_En_Circulation,prix_Par_Jour,carburant,image,modeleID,categorieID")] Voiture voiture)
        {
            HttpPostedFileBase file1 = Request.Files["ImageDataVoiture"];
            ContentRepository service = new ContentRepository();
            service.UploadImageInDataBase(file1, voiture);

            if (ModelState.IsValid)
            {
                db.Voitures.Add(voiture);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.categorieID = new SelectList(db.Categories, "categorieID", "type", voiture.categorieID);
            ViewBag.modeleID = new SelectList(db.Modeles, "modeleID", "nom", voiture.modeleID).Distinct();
            ViewBag.modeleSerie = new SelectList(db.Modeles, "modeleID", "serie", voiture.modeleID);
            return View(voiture);
        }

        // GET: Voitures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Voiture voiture = db.Voitures.Find(id);
            if (voiture == null)
            {
                return HttpNotFound();
            }
            ViewBag.categorieID = new SelectList(db.Categories, "categorieID", "type", voiture.categorieID);
            ViewBag.modeleID = new SelectList(db.Modeles, "modeleID", "nom", voiture.modeleID);
            ViewBag.modeleSerie = new SelectList(db.Modeles, "modeleID", "serie", voiture.modeleID);
            return View(voiture);
        }

        // POST: Voitures/Edit/5
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "voitureID,matricule,date_Mise_En_Circulation,prix_Par_Jour,carburant,image,modeleID,categorieID")] Voiture voiture)
        {
            HttpPostedFileBase file1 = Request.Files["ImageDataVoiture"];
            ContentRepository service = new ContentRepository();
            service.UploadImageInDataBase(file1, voiture);
            if (ModelState.IsValid)
            {
                db.Entry(voiture).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.categorieID = new SelectList(db.Categories, "categorieID", "type", voiture.categorieID);
            ViewBag.modeleID = new SelectList(db.Modeles, "modeleID", "nom", voiture.modeleID);
            ViewBag.modeleSerie = new SelectList(db.Modeles, "modeleID", "serie", voiture.modeleID);
            return View(voiture);
        }

        // GET: Voitures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Voiture voiture = db.Voitures.Find(id);
            if (voiture == null)
            {
                return HttpNotFound();
            }
            return View(voiture);
        }

        // POST: Voitures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Voiture voiture = db.Voitures.Find(id);
            db.Voitures.Remove(voiture);
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
