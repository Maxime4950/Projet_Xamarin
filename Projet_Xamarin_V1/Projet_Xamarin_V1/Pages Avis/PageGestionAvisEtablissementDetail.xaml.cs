using Projet_Xamarin_V1.Models;
using Projet_Xamarin_V1.Pages_Etablissement;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Projet_Xamarin_V1.Pages_Avis
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageGestionAvisEtablissementDetail : ContentPage
    {
        #region INITIALISATION DES VARIABLES
        public int id;
        public string pseudo;
        public int idEtablissement;
        #endregion

        #region CONSTRUCTEUR PageGestionAvisEtablissementDetail
        public PageGestionAvisEtablissementDetail(int idAvis, string Pseudo, int idEt)
        {
            id = idAvis;
            pseudo = Pseudo;
            idEtablissement = idEt;
            InitializeComponent();
            remplirEtablissement(idEt);
            remplirAvisEtablissement(idAvis);
        }

        #endregion

        #region METHODES

        #region Suppresion d'un avis
        private async void btnSupprimerAvis_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Question?", "Voulez vous vraiment supprimer votre avis ?", "Oui", "Non");
            if (answer == true)
            {
                bool rep = await App.AvisEtablissementRepository.SupprimerAvisEtablissementAsync(id);
                if (rep == true)
                {
                    await DisplayAlert("Suppression", "La suppression à été effectuée avec succès : redirection vers la page de liste des avis établissements.", "OK");
                    await Navigation.PushAsync(new PageGererAvisEtablissement(pseudo));
                }
                else
                {
                    await DisplayAlert("Suppression", "La suppression à connu une erreur.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Suppression", "Suppression annulée", "OK");
            }
        }
        #endregion

        #region Remplissage détail établissement
        private void remplirEtablissement(int id)
        {
            string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<Etablissements>(); //Call Table
            var etablissements = data.Where(x => x.Id == id).FirstOrDefault(); //Linq Query 
            var data2 = db.Table<Caracteristiques>();
            var caracteristique = data2.Where(x => x.Id == etablissements.IdCaracteristique).FirstOrDefault();

            eNom.Text = etablissements.Nom;
            eRue.Text = etablissements.Rue;
            eNumero.Text = etablissements.Numero.ToString();
            eVille.Text = etablissements.Ville.ToString();
            eCodePostal.Text = etablissements.CodePostal.ToString();
            ePays.Text = etablissements.Pays;
            eBudget.Text = etablissements.Budget.ToString() + "/5";
            eCaracteristique.Text = caracteristique.Nom;
            imgEtablissement.Source = etablissements.UrlImage;
        }
        #endregion

        #region Remplissage Avis Etablissement
        private void remplirAvisEtablissement(int id)
        {
            string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<AvisEtablissement>(); //Call Table
            var remplissage = data.Where(x => x.Id == id).FirstOrDefault(); //Linq Query 


            eNote.Text = remplissage.Note.ToString();
            eDestcription.Text = remplissage.Description;
        }
        #endregion

        #endregion
    }
}