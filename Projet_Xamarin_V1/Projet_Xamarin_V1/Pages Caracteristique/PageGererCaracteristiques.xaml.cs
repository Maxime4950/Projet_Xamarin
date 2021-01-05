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
    //CETTE PAGE CENTRALISE LA GESTION DES CARACTERISTIQUES
    public partial class PageGererCaracteristiques : ContentPage
    {
        #region INITIALISATION DES VARIABLES
        #endregion

        #region CONSTRUCTEUR PageGererCaracteristiques()
        public PageGererCaracteristiques()
        {
            InitializeComponent();
            remplirLvCaracteristiques();
        }
        #endregion

        #region METHODES

        #region Redirection vers l'ajout de caractéristique
        private async void btnAjouterCaracteristiques_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageAjouterCaracteristique());
        }
        #endregion

        #region Redirection vers la modification de caractéristique
        private async void btnModifierCaracteristiques_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageModifierCaracteristique());
        }
        #endregion

        #region Redirection vers la suppression de caractéristique
        private async void btnSupprimerCaracteristiques_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageSupprimerCaracteristique());
        }
        #endregion

        #region DECONNEXION
        private async void btnDeconnexion_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Déconnexion", "Voulez vous vraiment vous déconnecter ?", "Oui", "Non");
            if (answer == true)
            {
                await Navigation.PushAsync(new MainPage());
            }
        }
        #endregion

        #region REMPLISSAGE LISTE DES CARACTERISTIQUES
        private async void remplirLvCaracteristiques()
        {
            List<Caracteristiques> caracteristiques = await App.CaracteristiquesRepository.RecupererAllCaracteristiques();
            lvCaracteristiques.ItemsSource = caracteristiques;
        }
        #endregion

        #region REFRESH DE LA LISTE (slide vers le bas dans la liste)
        private void lvCaracteristiques_Refreshing(object sender, EventArgs e)
        {
            remplirLvCaracteristiques();
            lvCaracteristiques.EndRefresh();
        }
        #endregion
        #endregion
    }
}