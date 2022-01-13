using System;
using System.ComponentModel.DataAnnotations;

namespace Lc_Voitures.Models
{
    public class User
    {
        [Key]
        public int userID { get; set; }
        [Required]
        [StringLength(60)]
        public string nom_Complet { get; set; }
        [Required]
        public DateTime date_Naissance { get; set; }
        [Required]

        public int tele { get; set; }
        [Required]
        [EmailAddress]


        public string email { get; set; }
        [Required]
        public string password { get; set; }

        [DataType("image")]
        [MaxLength]

        public byte[] image_CIN { get; set; }
        [DataType("image")]
        [MaxLength]
        public byte[] image_Permis { get; set; }
        public bool IsAdmin { get; set; }

    }
}