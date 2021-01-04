using System;
using Projet_Xamarin_V1.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using SQLite;
using Xamarin.Essentials;
using Projet_Xamarin_V1.Repositories;

namespace Projet_Xamarin_V1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageAjouterCaracteristique : ContentPage
    {
        public PageAjouterCaracteristique()
        {
            InitializeComponent();
        }

       

        private async void btnConfirmerAjout_Clicked(object sender, EventArgs e)
        {
            if(eNomCaracteristique.Text != "")
            {
                await DisplayAlert("Ajout caractéristique", "Ajout de la caractéristique : " + eNomCaracteristique.Text + " reussi.", "OK");
                await App.CaracteristiquesRepository.AjoutNouvelleCaracteristiqueAsync(eNomCaracteristique.Text);
                eNomCaracteristique.Text = "";
            }
            else
            {
                await DisplayAlert("Ajout type", "Ajout du type échoué car champ vide", "Ok");
            }
        }

        private async void btnDeconnexion_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Déconnexion", "Voulez vous vraiment vous déconnecter ?", "Oui", "Non");
            if (answer == true)
            {
                await Navigation.PushAsync(new MainPage());
            }
        }
    }
}