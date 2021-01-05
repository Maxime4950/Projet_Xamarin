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
    public partial class PageRecetteDetail : ContentPage
    {
        #region INITIALISATION DES VARIABLES
        public string Pseudo = "";
        public int ID = 0;
        #endregion

        #region CONSTRUCTEUR PageRecetteDetail
        public PageRecetteDetail(int id, string pseudo)
        {
            InitializeComponent();
            Pseudo = pseudo;
            ID = id;
            remplirRecette(ID);
            remplirLvAvisrecettes();
        }
        #endregion

        #region METHODES

        #region Remplissage du détail de la recette
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
        #endregion

        #region Redirection vers l'ajout d'un avis
        private async void btnAjouterAvis_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageAjouterAvisRecettes(Pseudo, ID));
        }
        #endregion

        #region Remplir liste des avis recettes
        private async void remplirLvAvisrecettes()
        {
            List<AvisRecette> avis = await App.AvisRecetteRepository.RecupererAllAvisRecette(ID);
            lvAvisRecettes.ItemsSource = avis;
        }
        #endregion

        #region Refresh liste des avis recettes
        private void lvAvisRecettes_Refreshing(object sender, EventArgs e)
        {
            remplirLvAvisrecettes();
            lvAvisRecettes.EndRefresh();
        }
        #endregion

        #endregion
    }
}