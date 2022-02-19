using Lc_Voitures.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using static Lc_Voitures.Models.Voiture;
using static Lc_Voitures.Models.Categorie;

namespace Lc_Voitures.Controllers
{
    
    public class VoituresController : Controller
    {
        private LocationDB db = new LocationDB();
        private static DateTime start;
        private static DateTime end;
        private static Voiture VoitureFilter = new Voiture();

        // GET: Voitures
        [Authorize]
        public ActionResult Index()
        {
            string emailId = System.Web.HttpContext.Current.User.Identity.Name;
            if (emailId != "")
            {
                bool isAdmin = db.Users.FirstOrDefault(t => t.email == emailId).IsAdmin;
                if (isAdmin)
                {
                    var voitures = db.Voitures.Include(v => v.Categorie).Include(v => v.Modele);
                    return View(voitures.ToList());
                   
                }
                
            }
            return Redirect("/Voitures/ListeVoitures");

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
            Voiture selectedCar = db.Voitures.Find(id);
            ViewBag.carNumber = selectedCar.matricule;
            Voiture carType;
            Location carinfo;
            MakeCarInfo(id, out carType, out carinfo);
            if (selectedCar == null)
            {
                return HttpNotFound();
            }
            return View(selectedCar);
        }
        private void MakeCarInfo(int? id,out Voiture selectedCar, out Location carinfo)
        {
            selectedCar = db.Voitures.Find(id);
            carinfo = new Location
            {
                Voiture= selectedCar,
                StartDate = start,
                EndDate = end,
            };
        }

        // GET: Voitures/Create
        [Authorize]
        public ActionResult Create()
        {
            string emailId = System.Web.HttpContext.Current.User.Identity.Name;
            if (emailId != "")
            {
                bool isAdmin = db.Users.FirstOrDefault(t => t.email == emailId).IsAdmin;
                if (isAdmin)
                {
                    
                    ViewBag.categorieID = new SelectList(db.Categories, "categorieID", "type");
                    ViewBag.modeleID = new SelectList(db.Modeles, "modeleID", "nom").Distinct();
                    ViewBag.modeleSerie = new SelectList(db.Modeles, "modeleID", "serie");
                    return View();

                }

            }
            return RedirectToAction("ListeVoitures");

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
        private void SaveViewBages(int skip, int take, int carsNum)
        {
            ViewBag.carsNum = carsNum;
            ViewBag.current = skip;
            if (carsNum % take == 0)
            {
                ViewBag.pages = (carsNum / take);
            }
            else
            {
                ViewBag.pages = (carsNum / take) + 1;
            }
        }
        // GET: Voitures/Edit/5
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

            }
            return RedirectToAction("ListeVoitures");

           
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
                    Voiture voiture = db.Voitures.Find(id);
                    if (voiture == null)
                    {
                        return HttpNotFound();
                    }
                    return View(voiture);

                }

            }
            return RedirectToAction("ListeVoitures");

            
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

        public ActionResult ListeVoitures(int skip = 1, int take = 6)
        {
            string emailId = System.Web.HttpContext.Current.User.Identity.Name;
            if (emailId != "")
            {
                bool isAdmin = db.Users.FirstOrDefault(t => t.email == emailId).IsAdmin;
                if (isAdmin)
                {
                    return Redirect("/Voitures/index");
                }
            }
            List<Voiture> carTypes;
            int carsNum;
            MakeList(skip, take, out carTypes, out carsNum);
            CreateViewBags(skip, take, carsNum);
            return View(carTypes);

        }
        private void CreateViewBags(int skip, int take, int carsNum)
        {

            ViewBag.carsNum = carsNum;

            ViewBag.nom = db.Modeles.Select(t => t.nom ).Distinct();
            ViewBag.serie = db.Modeles.Select(t => t.serie).Distinct();
            ViewBag.type = db.Categories.Select(t => t.type).Distinct();
            
            
            ViewBag.current = skip;
            if (carsNum % take == 0)
            {
                ViewBag.pages = (carsNum / take);
            }
            else
            {
                ViewBag.pages = (carsNum / take) + 1;
            }
            ViewBag.start = start.ToString("dd/MM/yyyy");
            ViewBag.end = end.ToString("dd/MM/yyyy");
        }

        private void MakeList(int skip, int take, out List<Voiture> carTypes, out int carsNum)
        {
            carTypes = new List<Voiture>();
            carTypes = db.Voitures.OrderBy(t => t.voitureID).ToList();
            carTypes = FilterCarsbyModeleAndCategorie(carTypes);
            carsNum = carTypes.Count;
            carTypes = carTypes.
                Skip((skip - 1) * take).
                Take(take).
                ToList();
        }

        private static List<Voiture> FilterCarsbyModeleAndCategorie(List<Voiture> carTypes)
        {
            if ( VoitureFilter.modeleID  != 0)
            {
                carTypes = carTypes.
                    Where(w => w.modeleID == VoitureFilter.modeleID ).
                    ToList();
            }
            //if (VoitureFilter.modeleID != 0)
            //{
            //    carTypes = carTypes.
            //        Where( w=>w.Modele.serie == VoitureFilter.Modele.serie).
            //        ToList();
            //}
            if (VoitureFilter.categorieID != 0 )
            {
                carTypes = carTypes.
                    Where(w => w.categorieID  == VoitureFilter.categorieID).
                    ToList();
            }
           

            return carTypes;
        }

        [Authorize]
        public ActionResult Reservation(int? id)
        {
            string emailId = System.Web.HttpContext.Current.User.Identity.Name;
            Voiture voiture = db.Voitures.Find(id);
            var user = db.Users.SingleOrDefault(u => u.email.ToLower() == emailId);

            Location location = new Location()
            {
                Voiture = voiture,
                voitureID = voiture.voitureID,
                StartDate = start,
                EndDate = end,
                userID = user.userID

            };
            
            Location check = CheckLocation(location);
            if (check != null)
            {
                return View();
            }
            db.Locations.Add(location);
            db.SaveChanges();
            return View();
        }
        private Location CheckLocation(Location location)
        {
            return db.Locations.FirstOrDefault(
                            t => t.voitureID == location.voitureID
                            && t.userID==location.userID
                            && t.StartDate == location.StartDate
                            && t.EndDate == location.EndDate);
        }



        public void InitStartDate(DateTime startDate)
        {
            start = startDate;
        }

        public void InitEndDate(DateTime endDate)
        {
            end = endDate;
        }
        public void InitFilter(TypeCarburant filtercarburant, TypeCategorie type, string nom, string serie)
        {
            

            var categorie = db.Categories.SingleOrDefault(u => u.type == type);
            var modele = db.Modeles.SingleOrDefault(u => u.nom == nom);
            VoitureFilter.modeleID = modele.modeleID;
            VoitureFilter.carburant = filtercarburant;
            VoitureFilter.categorieID = categorie.categorieID;

            //VoitureFilter.Modele.serie = modele.serie;
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
