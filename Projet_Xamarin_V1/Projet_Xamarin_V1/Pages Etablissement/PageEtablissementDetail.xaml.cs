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
    public partial class PageEtablissementDetail : ContentPage
    {
        #region INITIALISATION DES VARIABLES
        public string Pseudo = "";
        public int ID = 0;
        #endregion

        #region CONSTRUCTEUR PageEtablissementDetail
        public PageEtablissementDetail(int id, string pseudo)
        {
            InitializeComponent();
            Pseudo = pseudo;
            ID = id;
            remplirEtablissement(ID);
            remplirLvAvisEtablissement();
        }
        #endregion

        #region METHODES

        #region Remplissage des détails de l'établissement
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
            eVille.Text = remplissage.Ville.ToString();
            eCodePostal.Text = remplissage.CodePostal.ToString();
            ePays.Text = remplissage.Pays;
            eBudget.Text = remplissage.Budget.ToString() + "/5";
            eCaracteristique.Text = caracteristique.Nom;
            imgEtablissement.Source = remplissage.UrlImage;
        }
        #endregion

        #region Redirection vers l'ajout d'un avis
        private async void btnAjouterAvis_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageAjouterAvisEtablissement(Pseudo, ID));
        }
        #endregion

        #region Remplissage de la liste des avis sur l'établissement
        private async void remplirLvAvisEtablissement()
        {
            List<AvisEtablissement> avis = await App.AvisEtablissementRepository.RecupererAllAvisEtablissements(ID);
            lvAvisEtablissement.ItemsSource = avis;
        }
        #endregion

        #region Refresh de la liste des avis de l'établissements
        private void lvAvisEtablissement_Refreshing(object sender, EventArgs e)
        {
            remplirLvAvisEtablissement();
            lvAvisEtablissement.EndRefresh();
        }
        #endregion

        #endregion
    }
}