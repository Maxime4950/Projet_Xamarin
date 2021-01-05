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
        #region INITIALISATION DES VARIABLES
        int IDUtilisateur;
        #endregion

        #region CONSTRUCTEUR PageModificationProfil
        public PageModificationProfil(string pseudo)
        {
            InitializeComponent();
            remplirProfil(pseudo);
        }
        #endregion

        #region METHODES

        #region Remplissage des données du profil
        public void remplirProfil(string pseudo)
        {
            ePseudoProfil.Text = pseudo;
            string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<Utilisateurs>(); //Call Table
            var remplissage = data.Where(x => x.Pseudo == pseudo).FirstOrDefault(); //Linq Query 

            eMotDePasseProfil.Text = remplissage.MotDePasse; //Pour l'affichage sécurisé
            eEmailProfil.Text = remplissage.Mail;
            IDUtilisateur = remplissage.Id;
        }
        #endregion

        #region Confirmation de la modification du profil
        private async void btnConfirmerModif_Clicked(object sender, EventArgs e)
        {
            bool rep = await App.UtilisateursRepository.ModifierUtilisateurAsync(ePseudoProfil.Text, eMotDePasseProfil.Text, eEmailProfil.Text, IDUtilisateur);
            if(rep == true)
            {
                await DisplayAlert("Modification", "La modification à été effectuée avec succès, retour à l'accueil", "OK");
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
        #endregion

        #region Déconnexion
        private async void btnDeconnexion_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Déconnexion", "Voulez vous vraiment vous déconnecter ?", "Oui", "Non");
            if (answer == true)
            {
                await Navigation.PushAsync(new MainPage());
            }
        }
        #endregion

        #endregion
    }
}