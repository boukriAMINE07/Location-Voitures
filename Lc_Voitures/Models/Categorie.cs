using System.ComponentModel.DataAnnotations;

namespace Lc_Voitures.Models
{
    public class Categorie
    {
        public enum TypeCategorie
        {
            DEFAULT,
            Citadine,
            Urban,
            Break,
            Berline
        }
        [Key]
        public int categorieID { get; set; }
        public TypeCategorie? type { get; set; }
    }
}