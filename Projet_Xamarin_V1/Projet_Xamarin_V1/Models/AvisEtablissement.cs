using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using SQLiteNetExtensions.Attributes;

namespace Projet_Xamarin_V1.Models
{
    [Table("AvisEtablissement")] //Pour définir le nom de la table dans la BD (de base elle prend le nom de la classe)
    public class AvisEtablissement
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(Etablissements))]
        public int IdEtablissement { get; set; }

        [ForeignKey(typeof(Utilisateurs))]
        public int IdUtilisateur { get; set; }

        [MaxLength(300)]
        public string Description { get; set; }

        public int Note { get; set; }
    }
}