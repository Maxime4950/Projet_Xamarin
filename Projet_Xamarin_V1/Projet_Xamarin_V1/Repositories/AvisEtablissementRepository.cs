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
    public class AvisEtablissementRepository
    {
        #region INITIALISATION DES VARIABLES
        //Async pour ne pas bloquer l'affichage de l'avis de l'établissement pdt un chargement
        private SQLiteAsyncConnection connection;
        public string StatusMessage { get; set; }
        #endregion

        #region CONSTRUCTEUR AvisEtablissementRepository
        public AvisEtablissementRepository(string dbPath)
        {
            connection = new SQLiteAsyncConnection(dbPath);
            //Si la table n'existe pas on la crée
            connection.CreateTableAsync<AvisEtablissement>();
        }
        #endregion

        #region METHODES

        //Ajout d'un avis
        public async Task AjoutNouvelAvisEtablissementAsync(string description, int note, int idutilisateur, int idetablissement)
        {
            int result = 0; //Pour savoir le nombre d'avis d'établissements ajoutés
            try
            {
                result = await connection.InsertAsync(new AvisEtablissement { Description = description, Note = note, IdUtilisateur = idutilisateur, IdEtablissement = idetablissement });//Retourne le nombre de ligne(s) ajouté à la table

                StatusMessage = $"{result} avis de l'etablissement ajouté : {idetablissement}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Impossible d'ajouter l'avis de l'étalissement : {idetablissement}.\n Erreur : {ex.Message}";
            }
        }

        //Récupération de tous les avis d'établissement (id)
        public async Task<List<AvisEtablissement>> RecupererAllAvisEtablissements(int id)
        {
            try
            {
                return await connection.Table<AvisEtablissement>().Where(x => x.IdEtablissement == id).ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Impossible de récupérer la liste des avis d'établissements.\n Erreur : {ex.Message}";
            }
            return new List<AvisEtablissement>(); //Si erreur on retourne une liste vide
        }

        //Récupération de tous les avis d'établissement (pseudo)
        public async Task<List<AvisEtablissement>> RecupererAllAvisEtablissementsUser(string pseudo)
        {
            try
            {
                string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
                var db = new SQLiteConnection(dpPath);
                var data = db.Table<Utilisateurs>(); //Call Table
                var user = data.Where(x => x.Pseudo == pseudo).FirstOrDefault(); //Linq Query 
                return await connection.Table<AvisEtablissement>().Where(x => x.IdUtilisateur == user.Id).ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Impossible de récupérer la liste des établissements de l'user.\n Erreur : {ex.Message}";
            }
            return new List<AvisEtablissement>(); //Si erreur on retourne une liste vide
        }

        //Suppression des avis de l'user
        public async Task<bool> SupprimerAvisEtablissementsUserAsync(int idUtilisateur)
        {
            bool res = false;
            try
            {
                string sql = $"DELETE FROM AvisEtablissement " +
                    $" WHERE Id={idUtilisateur}";

                await connection.ExecuteAsync(sql);
                res = true;
            }
            catch (Exception)
            {

            }
            return res;
        }

        //Suppression d'un avis unique
        public async Task<bool> SupprimerAvisEtablissementAsync(int idAvis)
        {
            bool res = false;
            try
            {
                string sql = $"DELETE FROM AvisEtablissement " +
                    $" WHERE Id={idAvis}";

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
