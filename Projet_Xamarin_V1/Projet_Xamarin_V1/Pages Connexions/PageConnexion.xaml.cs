using System;
using Projet_Xamarin_V1.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using SQLite;
using Xamarin.Essentials;
using Projet_Xamarin_V1.Repositories;

namespace Projet_Xamarin_V1
{
    public partial class MainPage : ContentPage
    {
        #region INITIALISATION DES VARIABLES
        #endregion

        #region CONSTRUCTEUR MainPage()
        public MainPage()
        {
            InitializeComponent();
            CreerAdmin();
        }
        #endregion

        #region METHODES

        #region Navigation vers la page inscription
        private async void btnInscription_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageInscription());
        }
        #endregion

        #region Création du compte admin
        private async void CreerAdmin()
        {
            string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<Utilisateurs>(); //Call Table
            var adminExiste = data.Where(x => x.Pseudo == "admin").FirstOrDefault(); //Linq Query  

            if(adminExiste == null)
            {
                await App.UtilisateursRepository.AjoutNouvelUtilisateurAsync("admin", "admin", "admin@gmail.com", DateTime.Now, DateTime.Now);
                await DisplayAlert("Inscription", "Le compte admin a été ajouté!", "OK");
            }
        }
        #endregion

        #region Gestion de la connexion (ADMIN et USER)
        private async void btnSeConnecter_Clicked(object sender, EventArgs e)
        {
            try
            {
                string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
                var db = new SQLiteConnection(dpPath);
                var data = db.Table<Utilisateurs>(); //Call Table
                var data1 = data.Where(x => x.Pseudo == eIdentifiant.Text && x.MotDePasse == eMotDePasse.Text).FirstOrDefault(); //Linq Query  
                if (data1 != null && data1.Pseudo != "admin") //Si la variable est != de null alors c'est qu'il y a une correspondance
                {
                    await DisplayAlert("Login", "Connecté avec succès.", "OK");
                    //Une fois l'utilisateur log on peut lancer la page d'accueil
                    await Navigation.PushAsync(new Accueil(eIdentifiant.Text));
                }
                else if(data1 != null && data1.Pseudo == "admin")
                {
                    await DisplayAlert("Login", "Connecté avec succès en tant qu'admin.", "OK");
                    //Une fois l'utilisateur log on peut lancer la page d'accueil
                    await Navigation.PushAsync(new AccueilAdmin(eIdentifiant.Text));
                }
                else
                {
                    await DisplayAlert("Login", "Erreur de connexion.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Login",ex.ToString(), "OK");
            }
        }
        #endregion

        #region Liste des utilisateurs (debug)
        /*
         private async void Button_Clicked(object sender, EventArgs e)
       {
           List<Utilisateurs> utilisateurs = await App.UtilisateursRepository.RecupererAllUtilisateurs();

           foreach (var user in utilisateurs)
           {
               Console.WriteLine($"{user.Pseudo}");
           }
       }*/
        #endregion

        #endregion
    }
}
