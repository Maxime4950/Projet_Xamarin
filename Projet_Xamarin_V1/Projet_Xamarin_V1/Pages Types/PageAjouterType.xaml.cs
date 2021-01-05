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
    public partial class PageAjouterType : ContentPage
    {
        #region INITIALISATION DES VARIABLES
        #endregion

        #region CONSTRUCTEUR pageAjouterType
        public PageAjouterType()
        {
            InitializeComponent();
        }
        #endregion

        #region METHODES

        #region Confirmation de l'ajout du type
        private async void btnConfirmerAjout_Clicked(object sender, EventArgs e)
        {
            if (eNomType.Text != "")
            {
                await DisplayAlert("Ajout type", "Ajout du type : " + eNomType.Text + " reussi.", "Ok");
                await App.TypesRepository.AjoutNouveauTypeAsync(eNomType.Text);
                eNomType.Text = "";
            }
            else
            {
                await DisplayAlert("Ajout type", "Ajout du type échoué car champ vide", "Ok");
            }
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