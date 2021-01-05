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
        public string Pseudo = "";
        public int ID = 0;
        public PageEtablissementDetail(int id, string pseudo)
        {
            InitializeComponent();
            Pseudo = pseudo;
            ID = id;
            remplirEtablissement(ID);
            remplirLvAvisEtablissement();
        }
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

        private async void btnAjouterAvis_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageAjouterAvisEtablissement(Pseudo, ID));
        }

        private async void remplirLvAvisEtablissement()
        {
            List<AvisEtablissement> avis = await App.AvisEtablissementRepository.RecupererAllAvisEtablissements(ID);
            lvAvisEtablissement.ItemsSource = avis;
        }

        private void lvAvisEtablissement_Refreshing(object sender, EventArgs e)
        {
            remplirLvAvisEtablissement();
            lvAvisEtablissement.EndRefresh();
        }

        private async void btnVueMaps_Clicked(object sender, EventArgs e)
        {
            var placemark = new Placemark
            {
                CountryName = ePays.Text,
                AdminArea = eCodePostal.Text,
                Thoroughfare = eNom.Text + " " + eNumero.Text,
                Locality = eVille.Text
            };
            var options = new MapLaunchOptions { Name = eNom.Text + " " + eNumero.Text/*, NavigationMode = NavigationMode.Driving*/ };

            await Map.OpenAsync(placemark, options);
        }
    }
}