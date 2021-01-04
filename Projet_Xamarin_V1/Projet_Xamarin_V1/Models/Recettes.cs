using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projet_Xamarin_V1.Models
{
    [Table("Recettes")]
    public class Recettes
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(Utilisateurs))]
        public int IdUtilisateurs { get; set; }

        [ForeignKey(typeof(Caracteristiques))]
        public int IdCaracteristiques { get; set; }

        [ForeignKey(typeof(TypeRecette))]
        public int IdTypeRecette { get; set; }

        [MaxLength(100)]
        public string Nom { get; set; }

        public int Budget { get; set; }

        [MaxLength(250)]
        public string Ingredients { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public string UrlImage { get; set; }

        [OneToMany]
        public AvisRecette AvisRecette { get; set; }

    }
}
