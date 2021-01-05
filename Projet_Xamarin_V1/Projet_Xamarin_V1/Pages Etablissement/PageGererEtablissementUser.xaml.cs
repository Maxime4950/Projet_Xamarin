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
    //CETTE PAGE SERA UTILISEE DANS LE PROFIL POUR GERER LES ETABLISSEMENTS CREES PAR L'UTILISATEUR
    public partial class PageGererEtablissementsUser : ContentPage
    {
        #region INITIALISATION DES VARIABLES
        string Pseudo = "";
        #endregion

        #region CONSTRUCTEUR PageGererEtablissementsUser
        public PageGererEtablissementsUser(string pseudo)
        {
            InitializeComponent();
            Pseudo = pseudo;
            remplirLvEtablissementsUser();
        }
        #endregion

        #region METHODES

        #region Remplissage de la liste des établissements créés par l'utilisateur
        private async void remplirLvEtablissementsUser()
        {
            List<Etablissements> etablissements = await App.EtablissementsRepository.RecupererAllEtablissementsUser(Pseudo);
            lvEtablissement.ItemsSource = etablissements;
        }
        #endregion

        #region Si on click sur l'établissement => redirection vers le detail
        private async void lvEtablissement_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {

                Etablissements etablissements = (Etablissements)lvEtablissement.SelectedItem;

                if (etablissements == null)
                    return;

                await Navigation.PushAsync(new PageGestionEtablissementDetail(etablissements.Id, Pseudo));
            }
            catch (Exception)
            {
                throw;
            }
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

        #endregion
    }
}