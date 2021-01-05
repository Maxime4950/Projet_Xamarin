using Projet_Xamarin_V1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Projet_Xamarin_V1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    //PAGE QUI CENTRALISE LA GESTION DES TYPES
    public partial class PageGererTypes : ContentPage
    {
        #region INITIALISATION DES VARIABLES
        #endregion

        #region CONSTRUCTEUR pageGererTypes
        public PageGererTypes()
        {
            InitializeComponent();
            remplirLvTypes();
        }
        #endregion

        #region METHODES

        #region Redirection vers l'ajout des types

        private async void btnAjouterTypes_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageAjouterType());
        }
        #endregion

        #region Redirection vers la modification des types
        private async void btnModifierTypes_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageModifierType());
        }
        #endregion

        #region Redirection vers la suppression des types
        private async void btnSupprimerTypes_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageSupprimerType());
        }
        #endregion

        #region Remplissage de la listview des types
        private async void remplirLvTypes()
        {
            List<TypeRecette> types = await App.TypesRepository.RecupererAllTypes();
            lvTypes.ItemsSource = types;
        }
        #endregion

        #region Refresh de la lsitview
        private void lvTypes_Refreshing(object sender, EventArgs e)
        {
            remplirLvTypes();
            lvTypes.EndRefresh();
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

        #endregion
    }
}