using System.ComponentModel.DataAnnotations;

namespace Lc_Voitures.Models
{
    public class Modele
    {
        [Key]
        public int modeleID { get; set; }
        [StringLength(50)]
        public string nom { get; set; }

        [StringLength(50)]
        public string serie { get; set; }
    }
}