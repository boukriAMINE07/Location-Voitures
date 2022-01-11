using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Lc_Voitures.Models
{
    public class ContentRepository
    {
        private readonly LocationDB db = new LocationDB();
        public void UploadImageInDataBase(HttpPostedFileBase file1, HttpPostedFileBase file2, User user)
        {
            user.image_CIN = ConvertToBytes(file1);
            user.image_Permis = ConvertToBytes(file2);
            var Client = new User
            {

                image_CIN = user.image_CIN,
                image_Permis = user.image_Permis
            };
            db.Users.Add(Client);
            db.SaveChanges();


        }
        public void UploadImageInDataBase(HttpPostedFileBase file, Voiture voiture)
        {
            voiture.image = ConvertToBytes(file);
            var Content = new Voiture
            {
               
                image = voiture.image
            };
            db.Voitures.Add(Content);
             db.SaveChanges();
            
        }
        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
           
                BinaryReader reader = new BinaryReader(image.InputStream);
                imageBytes = reader.ReadBytes((int)image.ContentLength);
                return imageBytes;

        }
    }
}