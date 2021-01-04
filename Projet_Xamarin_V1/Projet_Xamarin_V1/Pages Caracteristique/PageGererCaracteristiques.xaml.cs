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
    public partial class PageGererCaracteristiques : ContentPage
    {
        public PageGererCaracteristiques()
        {
            InitializeComponent();
            remplirLvCaracteristiques();
        }

        private async void btnAjouterCaracteristiques_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageAjouterCaracteristique());
        }

        private async void btnModifierCaracteristiques_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageModifierCaracteristique());
        }

        private async void btnSupprimerCaracteristiques_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageSupprimerCaracteristique());
        }

        private async void btnDeconnexion_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Déconnexion", "Voulez vous vraiment vous déconnecter ?", "Oui", "Non");
            if (answer == true)
            {
                await Navigation.PushAsync(new MainPage());
            }
        }

        private async void remplirLvCaracteristiques()
        {
            List<Caracteristiques> caracteristiques = await App.CaracteristiquesRepository.RecupererAllCaracteristiques();
            lvCaracteristiques.ItemsSource = caracteristiques;
        }
        private void lvCaracteristiques_Refreshing(object sender, EventArgs e)
        {
            remplirLvCaracteristiques();
            lvCaracteristiques.EndRefresh();
        }
    }
}