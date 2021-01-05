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
    public partial class PageGererAvisRecette : ContentPage
    {
        #region INITIALISATION DES VARIABLES
        string Pseudo = "";
        #endregion

        #region CONSTRUCTEUR PageGererAvisRecette
        public PageGererAvisRecette(string pseudo)
        {
            InitializeComponent();
            Pseudo = pseudo;
            remplirLvAvisRecettesUser();
        }
        #endregion

        #region METHODES

        #region Remplissage de la liste des avis sur les recettes de l'user
        private async void remplirLvAvisRecettesUser()
        {
            List<AvisRecette> avisrecettes = await App.AvisRecetteRepository.RecupererAllAvisRecetteUser(Pseudo);
            lvAvisRecette.ItemsSource = avisrecettes;
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

        #region Affichage de l'avis et de l'établissement lié
        private async void lvAvisRecette_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {

                AvisRecette avisrecettes = (AvisRecette)lvAvisRecette.SelectedItem;

                if (avisrecettes == null)
                    return;

                await Navigation.PushAsync(new PageGestionAvisRecettesDetails(avisrecettes.Id, Pseudo, avisrecettes.IdRecette));
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Retour à l'accueil
        private async void btnRetourAccueil_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Accueil(Pseudo));
        }
        #endregion

        #endregion
    }
}