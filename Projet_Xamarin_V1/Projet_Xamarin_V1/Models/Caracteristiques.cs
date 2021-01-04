using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using SQLiteNetExtensions.Attributes;

namespace Projet_Xamarin_V1.Models
{
    [Table("Caracteristiques")] //Pour définir le nom de la table dans la BD (de base elle prend le nom de la classe)
    public class Caracteristiques
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100), Unique]
        public string Nom { get; set; }

        [ManyToOne]
        public Etablissements Etablissements { get; set; }

        [ManyToOne]
        public Recettes Recettes { get; set; }
    }
}
