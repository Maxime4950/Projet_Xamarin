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
    public class AvisRecetteRepository
    {
        #region INITIALISATION DES VARIABLES
        //Async pour ne pas bloquer l'affichage de l'avis d'une recette pdt un chargement
        private SQLiteAsyncConnection connection;
        public string StatusMessage { get; set; }
        #endregion

        #region CONSTRUCTEUR  AvisRecetteRepository
        public AvisRecetteRepository(string dbPath)
        {
            connection = new SQLiteAsyncConnection(dbPath);
            //Si la table n'existe pas on la crée
            connection.CreateTableAsync<AvisRecette>();
        }
        #endregion

        #region METHODES
        //Ajout d'avis recette
        public async Task AjoutNouvelAvisRecetteAsync(string description, int note, int idutilisateur, int idrecette)
        {
            int result = 0; //Pour savoir le nombre d'avis de recettes ajoutés
            try
            {
                result = await connection.InsertAsync(new AvisRecette { Description = description, Note = note, IdUtilisateur = idutilisateur, IdRecette = idrecette});//Retourne le nombre de ligne(s) ajouté à la table

                StatusMessage = $"{result} avis de recette ajouté : {idrecette}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Impossible d'ajouter l'avis de recette : {idrecette}.\n Erreur : {ex.Message}";
            }
        }

        //Récupération de tous les avis de recette (pour debug)
        public async Task<List<AvisRecette>> RecupererAllAvisRecette()
        {
            try
            {
                return await connection.Table<AvisRecette>().ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Impossible de récupérer la liste des avis de recette.\n Erreur : {ex.Message}";
            }
            return new List<AvisRecette>(); //Si erreur on retourne une liste vide
        }

        //Récupération de tous les avis d'un utilisateur
        public async Task<List<AvisRecette>> RecupererAllAvisRecetteUser(int id)
        {
            try
            {
                string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
                var db = new SQLiteConnection(dpPath);
                var data = db.Table<Utilisateurs>(); //Call Table
                var user = data.Where(x => x.Id == id).FirstOrDefault(); //Linq Query 
                return await connection.Table<AvisRecette>().Where(x => x.IdUtilisateur == user.Id).ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Impossible de récupérer la liste des avis recettes.\n Erreur : {ex.Message}";
            }
            return new List<AvisRecette>(); //Si erreur on retourne une liste vide
        }

        //Suppression d'un avis recette
        public async Task<bool> SupprimerAvisRecettesUserAsync(int idRecette)
        {
            bool res = false;
            try
            {
                string sql = $"DELETE FROM AvisRecette " +
                    $" WHERE Id={idRecette}";

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
