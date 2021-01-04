using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projet_Xamarin_V1.Models
{
    [Table("TypeRecette")]
    public class TypeRecette
    {
        [PrimaryKey ,AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100), Unique]
        public string Nom { get; set; }

        [ManyToOne]
        public Recettes Recettes { get; set; }
    }
}
