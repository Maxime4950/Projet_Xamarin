using Projet_Xamarin_V1.Models;
using Projet_Xamarin_V1.Pages_Recette;
using SQLite;
using System;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Projet_Xamarin_V1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageAjouterAvisRecettes : ContentPage
    {
        #region INITIALISATION DES VARIABLES
        public string pseudo;
        public int IDrecette;
        #endregion

        #region CONSTRUCTEUR PageAjouterAvisRecettes
        public PageAjouterAvisRecettes(string pseudo, int ID)
        {
            InitializeComponent();
            this.pseudo = pseudo;
            this.IDrecette = ID;
        }
        #endregion

        #region METHODES

        #region Confirmation de l'ajout d'un avis sur une recette
        private async void btnConfirmerAjout_Clicked(object sender, EventArgs e)
        {
            if (eNote.Text == "" || eDescription.Text == "")
            {
                await Application.Current.MainPage.DisplayAlert("Ajout Avis Recette", "Remplissez tous les champs", "OK");
            }
            else
            {
                string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
                var db = new SQLiteConnection(dpPath);
                var data = db.Table<Utilisateurs>();
                var data2 = db.Table<Recettes>();
                var user = data.Where(x => x.Pseudo == pseudo).FirstOrDefault();
                var recette = data2.Where(x => x.Id == IDrecette).FirstOrDefault();
                await DisplayAlert("Ajout Avis Recette", "Ajout Réussi.", "OK");
                await App.AvisRecetteRepository.AjoutNouvelAvisRecetteAsync(eDescription.Text,int.Parse(eNote.Text), user.Id, IDrecette);
                await Navigation.PushAsync(new PageRecetteDetail(IDrecette, pseudo));
            }
        }
        #endregion

        #region redirection vers la page de détail des recettes
        private async void btnAnnulerAjout_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageRecetteDetail(IDrecette, pseudo));
        }
        #endregion

        #endregion
    }
}