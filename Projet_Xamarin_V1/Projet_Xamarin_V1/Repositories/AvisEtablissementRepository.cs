using Projet_Xamarin_V1.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Xamarin_V1.Repositories
{
    public class AvisEtablissementRepository
    {
        //Async pour ne pas bloquer l'affichage de l'avis de l'établissement pdt un chargement
        private SQLiteAsyncConnection connection;

        public string StatusMessage { get; set; }
        public AvisEtablissementRepository(string dbPath)
        {
            connection = new SQLiteAsyncConnection(dbPath);
            //Si la table n'existe pas on la crée
            connection.CreateTableAsync<AvisEtablissement>();
        }

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

        //Récupération de tous les avis d'établissement (pour debug)
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

        public async Task<bool> SupprimerAvisEtablissementsUserAsync(int idEtablissement)
        {
            bool res = false;
            try
            {
                string sql = $"DELETE FROM AvisEtablissement " +
                    $" WHERE Id={idEtablissement}";

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
