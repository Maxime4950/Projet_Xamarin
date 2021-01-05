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
    public class CaracteristiquesRepository
    {
        #region INITIALISATION DES VARIABLES
        //Async pour ne pas bloquer l'affichage de l'utilisateurs pdt un chargement
        private SQLiteAsyncConnection connection;
        public string StatusMessage { get; set; }
        #endregion

        #region CONSTRUCTEUR CaracteristiquesRepository
        public CaracteristiquesRepository(string dbPath)
        {
            connection = new SQLiteAsyncConnection(dbPath);
            //Si la table n'existe pas on la crée
            connection.CreateTableAsync<Caracteristiques>();
        }
        #endregion

        #region METHODES
        //Ajout d'une nouvelle caractéristique
        public async Task AjoutNouvelleCaracteristiqueAsync(string nom)
        {
            int result = 0; //Pour savoir le nombre d'utlisateurs ajoutés
            try
            {
                result = await connection.InsertAsync(new Caracteristiques { Nom = nom});//Retourne le nombre de ligne(s) ajouté à la table

                StatusMessage = $"{result} caractéristique ajoutée : {nom}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Impossible d'ajouter la caractéristique : {nom}.\n Erreur : {ex.Message}";
            }
        }

        //Récupération de tous les utilisateurs (pour debug)
        public async Task<List<Caracteristiques>> RecupererAllCaracteristiques()
        {
            try
            {
                return await connection.Table<Caracteristiques>().ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Impossible de récupérer la liste des caractéristiques.\n Erreur : {ex.Message}";
            }
            return new List<Caracteristiques>(); //Si erreur on retourne une liste vide
        }

        //Modification d'une caractéristique
        public async Task<bool> ModifierCaracteristiquesAsync(string nom,string nomModif)
        {
            string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<Caracteristiques>(); //Call Table
            var caracteristiques = data.Where(x => x.Nom == nom).FirstOrDefault(); //Linq Query 

            bool res = false;
            try
            {
                string sql = $"UPDATE Caracteristiques " +
                    $"SET Nom='{nomModif}'" +
                    $"WHERE Id = '{caracteristiques.Id}'";

                await connection.ExecuteAsync(sql);
                res = true;
            }
            catch (Exception)
            {

            }
            return res;
        }

        //Suppression d'une caractéristique
        public async Task<bool> SupprimerCaracteristiqueAsync(string nom)
        {
            string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<Caracteristiques>(); //Call Table
            var caracteristiques = data.Where(x => x.Nom == nom).FirstOrDefault(); //Linq Query 

            bool res = false;
            try
            {
                string sql = $"DELETE FROM Caracteristiques " +
                    $" WHERE Id={caracteristiques.Id}";

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
