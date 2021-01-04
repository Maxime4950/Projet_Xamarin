using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Projet_Xamarin_V1.Models; //Pour accéder aux modèles
using SQLite;
using Xamarin.Essentials;

namespace Projet_Xamarin_V1.Repositories
{
    public class TypesRepository
    {
        //Async pour ne pas bloquer l'affichage de l'utilisateurs pdt un chargement
        private SQLiteAsyncConnection connection;

        public string StatusMessage { get; set; }
        public TypesRepository(string dbPath)
        {
            connection = new SQLiteAsyncConnection(dbPath);
            //Si la table n'existe pas on la crée
            connection.CreateTableAsync<TypeRecette>();
        }

        public async Task AjoutNouveauTypeAsync(string nom)
        {
            int result = 0; //Pour savoir le nombre d'utlisateurs ajoutés
            try
            {
                result = await connection.InsertAsync(new TypeRecette { Nom = nom });//Retourne le nombre de ligne(s) ajouté à la table

                StatusMessage = $"{result} type ajouté : {nom}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Impossible d'ajouter le type : {nom}.\n Erreur : {ex.Message}";
            }
        }

        //Récupération de tous les utilisateurs (pour debug)
        public async Task<List<TypeRecette>> RecupererAllTypes()
        {
            try
            {
                return await connection.Table<TypeRecette>().ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Impossible de récupérer la liste des types.\n Erreur : {ex.Message}";
            }
            return new List<TypeRecette>(); //Si erreur on retourne une liste vide
        }

        public async Task<bool> ModifieTypeAsync(string nom, string nomModif)
        {
            string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<TypeRecette>(); //Call Table
            var typeRecette = data.Where(x => x.Nom == nom).FirstOrDefault(); //Linq Query 

            bool res = false;
            try
            {
                string sql = $"UPDATE TypeRecette " +
                    $"SET Nom='{nomModif}'" +
                    $"WHERE Id = '{typeRecette.Id}'";

                await connection.ExecuteAsync(sql);
                res = true;
            }
            catch (Exception)
            {

            }
            return res;
        }

        public async Task<bool> SupprimerTypeAsync(string nom)
        {
            string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<TypeRecette>(); //Call Table
            var typeRecette = data.Where(x => x.Nom == nom).FirstOrDefault(); //Linq Query 

            bool res = false;
            try
            {
                string sql = $"DELETE FROM TypeRecette " +
                    $" WHERE Id={typeRecette.Id}";

                await connection.ExecuteAsync(sql);
                res = true;
            }
            catch (Exception)
            {

            }
            return res;
        }
    }
}
