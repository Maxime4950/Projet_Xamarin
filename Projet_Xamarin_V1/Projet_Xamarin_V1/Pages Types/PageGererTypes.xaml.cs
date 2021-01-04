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
    public partial class PageGererTypes : ContentPage
    {
        public PageGererTypes()
        {
            InitializeComponent();
            remplirLvTypes();
        }

        private async void btnAjouterTypes_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageAjouterType());
        }

        private async void btnModifierTypes_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageModifierType());
        }

        private async void btnSupprimerTypes_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageSupprimerType());
        }

        private async void btnDeconnexion_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Déconnexion", "Voulez vous vraiment vous déconnecter ?", "Oui", "Non");
            if (answer == true)
            {
                await Navigation.PushAsync(new MainPage());
            }
        }

        private async void remplirLvTypes()
        {
            List<TypeRecette> types = await App.TypesRepository.RecupererAllTypes();
            lvTypes.ItemsSource = types;
        }

        private void lvTypes_Refreshing(object sender, EventArgs e)
        {
            remplirLvTypes();
            lvTypes.EndRefresh();
        }
    }
}