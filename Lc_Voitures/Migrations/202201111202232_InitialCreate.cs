namespace Lc_Voitures.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        categorieID = c.Int(nullable: false, identity: true),
                        type = c.Int(),
                    })
                .PrimaryKey(t => t.categorieID);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        locationID = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        voitureID = c.Int(nullable: false),
                        userID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.locationID)
                .ForeignKey("dbo.Users", t => t.userID, cascadeDelete: true)
                .ForeignKey("dbo.Voitures", t => t.voitureID, cascadeDelete: true)
                .Index(t => t.voitureID)
                .Index(t => t.userID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        userID = c.Int(nullable: false, identity: true),
                        nom_Complet = c.String(nullable: false, maxLength: 60),
                        date_Naissance = c.DateTime(nullable: false),
                        tele = c.Int(nullable: false),
                        email = c.String(nullable: false),
                        password = c.String(nullable: false),
                        image_CIN = c.Binary(),
                        image_Permis = c.Binary(),
                        IsAdmin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.userID);
            
            CreateTable(
                "dbo.Voitures",
                c => new
                    {
                        voitureID = c.Int(nullable: false, identity: true),
                        matricule = c.String(nullable: false),
                        date_Mise_En_Circulation = c.DateTime(nullable: false),
                        prix_Par_Jour = c.Int(nullable: false),
                        carburant = c.Int(),
                        image = c.Binary(),
                        modeleID = c.Int(nullable: false),
                        categorieID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.voitureID)
                .ForeignKey("dbo.Categories", t => t.categorieID, cascadeDelete: true)
                .ForeignKey("dbo.Modeles", t => t.modeleID, cascadeDelete: true)
                .Index(t => t.modeleID)
                .Index(t => t.categorieID);
            
            CreateTable(
                "dbo.Modeles",
                c => new
                    {
                        modeleID = c.Int(nullable: false, identity: true),
                        nom = c.String(maxLength: 50),
                        serie = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.modeleID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Locations", "voitureID", "dbo.Voitures");
            DropForeignKey("dbo.Voitures", "modeleID", "dbo.Modeles");
            DropForeignKey("dbo.Voitures", "categorieID", "dbo.Categories");
            DropForeignKey("dbo.Locations", "userID", "dbo.Users");
            DropIndex("dbo.Voitures", new[] { "categorieID" });
            DropIndex("dbo.Voitures", new[] { "modeleID" });
            DropIndex("dbo.Locations", new[] { "userID" });
            DropIndex("dbo.Locations", new[] { "voitureID" });
            DropTable("dbo.Modeles");
            DropTable("dbo.Voitures");
            DropTable("dbo.Users");
            DropTable("dbo.Locations");
            DropTable("dbo.Categories");
        }
    }
}
