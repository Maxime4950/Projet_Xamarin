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
    //Classe qui gère les données
    public class UtilisateursRepository
    {
        #region INITIALISATION DES VARIABLES
        //Async pour ne pas bloquer l'affichage de l'utilisateurs pdt un chargement
        private SQLiteAsyncConnection connection;
        public string StatusMessage { get; set; }
        #endregion

        #region CONSTRUCTEUR UtilisateursRepository
        public UtilisateursRepository(string dbPath)
        {
            connection = new SQLiteAsyncConnection(dbPath);
            //Si la table n'existe pas on la crée
            connection.CreateTableAsync<Utilisateurs>();
        }
        #endregion

        #region METHODES
        //Ajout d'un nouvel utilisateur
        public async Task AjoutNouvelUtilisateurAsync(string pseudo, string motDePasse, string mail, DateTime dateInscription, DateTime dateNaissance )
        {
            int result = 0; //Pour savoir le nombre d'utlisateurs ajoutés
            try
            {
                result = await connection.InsertAsync(new Utilisateurs { Pseudo = pseudo, MotDePasse = motDePasse, Mail = mail, DateInscription = dateInscription, DateNaissance = dateNaissance });//Retourne le nombre de ligne(s) ajouté à la table

                StatusMessage = $"{result} utilisateurs ajouté : {pseudo}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Impossible d'ajouter l'utilisateur : {pseudo}.\n Erreur : {ex.Message}";
            }
        }

        //Récupération de tous les utilisateurs (pour debug)
        public async Task<List<Utilisateurs>> RecupererAllUtilisateurs()
        {
            try
            {
                return await connection.Table<Utilisateurs>().ToListAsync(); 
            }
            catch (Exception ex)
            {
                StatusMessage = $"Impossible de récupérer la liste des utilisateurs.\n Erreur : {ex.Message}";
            }

            return new List<Utilisateurs>(); //Si erreur on retourne une liste vide
        }

        //Modification d'un utilisateur
        public async Task<bool> ModifierUtilisateurAsync(string pseudo,string mdp, string email, int id)
        {
            string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<Utilisateurs>(); //Call Table
            var utilisateur = data.Where(x => x.Id == id).FirstOrDefault(); //Linq Query

            bool res = false;
            try
            {
                string sql = $"UPDATE Utilisateurs " +
                    $"SET Pseudo='{pseudo}'," +
                    $"MotDePasse='{mdp}'," +
                    $"Mail='{email}'" +
                    $",DateNaissance='{utilisateur.DateNaissance}'" +
                    $" WHERE Id={utilisateur.Id}";

                await connection.ExecuteAsync(sql);
                res = true;
            }
            catch(Exception)
            {   

            }
            return res;
        }

        //Suppression d'un utilisateur
        public async Task<bool> SupprimerUtilisateurAsync(string pseudo)
        {
            string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<Utilisateurs>(); //Call Table
            var utilisateur = data.Where(x => x.Pseudo == pseudo).FirstOrDefault(); //Linq Query 

            bool res = false;
            try
            {
                string sql = $"DELETE FROM Utilisateurs " +
                    $" WHERE Id={utilisateur.Id}";

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
