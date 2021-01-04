using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projet_Xamarin_V1.Models
{
    [Table("AvisRecette")]
    public class AvisRecette
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(Recettes))]
        public int IdRecette { get; set; }

        [ForeignKey(typeof(Utilisateurs))]
        public int IdUtilisateur { get; set; }

        [MaxLength(300)]
        public string Description { get; set; }

        public int Note { get; set; }
    }
}
