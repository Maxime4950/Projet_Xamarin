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

namespace Projet_Xamarin_V1.Pages_Avis
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageGestionAvisRecettesDetails : ContentPage
    {
        #region INITIALISATION DES VARIABLES
        public int id;
        public string Pseudo;
        public int idRecette;
        #endregion

        #region CONSTRUCTEUR PageGestionAvisRecettesDetails
        public PageGestionAvisRecettesDetails(int id, string Pseudo, int idRecette)
        {
            InitializeComponent();
            this.id = id;
            this.Pseudo = Pseudo;
            this.idRecette = idRecette;
            remplirRecette(idRecette);
            remplirAvisRecette(id);
        }
        #endregion

        #region METHODES

        #region Suppression de l'avis
        private async void btnSupprimerAvis_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Question?", "Voulez vous vraiment supprimer votre avis sur cette recette ?", "Oui", "Non");
            if (answer == true)
            {
                bool rep = await App.AvisRecetteRepository.SupprimerAvisRecetteAsync(id);
                if (rep == true)
                {
                    await DisplayAlert("Suppression", "La suppression à été effectuée avec succès : redirection vers la page de liste des avis établissements.", "OK");
                    await Navigation.PushAsync(new PageGererAvisRecette(Pseudo));
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
        #endregion

        #region Remplissage de la recette associé a l'avis
        private void remplirRecette(int id)
        {
            string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<Recettes>(); //Call Table
            var remplissage = data.Where(x => x.Id == id).FirstOrDefault(); //Linq Query 
            var data2 = db.Table<Caracteristiques>();
            var caracteristique = data2.Where(x => x.Id == remplissage.IdCaracteristiques).FirstOrDefault();
            var data3 = db.Table<TypeRecette>();
            var Type = data3.Where(x => x.Id == remplissage.IdTypeRecette).FirstOrDefault();

            eNom.Text = remplissage.Nom;
            eBudget.Text = remplissage.Budget.ToString();
            eIngredients.Text = remplissage.Ingredients;
            eDescription.Text = remplissage.Description;
            eCaracteristique.Text = caracteristique.Nom;
            eType.Text = Type.Nom;
            imgRecette.Source = remplissage.UrlImage;
        }
        #endregion

        #region Remplissage de l'avis sélectionné
        private void remplirAvisRecette(int id)
        {
            string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<AvisRecette>(); //Call Table
            var remplissage = data.Where(x => x.Id == id).FirstOrDefault(); //Linq Query 


            eNote.Text = remplissage.Note.ToString();
            eDescription.Text = remplissage.Description;
        }
        #endregion

        #endregion
    }
}