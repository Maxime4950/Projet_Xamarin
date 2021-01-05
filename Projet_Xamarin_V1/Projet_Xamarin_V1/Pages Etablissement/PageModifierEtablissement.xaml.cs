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

namespace Projet_Xamarin_V1.Pages_Etablissement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageModifierEtablissement : ContentPage
    {
        #region INITIALISATION DES VARIABLES
        public string Pseudo = "";
        public int ID = 0;
        public const string regexnumero = @"^[0-9]{1,3}$";
        public const string regexcodepostal = @"^[0-9]{4}$";
        public const string regexbudget = @"^[0-5]{1}$";
        public string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3");
        #endregion

        #region CONSTRUCTEUR PageModifierEtablissement
        public PageModifierEtablissement(int id, string pseudo)
        {
            InitializeComponent();
            remplirEtablissement(id);
            Pseudo = pseudo;
            ID = id;
        }
        #endregion

        #region METHODES

        #region Remplissage des infos de l'établissement
        private void remplirEtablissement(int id)
        {
            string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<Etablissements>(); //Call Table
            var remplissage = data.Where(x => x.Id == id).FirstOrDefault(); //Linq Query 
            var data2 = db.Table<Caracteristiques>();
            var caracteristique = data2.Where(x => x.Id == remplissage.IdCaracteristique).FirstOrDefault();

            eNom.Text = remplissage.Nom;
            eRue.Text = remplissage.Rue;
            eNumero.Text = remplissage.Numero.ToString();
            eVille.Text = remplissage.Ville;
            ePays.Text = remplissage.Pays;
            eBudget.Text = remplissage.Budget.ToString();
            eCodePostal.Text = remplissage.CodePostal.ToString();
            eUrl.Text = remplissage.UrlImage;
            remplirPickerCaracteristiques(caracteristique.Nom);
        }
        #endregion

        #region Sécurité des champs (unfocused)
        private async void eNumero_Unfocused(object sender, FocusEventArgs e)
        {
            if (VerificationNumero(eNumero.Text) == false)
            {
                await DisplayAlert("Ajout Etablissement", "Syntaxe de numéro incorrecte.", "OK");
                eNumero.Text = "";
            }
        }
        private async void eCodePostal_Unfocused(object sender, FocusEventArgs e)
        {
            if (VerificationCodePostal(eCodePostal.Text) == false)
            {
                await DisplayAlert("Ajout Etablissement", "Syntaxe de code postal incorrecte.", "OK");
                eCodePostal.Text = "";
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

        #region Regex
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

        #region Vérification des longueurs

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

        #region Vérification si un établissement existe déjà
        private bool EtablissementExiste(int id)
        {
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<Etablissements>(); //Call Table
            int Num = int.Parse(eNumero.Text);
            int CP = int.Parse(eCodePostal.Text);
            var etablissementExiste = data.Where(x => x.Nom == eNom.Text && x.Rue == eRue.Text && x.Numero == Num && x.CodePostal == CP && x.Ville == eVille.Text && x.Pays == ePays.Text && x.Id != id).FirstOrDefault();
            if (etablissementExiste != null)
            {
                eNom.Text = "";
                eRue.Text = "";
                eNumero.Text = "";
                eCodePostal.Text = "";
                eVille.Text = "";
                ePays.Text = "";
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Remplissage du picker caractéristique
        private async void remplirPickerCaracteristiques(string nom)
        {

            List<Caracteristiques> caracteristiques = await App.CaracteristiquesRepository.RecupererAllCaracteristiques();

            pCaracteristiques.Items.Clear();

            foreach (var car in caracteristiques)
            {
                pCaracteristiques.Items.Add(car.Nom);
            }
            foreach (Caracteristiques c in caracteristiques)
            {
                if(c.Nom == nom)
                {
                    pCaracteristiques.SelectedIndex = c.Id - 1;
                }
            }
        }
        #endregion

        #region Confirmation de la modification
        private async void btnModifierEtablissement_Clicked(object sender, EventArgs e)
        {
            if ((eNom.Text != "" && eRue.Text != "" && eNumero.Text != "" && eCodePostal.Text != "" && eVille.Text != "" && ePays.Text != "" && eBudget.Text != "" && eUrl.Text != "") && EtablissementExiste(ID) == false)
            {
                var db = new SQLiteConnection(dpPath);
                var data = db.Table<Utilisateurs>(); //Call Table
                var data2 = db.Table<Caracteristiques>(); //Call Table
                var C = pCaracteristiques.SelectedItem.ToString();
                var user = data.Where(x => x.Pseudo == Pseudo).FirstOrDefault();
                var caracteristique = data2.Where(x => x.Nom == C).FirstOrDefault();
                bool rep = await App.EtablissementsRepository.ModifierEtablissementAsync(eNom.Text, eRue.Text, int.Parse(eNumero.Text), eVille.Text, int.Parse(eCodePostal.Text), ePays.Text, int.Parse(eBudget.Text), eUrl.Text, caracteristique.Id, ID);
                if (rep == true)
                {
                    await DisplayAlert("Modification", "La modification à été effectuée avec succès", "OK");
                    await Navigation.PushAsync(new Accueil(Pseudo));
                }
                else
                {
                    await DisplayAlert("Modification", "La modification à connu une erreur", "OK");
                }
            }
            else
            {
                await DisplayAlert("Modification Etablissement", "Remplissez tous les champs", "OK");
            }
        }
        #endregion

        #region Redirection vers la page de détail de l'établissement
        private async void btnAnnulerModification_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageGestionEtablissementDetail(ID, Pseudo));
        }
        #endregion

        #endregion
    }
}