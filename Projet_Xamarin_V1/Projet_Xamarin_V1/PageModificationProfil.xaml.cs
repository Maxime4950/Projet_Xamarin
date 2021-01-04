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
    public partial class PageModificationProfil : ContentPage
    {
        int IDUtilisateur;
        public PageModificationProfil(string pseudo)
        {
            InitializeComponent();
            remplirProfil(pseudo);
        }

        public void remplirProfil(string pseudo)
        {
            ePseudoProfil.Text = pseudo;
            string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<Utilisateurs>(); //Call Table
            var remplissage = data.Where(x => x.Pseudo == pseudo).FirstOrDefault(); //Linq Query 

            eMotDePasseProfil.Text = remplissage.MotDePasse; //Pour l'affichage sécurisé
            eEmailProfil.Text = remplissage.Mail;
            dtpDateNaissanceProfil.Date = remplissage.DateNaissance;
            IDUtilisateur = remplissage.Id;
        }

        private async void btnConfirmerModif_Clicked(object sender, EventArgs e)
        {

            bool rep = await App.UtilisateursRepository.ModifierUtilisateurAsync(ePseudoProfil.Text, eMotDePasseProfil.Text, eEmailProfil.Text, dtpDateNaissanceProfil.Date, IDUtilisateur);
            if(rep == true)
            {
                await DisplayAlert("Modification", "La modification à été effectuée avec succès", "OK");
                if(ePseudoProfil.Text == "admin")
                {
                    await Navigation.PushAsync(new AccueilAdmin(ePseudoProfil.Text));
                }
                else
                {
                    await Navigation.PushAsync(new Accueil(ePseudoProfil.Text));
                }
            }
            else
            {
                await DisplayAlert("Modification", "La modification à connu une erreur", "OK");
            }
     
        }

        private async void btnDeconnexion_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Déconnexion", "Voulez vous vraiment vous déconnecter ?", "Oui", "Non");
            if (answer == true)
            {
                await Navigation.PushAsync(new MainPage());
            }
        }
    }
}