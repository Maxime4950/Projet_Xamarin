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

namespace Projet_Xamarin_V1.Pages_Recette
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageModifierRecette : ContentPage
    {
        public string Pseudo = "";
        public int ID = 0;
        public const string regexbudget = @"^[0-5]{1}$";
        public string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3");
        public PageModifierRecette(int id, string pseudo)
        {
            InitializeComponent();
            remplirRecette(id);
            Pseudo = pseudo;
            ID = id;
        }

        public void remplirRecette(int id)
        {
            string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<Recettes>(); //Call Table
            var remplissage = data.Where(x => x.Id == id).FirstOrDefault(); //Linq Query 
            var data2 = db.Table<Caracteristiques>();
            var caracteristique = data2.Where(x => x.Id == remplissage.IdCaracteristiques).FirstOrDefault();
            var data3 = db.Table<TypeRecette>();
            var type = data3.Where(x => x.Id == remplissage.IdTypeRecette).FirstOrDefault();

            eNom.Text = remplissage.Nom;
            eBudget.Text = remplissage.Budget.ToString();
            eIngredients.Text = remplissage.Ingredients;
            eDescription.Text = remplissage.Description;
            eUrl.Text = remplissage.UrlImage;
            remplirPickerCaracteristiques(caracteristique.Nom);
            remplirPickerTypes(type.Nom);
        }
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
                if (c.Nom == nom)
                {
                    pCaracteristiques.SelectedIndex = c.Id - 1;
                }
            }
        }

        private async void remplirPickerTypes(string nom)
        {

            List<TypeRecette> typeRecettes = await App.TypesRepository.RecupererAllTypes();

            pType.Items.Clear();

            foreach (var car in typeRecettes)
            {
                pType.Items.Add(car.Nom);
            }
            foreach (TypeRecette t in typeRecettes)
            {
                if (t.Nom == nom)
                {
                    pType.SelectedIndex = t.Id - 1;
                }
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
        private async void eNom_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (eNom.Text.Length > 50)
            {
                await DisplayAlert("Ajout Recette", "Nombre de caractères dépassés", "OK");
                eNom.Text = "";
            }
        }

        private async void eBudget_Unfocused(object sender, FocusEventArgs e)
        {
            if (VerificationBudget(eBudget.Text) == false)
            {
                await DisplayAlert("Ajout Recette", "Syntaxe de budget incorrecte.", "OK");
                eBudget.Text = "";
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

        private async void eIngredients_Unfocused(object sender, FocusEventArgs e)
        {
            if (eIngredients.Text.Length > 250)
            {
                await DisplayAlert("Ajout Recette", "Nombre de caractères dépassés", "OK");
                eIngredients.Text = "";
            }
        }



        private async void btnModifierRecette_Clicked(object sender, EventArgs e)
        {
            if (eNom.Text != "" && eBudget.Text != "" && eIngredients.Text != "" && eDescription.Text != "" && eUrl.Text != "")
            {
                var db = new SQLiteConnection(dpPath);
                var data = db.Table<Utilisateurs>(); //Call Table
                var data2 = db.Table<Caracteristiques>(); //Call Table
                var data3 = db.Table<TypeRecette>();
                var CaractSelect = pCaracteristiques.SelectedItem.ToString();
                var TypeSelect = pType.SelectedItem.ToString();
                var user = data.Where(x => x.Pseudo == Pseudo).FirstOrDefault();
                var caracteristique = data2.Where(x => x.Nom == CaractSelect).FirstOrDefault();
                var type = data3.Where(x => x.Nom == TypeSelect).FirstOrDefault();
                bool rep = await App.RecettesRepository.ModifierRecettesAsync(eNom.Text, int.Parse(eBudget.Text), eIngredients.Text, eDescription.Text, eUrl.Text, caracteristique.Id, type.Id, ID);
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
                await DisplayAlert("Modification Recette", "Remplissez tous les champs", "OK");
            }
        }

        private async void btnAnnulerModification_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageGestionRecetteDetail(ID, Pseudo));
        }
    }
}