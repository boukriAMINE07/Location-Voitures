using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Lc_Voitures.Models
{
    public class Voiture
    {
        public enum TypeCarburant
        {
            ParDefaut,
            Essencee,
            Diesel
        }
        [Key]
        public int voitureID { get; set; }
        [Required]

        public string matricule { get; set; }
        [Required]
        [DisplayName("Mise En Circulation")]
        public DateTime date_Mise_En_Circulation { get; set; }
        [Required]
        [Range(100, 600)]
        [DisplayName("Prix/Jour")]
        public int prix_Par_Jour { get; set; }
        public TypeCarburant? carburant { get; set; }
        [DataType("image")]
        [MaxLength]
        public byte[] image { get; set; }

        public int modeleID { get; set; }
        public virtual Modele Modele { get; set; }

        public int categorieID { get; set; }

        public virtual Categorie Categorie { get; set; }
    }
}