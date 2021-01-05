using Projet_Xamarin_V1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Projet_Xamarin_V1.Pages_Recette
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    //CETTE PAGE SERA UTILISEE DANS LE PROFIL POUR GERER LES RECETTES CREES PAR L'USER
    public partial class PageGererRecetteUser : ContentPage
    {
        #region INITIALISATION DES VARIABLES
        string Pseudo = "";
        #endregion

        #region CONSTRUCTEUR PageGererRecetteUser
        public PageGererRecetteUser(string pseudo)
        {
            InitializeComponent();
            Pseudo = pseudo;
            remplirLvRecetteUser();
        }
        #endregion

        #region METHODES

        #region Remplissage de la liste des rcettes créés par l'user
        private async void remplirLvRecetteUser()
        {
            List<Recettes> recettes = await App.RecettesRepository.RecupererAllRecettesUser(Pseudo);
            lvRecette.ItemsSource = recettes;
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

        #region Affichage du détail de la recette au click
        private async void lvRecette_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                Recettes recettes = (Recettes)lvRecette.SelectedItem;

                if (recettes == null)
                    return;

                await Navigation.PushAsync(new PageGestionRecetteDetail(recettes.Id, Pseudo));
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Redirection vers l'accueil
        private async void btnRetourAcceuil_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Accueil(Pseudo));
        }
        #endregion

        #endregion
    }
}