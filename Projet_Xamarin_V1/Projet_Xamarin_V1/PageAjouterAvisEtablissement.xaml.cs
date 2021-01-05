using Projet_Xamarin_V1.Models;
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

namespace Projet_Xamarin_V1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageAjouterAvisEtablissement : ContentPage
    {
        #region INITIALISATION DES VARIABLES
        public int ID = 0;
        string pseudo;
        #endregion

        #region CONSTRUCTEUR PageAjouterAvisEtablissement
        public PageAjouterAvisEtablissement(string pseudo, int id)
        {
            this.pseudo = pseudo;
            ID = id;
            InitializeComponent();
        }
        #endregion

        #region METHODES

        #region Confirmation d'ajout d'un avis sur un établissement
        private async void btnConfirmerAjout_Clicked(object sender, EventArgs e)
        {
            if (eNote.Text == "" || eDescription.Text == "")
            {
                await Application.Current.MainPage.DisplayAlert("AjoutAvis", "Erreur", "OK");
            }
            else
            {
                string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
                var db = new SQLiteConnection(dpPath);
                var data = db.Table<Etablissements>();
                var data2 = db.Table<Utilisateurs>();
                var user = data2.Where(x => x.Pseudo == pseudo).FirstOrDefault(); //Linq Query 
                var etablissement = data.Where(x => x.Id == ID).FirstOrDefault();
                await DisplayAlert("Ajout Avis Etablissement", "Ajout Réussi.", "OK");
                await App.AvisEtablissementRepository.AjoutNouvelAvisEtablissementAsync( eDescription.Text, int.Parse(eNote.Text), user.Id, ID);
                await Navigation.PushAsync(new PageEtablissementDetail(ID, pseudo));
            }
        }
        #endregion

        #region Redirection vers le détail de l'établissement
        private async void btnAnnulerAjout_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageEtablissementDetail(ID, pseudo));
        }
        #endregion

        #endregion
    }
}