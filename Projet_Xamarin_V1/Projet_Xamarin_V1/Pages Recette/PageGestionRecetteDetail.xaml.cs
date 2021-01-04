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

namespace Projet_Xamarin_V1.Pages_Recette
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageGestionRecetteDetail : ContentPage
    {
        public string Pseudo = "";
        public int ID = 0;
        public PageGestionRecetteDetail(int id, string pseudo)
        {
            InitializeComponent();
            ID = id;
            Pseudo = pseudo;
            remplirRecette(ID);
        }

        private void remplirRecette(int id)
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
            imgRecette.Source = remplissage.UrlImage;
            eCaracteristique.Text = caracteristique.Nom;
            eType.Text = type.Nom;
        }

        private async void btnSupprimer_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Question?", "Voulez vous vraiment supprimer votre recette ?", "Oui", "Non");
            if (answer == true)
            {
                bool rep = await App.RecettesRepository.SupprimerRecetteAsync(eNom.Text, int.Parse(eBudget.Text), eIngredients.Text, eDescription.Text);
                if (rep == true)
                {
                    await DisplayAlert("Suppression", "La suppression à été effectuée avec succès : redirection vers la page de liste des établissements.", "OK");
                    await Navigation.PushAsync(new PageGererRecetteUser(Pseudo));
                }
                else
                {
                    await DisplayAlert("Suppression", "La suppression à connu une erreur.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Suppression", "Suppression annulée", "OK");
            }
        }

        private void btnModifier_Clicked(object sender, EventArgs e)
        {

        }
    }
}