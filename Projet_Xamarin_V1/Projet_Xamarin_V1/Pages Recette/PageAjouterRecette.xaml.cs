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
    public partial class PageAjouterRecette : ContentPage
    {
        #region INITIALISATION DES VARIABLES
        public const string regexbudget = @"^[0-5]{1}$";
        public string Pseudo = "";
        public string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3");
        #endregion

        #region CONSTRUCTEUR PageAjouterRecette
        public PageAjouterRecette(string pseudo)
        {
            InitializeComponent();
            remplirPickerCaracteristiques();
            remplirPickerTypeRecette();
            Pseudo = pseudo;
        }
        #endregion

        #region Confirmation de l'ajout
        private async void btnConfirmerAjout_Clicked(object sender, EventArgs e)
        {
            if (eNomRecette.Text != "" && eBudget.Text != "" && eIngredients.Text != "" && eDescription.Text != "")
            {


                var db = new SQLiteConnection(dpPath);
                var data = db.Table<Utilisateurs>();
                var data2 = db.Table<Caracteristiques>();
                var data3 = db.Table<TypeRecette>();
                var CaractSelect = pCaract.SelectedItem.ToString();
                var TypeSelect = pType.SelectedItem.ToString();

                var user = data.Where(x => x.Pseudo == Pseudo).FirstOrDefault();
                var caracteristique = data2.Where(x => x.Nom == CaractSelect).FirstOrDefault();
                var typerecette = data3.Where(x => x.Nom == TypeSelect).FirstOrDefault();

                await App.RecettesRepository.AjoutNouvelleRecetteAsync(eNomRecette.Text, eIngredients.Text, eDescription.Text, int.Parse(eBudget.Text), eUrl.Text, user.Id, typerecette.Id, caracteristique.Id);
                await DisplayAlert("AjoutRecette", "Recette bien Ajoutée", "OK");

                await Navigation.PushAsync(new Accueil(Pseudo));
            }
            else
            {
                await DisplayAlert("AjoutRecette", "Remplissez tous les champs", "OK");
            }
        }
        #endregion

        #region Remplissage du picker caractéristique
        private async void remplirPickerCaracteristiques()
        {
            string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<Caracteristiques>(); //Call Table

            List<Caracteristiques> caracteristiques = await App.CaracteristiquesRepository.RecupererAllCaracteristiques();

            pCaract.Items.Clear();

            foreach (var car in caracteristiques)
            {
                pCaract.Items.Add(car.Nom);
            }
        }
        #endregion

        #region Remplissage du picker typerecette
        private async void remplirPickerTypeRecette()
        {
            string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3");
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<TypeRecette>();

            List<TypeRecette> typerecette = await App.TypesRepository.RecupererAllTypes();

            foreach(var typ in typerecette)
            {
                pType.Items.Add(typ.Nom);
            }
        }
        #endregion

        #region Redirection vers l'accueil
        private async void btnAnnulerAjout_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Accueil(Pseudo));
        }
        #endregion

        #region Sécurité des champs (unfocused)
        private async void eNomRecette_Unfocused(object sender, FocusEventArgs e)
        {
            if (eNomRecette.Text == "")
            {
                await DisplayAlert("AjoutRecette", "Syntaxe du Nom vide", "OK");
            }
        }
        private async void eIngredients_Unfocused(object sender, FocusEventArgs e)
        {
            if (eIngredients.Text == "")
            {
                await DisplayAlert("AjoutRecette", "Syntaxe d'Ingredient vide", "OK");
            }
        }

        private async void eDescription_Unfocused(object sender, FocusEventArgs e)
        {
            if (eDescription.Text == "")
            {
                await DisplayAlert("AjoutRecette", "Syntaxe de Description vide", "OK");
            }
        }
        private async void eBudget_Unfocused(object sender, FocusEventArgs e)
        {
            if (VerifBudget(eBudget.Text) == false)
            {
                await DisplayAlert("AjoutRecette", "Syntaxe de Budget vide ou incorrecte.", "OK");
                eBudget.Text = "";
            }
        }
        private async void eUrl_Unfocused(object sender, FocusEventArgs e)
        {
            if (eUrl.Text == "")
            {
                await DisplayAlert("AjoutRecette", "Url manquant", "OK");
            }
        }

        #endregion

        #region Verif longueur

        private async void eNomRecette_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (eNomRecette.Text.Length > 50)
            {
                await DisplayAlert("Ajout Recette", "Nombre de caractères dépassés", "OK");
                eNomRecette.Text = "";
            }
        }

        private async void eIngredients_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (eIngredients.Text.Length > 250)
            {
                await DisplayAlert("Ajout Recette", "Nombre de caractères dépassés", "OK");
                eIngredients.Text = "";
            }
        }

        private async void eDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (eDescription.Text.Length > 500)
            {
                await DisplayAlert("Ajout Recette", "Nombre de caractères dépassés", "OK");
                eDescription.Text = "";
            }
        }


        #endregion

        #region Verif regex
        private bool VerifBudget(string budget)
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
    }
}