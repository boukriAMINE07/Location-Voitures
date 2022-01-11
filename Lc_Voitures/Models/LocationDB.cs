using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Lc_Voitures.Models
{
    public class LocationDB : DbContext
    {
        public LocationDB() : base("LVDB")
        {
        }
        public DbSet<Voiture> Voitures { get; set; }
        public DbSet<Categorie> Categories { get; set; }
        public DbSet<Modele> Modeles { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<User> Users { get; set; }
    }

}