using Projet_Xamarin_V1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Projet_Xamarin_V1.Pages_Avis
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageGererAvisEtablissement : ContentPage
    {
        #region INITIALISATION DES VARIABLES
        string Pseudo = "";
        #endregion

        #region CONSTRUCTEUR PageGererAvisEtablissement
        public PageGererAvisEtablissement(string pseudo)
        {
            InitializeComponent();
            Pseudo = pseudo;
            remplirLvAvisEtablissementsUser();
        }
        #endregion

        #region METHODES

        #region Remplissage liste des avis etablissements
        private async void remplirLvAvisEtablissementsUser()
        {
            List<AvisEtablissement> avisetablissements = await App.AvisEtablissementRepository.RecupererAllAvisEtablissementsUser(Pseudo);
            lvAvisEtablissement.ItemsSource = avisetablissements;
        }
        #endregion

        #region Redirection vers l'accueil
        private async void btnRetourAccueil_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Accueil(Pseudo));
        }
        #endregion

        #region Déconnexion
        private async void btnDeconnexion_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Déconnexion", "Voulez vous vraiment vous déconnecter ?", "Oui", "Non");
            if (answer == true)
            {
                await Navigation.PushAsync(new MainPage());
            }
        }
        #endregion

        #region Redirection vers la page de détail de l'avis
        private async void lvAvisEtablissement_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {

                AvisEtablissement avisetablissements = (AvisEtablissement)lvAvisEtablissement.SelectedItem;

                if (avisetablissements == null)
                    return;

                Console.WriteLine(avisetablissements.Id+"\n");
                Console.WriteLine(Pseudo + "\n");
                Console.WriteLine(avisetablissements.IdEtablissement + "\n");
                await Navigation.PushAsync(new PageGestionAvisEtablissementDetail(avisetablissements.Id, Pseudo, avisetablissements.IdEtablissement));
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #endregion
    }
}