using Projet_Xamarin_V1.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Projet_Xamarin_V1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageAjoutEtablissement : ContentPage
    {
        #region INITIALISATION DES VARIABLES
        public const string regexnumero = @"^[0-9]{1,3}$";
        public const string regexcodepostal = @"^[0-9]{4}$";
        public const string regexbudget = @"^[0-5]{1}$";
        public string Pseudo = "";
        public string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database 
        #endregion

        #region CONSTRUCTEUR PageAjoutEtablissement
        public PageAjoutEtablissement(string pseudo)
        {
            InitializeComponent();
            remplirPickerCaracteristiques();
            Pseudo = pseudo;
        }
        #endregion

        #region METHODES

        #region Confirmation de l'ajout de l'établissement
        private async void btnConfirmerAjout_Clicked(object sender, EventArgs e)
        {
            if (eNom.Text != "" && eRue.Text != "" && eNumero.Text != "" && eCodePostal.Text != "" && eVille.Text != "" && ePays.Text != "" && eBudget.Text != "" && eUrl.Text != "")
            {
                var db = new SQLiteConnection(dpPath);
                var data = db.Table<Utilisateurs>(); //Call Table
                var data2 = db.Table<Caracteristiques>(); //Call Table
                var C = pCaracteristiques.SelectedItem.ToString();
                var user = data.Where(x => x.Pseudo == Pseudo).FirstOrDefault();
                var caracteristique = data2.Where(x => x.Nom == C).FirstOrDefault();
                await DisplayAlert("Ajout Etablissement", "Ajout Réussi.", "OK");
                await App.EtablissementsRepository.AjoutNouvelEtablissementAsync(eNom.Text, eRue.Text, int.Parse(eNumero.Text), eVille.Text, int.Parse(eCodePostal.Text), ePays.Text, int.Parse(eBudget.Text), eUrl.Text, user.Id, caracteristique.Id);
                await Navigation.PushAsync(new Accueil(Pseudo));
            }
            else
            {
                await DisplayAlert("Ajout Etablissement", "Remplissez tous les champs", "OK");
            }
        }
        #endregion

        #region Remplissage du picker caracteristique
        private async void remplirPickerCaracteristiques()
        {
            string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<Caracteristiques>(); //Call Table

            List<Caracteristiques> caracteristiques = await App.CaracteristiquesRepository.RecupererAllCaracteristiques();

            pCaracteristiques.Items.Clear();

            foreach (var car in caracteristiques)
            {
                pCaracteristiques.Items.Add(car.Nom);
            }
        }
        #endregion

        #region Gestion de la sécurité des champs
        private void eNom_Unfocused(object sender, FocusEventArgs e)
        {
            if (eNom.Text != "" && eRue.Text != "" && eNumero.Text != "" && eCodePostal.Text != "" && eVille.Text != "" && ePays.Text != "")
            {
                EtablissementExiste();
            }
        }

        private void eRue_Unfocused(object sender, FocusEventArgs e)
        {
            if (eNom.Text != "" && eRue.Text != "" && eNumero.Text != "" && eCodePostal.Text != "" && eVille.Text != "" && ePays.Text != "")
            {
                EtablissementExiste();
            }
        }

        private async void eNumero_Unfocused(object sender, FocusEventArgs e)
        {
            if (VerificationNumero(eNumero.Text) == false)
            {
                await DisplayAlert("Ajout Etablissement", "Syntaxe de numéro incorrecte.", "OK");
                eNumero.Text = "";
            }
            else
            {
                if (eNom.Text != "" && eRue.Text != "" && eNumero.Text != "" && eCodePostal.Text != "" && eVille.Text != "" && ePays.Text != "")
                {
                    EtablissementExiste();
                }
            }
        }

        private void eVille_Unfocused(object sender, FocusEventArgs e)
        {
            if (eNom.Text != "" && eRue.Text != "" && eNumero.Text != "" && eCodePostal.Text != "" && eVille.Text != "" && ePays.Text != "")
            {
                EtablissementExiste();
            }
        }

        private async void eCodePostal_Unfocused(object sender, FocusEventArgs e)
        {
            if (VerificationCodePostal(eCodePostal.Text) == false)
            {
                await DisplayAlert("Ajout Etablissement", "Syntaxe de code postal incorrecte.", "OK");
                eCodePostal.Text = "";
            }
            else
            {
                if (eNom.Text != "" && eRue.Text != "" && eNumero.Text != "" && eCodePostal.Text != "" && eVille.Text != "" && ePays.Text != "")
                {
                    EtablissementExiste();
                }
            }
        }

        private void ePays_Unfocused(object sender, FocusEventArgs e)
        {
            if (eNom.Text != "" && eRue.Text != "" && eNumero.Text != "" && eCodePostal.Text != "" && eVille.Text != "" && ePays.Text != "")
            {
                EtablissementExiste();
            }
        }
        private async void eBudget_Unfocused(object sender, FocusEventArgs e)
        {
            if (VerificationBudget(eBudget.Text) == false)
            {
                await DisplayAlert("Ajout Etablissement", "Syntaxe de budget incorrecte.", "OK");
                eBudget.Text = "";
            }
        }
        #endregion

        #region Regex utilisé dans la sécurité des champs
        private bool VerificationNumero(string numero)
        {
            if (numero != null)
            {
                return Regex.IsMatch(numero, regexnumero);
            }
            else
            {
                return false;
            }
        }
        private bool VerificationCodePostal(string codepostal)
        {
            if (codepostal != null)
            {
                return Regex.IsMatch(codepostal, regexcodepostal);
            }
            else
            {
                return false;
            }
        }
        private bool VerificationBudget(string budget)
        {
            if (budget != null)
            {
                return Regex.IsMatch(budget, regexbudget);
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Vérification des longueurs des champs

        private async void eNom_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (eNom.Text.Length > 50)
            {
                await DisplayAlert("Ajout Etablissement", "Nombre de caractères dépassés", "OK");
                eNom.Text = "";
            }
        }
        private async void eRue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (eRue.Text.Length > 100)
            {
                await DisplayAlert("Ajout Etablissement", "Nombre de caractères dépassés", "OK");
                eRue.Text = "";
            }
        }
        private async void eVille_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (eVille.Text.Length > 50)
            {
                await DisplayAlert("Ajout Etablissement", "Nombre de caractères dépassés", "OK");
                eVille.Text = "";
            }
        }
        private async void ePays_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ePays.Text.Length > 50)
            {
                await DisplayAlert("Ajout Etablissement", "Nombre de caractères dépassés", "OK");
                ePays.Text = "";
            }
        }
        #endregion

        #region Vérification si l'établissement existe déjà
        private async void EtablissementExiste()
        {
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<Etablissements>(); //Call Table
            int Num = int.Parse(eNumero.Text);
            int CP = int.Parse(eCodePostal.Text);
            var etablissementExiste = data.Where(x => x.Nom == eNom.Text && x.Rue == eRue.Text && x.Numero == Num && x.CodePostal == CP && x.Ville == eVille.Text && x.Pays == ePays.Text).FirstOrDefault();
            if (etablissementExiste != null)
            {
                await DisplayAlert("Ajout Etablissement", etablissementExiste.Nom + " existe déjà.", "OK");
                eNom.Text = "";
                eRue.Text = "";
                eNumero.Text = "";
                eCodePostal.Text = "";
                eVille.Text = "";
                ePays.Text = "";
            }
        }
        #endregion

        #region Redirection vers l'accueil
        private async void btnAnnulerAjout_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Accueil(Pseudo));
        }
        #endregion

        #endregion

    }
}