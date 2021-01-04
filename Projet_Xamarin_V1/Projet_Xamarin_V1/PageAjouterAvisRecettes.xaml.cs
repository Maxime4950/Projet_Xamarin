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
        public string pseudo;
        public int IDrecette;

        public PageAjouterAvisRecettes(string pseudo, int ID)
        {
            InitializeComponent();
            this.pseudo = pseudo;
            this.IDrecette = ID;
        }
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

        private async void btnAnnulerAjout_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageRecetteDetail(IDrecette, pseudo));
        }
    }
}