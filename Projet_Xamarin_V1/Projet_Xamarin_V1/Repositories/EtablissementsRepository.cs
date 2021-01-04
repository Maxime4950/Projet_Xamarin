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
    public class EtablissementsRepository
    {
        //Async pour ne pas bloquer l'affichage de l'établissement pdt un chargement
        private SQLiteAsyncConnection connection;

        public string StatusMessage { get; set; }
        public EtablissementsRepository(string dbPath)
        {
            connection = new SQLiteAsyncConnection(dbPath);
            //Si la table n'existe pas on la crée
            connection.CreateTableAsync<Etablissements>();
        }

        public async Task AjoutNouvelEtablissementAsync(string nom, string rue, int numero, string ville, int codepostal, string pays, int budget, string urlimage, int idutilisateur, int idcaracteristique)
        {
            int result = 0; //Pour savoir le nombre d'etablissements ajoutés
            try
            {
                result = await connection.InsertAsync(new Etablissements { Nom = nom, Rue = rue, Numero = numero, Budget = budget, Ville = ville, CodePostal = codepostal, Pays = pays, UrlImage = urlimage, IdUtilisateur = idutilisateur, IdCaracteristique = idcaracteristique });//Retourne le nombre de ligne(s) ajouté à la table

                StatusMessage = $"{result} etablissement ajouté : {nom}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Impossible d'ajouter l'étalissement : {nom}.\n Erreur : {ex.Message}";
            }
        }

        //Récupération de tous les établissements (pour debug)
        public async Task<List<Etablissements>> RecupererAllEtablissements()
        {
            try
            {
                return await connection.Table<Etablissements>().ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Impossible de récupérer la liste des établissements.\n Erreur : {ex.Message}";
            }
            return new List<Etablissements>(); //Si erreur on retourne une liste vide
        }

        public async Task<List<Etablissements>> RecupererAllEtablissementsUser(string pseudo)
        {
            try
            {
                string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
                var db = new SQLiteConnection(dpPath);
                var data = db.Table<Utilisateurs>(); //Call Table
                var user = data.Where(x => x.Pseudo == pseudo ).FirstOrDefault(); //Linq Query 
                return await connection.Table<Etablissements>().Where(x => x.IdUtilisateur == user.Id).ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Impossible de récupérer la liste des établissements de l'user.\n Erreur : {ex.Message}";
            }
            return new List<Etablissements>(); //Si erreur on retourne une liste vide
        }

        public async Task<List<Etablissements>> RecupererAllEtablissementsUserID(int id)
        {
            try
            {
                return await connection.Table<Etablissements>().Where(x => x.IdUtilisateur == id).ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Impossible de récupérer la liste des établissements de l'user.\n Erreur : {ex.Message}";
            }
            return new List<Etablissements>(); //Si erreur on retourne une liste vide
        }

        public async Task<List<Etablissements>> RecupererAllEtablissementsVille(string ville)
        {
            try
            {
                string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
                var db = new SQLiteConnection(dpPath);
                return await connection.Table<Etablissements>().Where(x => x.Ville == ville).ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Impossible de récupérer la liste des établissements.\n Erreur : {ex.Message}";
            }
            return new List<Etablissements>(); //Si erreur on retourne une liste vide
        }

        public async Task<bool> ModifierEtablissementAsync(string nom, string rue, int numero, string ville, int codepostal, string pays, int budget, string urlimage, int idcaracteristique, int id)
        {
            string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<Etablissements>(); //Call Table
            var etablissement = data.Where(x => x.Id == id).FirstOrDefault(); //Linq Query 

            bool res = false;
            try
            {
                string sql = $"UPDATE Etablissements " +
                    $"SET Nom='{nom}'," +
                    $"Rue='{rue}'," +
                    $"Numero='{numero}'" +
                    $",CodePostal='{codepostal}'" +
                    $",Ville='{ville}'" +
                    $",Pays='{pays}'" +
                    $",Budget='{budget}'" +
                    $",UrlImage='{urlimage}'" +
                    $",IdCaracteristique='{idcaracteristique}'" +
                    $" WHERE Id={etablissement.Id}";

                await connection.ExecuteAsync(sql);
                res = true;
            }
            catch (Exception)
            {

            }
            return res;
        }

        public async Task<bool> SupprimerEtablissementAsync(string nom, string rue, int numero, string ville, int codepostal, string pays)
        {
            string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<Etablissements>(); //Call Table
            var etablissement = data.Where(x => x.Nom == nom && x.Rue == rue && x.Numero == numero && x.CodePostal == codepostal && x.Ville == ville && x.Pays == pays).FirstOrDefault(); //Linq Query 

            bool res = false;
            try
            {
                string sql = $"DELETE FROM Etablissements " +
                    $" WHERE Id={etablissement.Id}";

                await connection.ExecuteAsync(sql);
                res = true;
            }
            catch (Exception)
            {

            }
            return res;
        }

        //Pour la suppression des relations quand on supprime le profil
        public async Task<bool> SupprimerEtablissementUserAsync(int id)
        {

            bool res = false;
            try
            {
                string sql = $"DELETE FROM Etablissements " +
                    $" WHERE Id={id}";

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
