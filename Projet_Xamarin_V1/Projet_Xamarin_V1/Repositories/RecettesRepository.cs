using Projet_Xamarin_V1.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Projet_Xamarin_V1.Repositories
{
    public class RecettesRepository
    {
        //Async pour ne pas bloquer l'affichage de la recette pdt un chargement
        #region INITIALISATON DES VARIABLES
        private SQLiteAsyncConnection connection;
        public string StatusMessage { get; set; }
        #endregion

        #region CONSTRUCTEUR RecettesRepository
        public RecettesRepository(string dbPath)
        {
            connection = new SQLiteAsyncConnection(dbPath);
            //Si la table n'existe pas on la crée
            connection.CreateTableAsync<Recettes>();
        }
        #endregion

        #region METHODES
        //Ajout d'une recette
        public async Task AjoutNouvelleRecetteAsync(string nom, string ingredients, string description, int budget, string urlimage, int idutilisateur, int idtyperecette, int idcaracteristique)
        {
            int result = 0; //Pour savoir le nombre de recettes ajoutées
            try
            {
                result = await connection.InsertAsync(new Recettes { Nom = nom, Ingredients = ingredients, Description = description, Budget = budget, UrlImage = urlimage, IdUtilisateurs = idutilisateur, IdTypeRecette = idtyperecette, IdCaracteristiques = idcaracteristique });//Retourne le nombre de ligne(s) ajouté à la table

                StatusMessage = $"{result} recette ajouté : {nom}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Impossible d'ajouter la recette : {nom}.\n Erreur : {ex.Message}";
            }
        }

        //Récupération de toutes les recettes (pour debug)
        public async Task<List<Recettes>> RecupererAllRecettes()
        {
            try
            {
                return await connection.Table<Recettes>().ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Impossible de récupérer la liste des recettes.\n Erreur : {ex.Message}";
            }
            return new List<Recettes>(); //Si erreur on retourne une liste vide
        }

        //Récupération des recettes créés par l'user (pseudo)
        public async Task<List<Recettes>> RecupererAllRecettesUser(string pseudo)
        {
            try
            {
                string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
                var db = new SQLiteConnection(dpPath);
                var data = db.Table<Utilisateurs>(); //Call Table
                var user = data.Where(x => x.Pseudo == pseudo).FirstOrDefault(); //Linq Query 
                return await connection.Table<Recettes>().Where(x => x.IdUtilisateurs == user.Id).ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Impossible de récupérer la liste des recettes.\n Erreur : {ex.Message}";
            }
            return new List<Recettes>(); //Si erreur on retourne une liste vide
        }

        //Récupération des recettes créés par l'user (id)
        public async Task<List<Recettes>> RecupererAllRecettesUserID(int id)
        {
            try
            {
                string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
                return await connection.Table<Recettes>().Where(x => x.IdUtilisateurs == id).ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Impossible de récupérer la liste des recettes de l'user.\n Erreur : {ex.Message}";
            }
            return new List<Recettes>(); //Si erreur on retourne une liste vide
        }

        //Récupération des caractéristiques
        public async Task<List<Recettes>> RecupererAllRecettesCaract(string caract)
        {
            try
            {
                string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
                var db = new SQLiteConnection(dpPath);

                //Obtention de l'id du nom de la caractéristique
                var data = db.Table<Caracteristiques>(); //Call Table
                var caracteristiques = data.Where(x => x.Nom == caract).FirstOrDefault(); //Linq Query 

                return await connection.Table<Recettes>().Where(x => x.IdCaracteristiques == caracteristiques.Id).ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Impossible de récupérer la liste des recettes.\n Erreur : {ex.Message}";
            }
            return new List<Recettes>(); //Si erreur on retourne une liste vide
        }

        //Supprimer une recette avec les caract
        public async Task<bool> SupprimerRecetteAsync(string nom, int budget, string ingredients, string description)
        {
            string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<Recettes>(); //Call Table
            var recettes = data.Where(x => x.Nom == nom && x.Budget==budget && x.Ingredients == ingredients && x.Description == description ).FirstOrDefault(); //Linq Query 

            bool res = false;
            try
            {
                string sql = $"DELETE FROM Recettes " +
                    $" WHERE Id={recettes.Id}";

                await connection.ExecuteAsync(sql);
                res = true;
            }
            catch (Exception)
            {

            }
            return res;
        }

        //Supprimer une recette avec l'id
        public async Task<bool> SupprimerRecettesUserAsync(int id)
        {

            bool res = false;
            try
            {
                string sql = $"DELETE FROM Recettes " +
                    $" WHERE Id={id}";

                await connection.ExecuteAsync(sql);
                res = true;
            }
            catch (Exception)
            {

            }
            return res;
        }
        #endregion
    }
}
