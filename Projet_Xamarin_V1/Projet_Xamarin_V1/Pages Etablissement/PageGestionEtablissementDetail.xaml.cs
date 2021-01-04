using Projet_Xamarin_V1.Models;
using Projet_Xamarin_V1.Pages_Etablissement;
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
    public partial class PageGestionEtablissementDetail : ContentPage
    {
        public string Pseudo = "";
        public int ID = 0;
        public PageGestionEtablissementDetail(int id, string pseudo)
        {
            InitializeComponent();
            ID = id;
            Pseudo = pseudo;
            remplirEtablissement(ID);
            remplirLvAvisEtablissement();
        }

        private async void btnSupprimer_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Question?", "Voulez vous vraiment supprimer votre établissement ?", "Oui", "Non");
            if (answer == true)
            {
                bool rep = await App.EtablissementsRepository.SupprimerEtablissementAsync(eNom.Text, eRue.Text, int.Parse(eNumero.Text), eVille.Text, int.Parse(eCodePostal.Text), ePays.Text);
                if (rep == true)
                {
                    await DisplayAlert("Suppression", "La suppression à été effectuée avec succès : redirection vers la page de liste des établissements.", "OK");
                    await Navigation.PushAsync(new PageGererEtablissementsUser(Pseudo));
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
        private async void btnModifier_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageModifierEtablissement(ID, Pseudo));
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
    }
}