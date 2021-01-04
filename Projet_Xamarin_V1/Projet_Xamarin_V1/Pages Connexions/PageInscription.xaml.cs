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
using System.Text.RegularExpressions;

namespace Projet_Xamarin_V1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageInscription : ContentPage
    {
        #region INITIALISATION DES VARIABLES
        public const string regexEmail = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        public bool CompareMdpOK = false;
        public bool VerifAge = false;
        #endregion

        #region CONSTRUCTEUR PAGEINSCRIPTION
        public PageInscription()
        {
            InitializeComponent();
        }
        #endregion

        #region CONFIRMATION AJOUT USER
        private async void btnConfirmerAjout_Clicked(object sender, EventArgs e)
        {
            if(ePseudo.Text != "" && eMotDePasse.Text != "" && eEmail.Text != "" && dtpDateDeNaissance.Date != null && CompareMdpOK == true && VerifAge == true)
            {
                await DisplayAlert("Inscription", "Inscription réussie.", "OK");
                await App.UtilisateursRepository.AjoutNouvelUtilisateurAsync(ePseudo.Text, eMotDePasse.Text, eEmail.Text, DateTime.Now, dtpDateDeNaissance.Date);
                await Navigation.PushAsync(new MainPage());
            }
            else if(CompareMdpOK == false && ePseudo.Text != "" && eMotDePasse.Text != "" && eEmail.Text != "" && dtpDateDeNaissance.Date != null && VerifAge == true)
            {
                await DisplayAlert("Inscription", "Le mot de passe de confirmation ne correspond pas !", "OK");
            }
            else if(CompareMdpOK == true && ePseudo.Text != "" && eMotDePasse.Text != "" && eEmail.Text != "" && dtpDateDeNaissance.Date != null && VerifAge == false)
            {
                await DisplayAlert("Inscription", "Vous n'avez pas l'âge nécessaire !", "OK");
            }
            else if (CompareMdpOK == false && ePseudo.Text != "" && eMotDePasse.Text != "" && eEmail.Text != "" && dtpDateDeNaissance.Date != null && VerifAge == false)
            {
                await DisplayAlert("Inscription", "Vous n'avez pas l'âge nécessaire et les mots de passes ne correspondent pas!", "OK");
            }
            else if(ePseudo.Text == "" || eMotDePasse.Text == "" || eConfirmerMotDePasse.Text == "" || eEmail.Text == "" || dtpDateDeNaissance.Date == DateTime.Now)
            {
                await DisplayAlert("Inscription", "Veuillez remplir tous les champs.", "OK");
            }
        }
        #endregion

        #region SECURITE DES CHAMPS

        #region Gestion Email
        
        private async void eEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(eEmail.Text.Length > 100)
            {
                await DisplayAlert("Inscription", "Maximum 100 caractères pour l'email !", "OK");
                eEmail.Text = "";
            }
        }

        private async void eEmail_Unfocused(object sender, FocusEventArgs e)
        {
            if (VerificationSyntaxeEmail(eEmail.Text) == false)
            {
                await DisplayAlert("Inscription", "Erreur de syntaxe dans l'email.", "OK");
                eEmail.Text = "";
            }
            else //Si la syntaxe est bonne on vérifie si il n'existe pas déjà
            {
                VerificationEmailExiste();
            }
        }

        private bool VerificationSyntaxeEmail(string email)
        {
            if (email != null)
            {
                return Regex.IsMatch(email, regexEmail);
            }
            else
            {
                return false;
            }
        }

        private async void VerificationEmailExiste()
        {
            string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<Utilisateurs>(); //Call Table
            var emailExiste = data.Where(x => x.Mail == eEmail.Text).FirstOrDefault(); //Linq Query  

            if (emailExiste != null)
            {
                await DisplayAlert("Inscription", "L'email : " + emailExiste.Mail + " existe déjà !", "OK");
                eEmail.Text = "";
            }
        }
        #endregion

        #region gestion MDP
        private async void eMotDePasse_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (eMotDePasse.Text.Length > 25)
            {
                await DisplayAlert("Inscription", "Maximum 25 caractères pour le mot de passe !", "OK");
                eMotDePasse.Text = "";
            }
        }

        private void eMotDePasse_Unfocused(object sender, FocusEventArgs e)
        {
            ComparerMotDePasse();
        }

        private async void eConfirmerMotDePasse_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (eMotDePasse.Text.Length > 25)
            {
                await DisplayAlert("Inscription", "Maximum 25 caractères pour le mot de passe !", "OK");
                eConfirmerMotDePasse.Text = "";
            }
        }
        private void eConfirmerMotDePasse_Unfocused(object sender, FocusEventArgs e)
        {
            ComparerMotDePasse();
        }

        private void ComparerMotDePasse()
        {
            if(eMotDePasse.Text == eConfirmerMotDePasse.Text)
            {
                eMotDePasse.TextColor = System.Drawing.Color.Orange;
                eConfirmerMotDePasse.TextColor = System.Drawing.Color.Orange;
                CompareMdpOK = true;
            }
            else
            {
                eMotDePasse.TextColor = System.Drawing.Color.Red;
                eConfirmerMotDePasse.TextColor = System.Drawing.Color.Red;
                CompareMdpOK = false;
            }
        }
        #endregion

        #region Gestion Pseudo
        private async void ePseudo_Unfocused(object sender, FocusEventArgs e)
        {
            string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<Utilisateurs>(); //Call Table
            var pseudoExiste = data.Where(x => x.Pseudo == ePseudo.Text).FirstOrDefault(); //Linq Query  

            if (pseudoExiste != null)
            {
                await DisplayAlert("Inscription", "Le pseudo : " + pseudoExiste.Pseudo + " existe déjà !", "OK");
                ePseudo.Text = "";
            }
        }
        #endregion

        #region Gestion DateNaissance
        private async void dtpDateDeNaissance_DateSelected(object sender, DateChangedEventArgs e)
        {
            VerificationAge();
            if(VerifAge == false)
            {
                await DisplayAlert("Date de naissance", "Il faut avoir 10 ans minimum pour s'inscrire", "OK");
            }
        }

        private void VerificationAge()
        {
            DateTime dateAnniv = dtpDateDeNaissance.Date;
            int age = DateTime.Now.Year - dtpDateDeNaissance.Date.Year;
            //Si l'anniversaire n'est pas encore passé, on retire 1 an
            if (dateAnniv > DateTime.Now)
            {
                age--;
            }

            if (age <= 10)
            {

                VerifAge = false;
            }
            VerifAge = true;
        }
        #endregion

        #endregion
    }
}