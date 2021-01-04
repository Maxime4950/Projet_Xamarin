using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projet_Xamarin_V1.Models
{
    [Table("Utilisateurs")] //Pour définir le nom de la table dans la BD (de base elle prend le nom de la classe)
    public class Utilisateurs
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100),Unique]
        public string Pseudo { get; set; }

        [MaxLength(50)]
        public string MotDePasse { get; set; }

        [MaxLength(100)]
        public string Mail { get; set; }

        public DateTime DateInscription { get; set; }

        public DateTime DateNaissance { get; set; }

        [OneToMany]
        public List<Recettes> Recettes { get; set; }

        [OneToMany]
        public List<Etablissements> Etablissements { get; set; }

        [OneToMany]
        public AvisEtablissement AvisEtablissement { get; set; }

        [OneToMany]
        public AvisRecette AvisRecette { get; set; }
    }
}
