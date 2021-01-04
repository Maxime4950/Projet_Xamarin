using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projet_Xamarin_V1.Models
{
    [Table("Etablissements")]
    public class Etablissements
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(Utilisateurs))]
        public int IdUtilisateur { get; set; }

        [ForeignKey(typeof(Caracteristiques))]
        public int IdCaracteristique { get; set; }

        [MaxLength(100)]
        public string Nom { get; set; }

        [MaxLength(150)]
        public string Rue { get; set; }

        public int Numero { get; set; }

        [MaxLength(100)]
        public string Ville { get; set; }

        public int CodePostal { get; set; }

        [MaxLength(100)]
        public string Pays { get; set; }

        public int Budget { get; set; }

        public string UrlImage { get; set; }

        [OneToMany]
        public AvisEtablissement AvisEtablissement { get; set; }
    }
}
