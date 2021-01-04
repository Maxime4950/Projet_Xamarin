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
    public partial class PageGererRecetteUser : ContentPage
    {

        string Pseudo = "";
        public PageGererRecetteUser(string pseudo)
        {
            InitializeComponent();
            Pseudo = pseudo;
            remplirLvRecetteUser();
        }

        private async void remplirLvRecetteUser()
        {
            List<Recettes> recettes = await App.RecettesRepository.RecupererAllRecettesUser(Pseudo);
            lvRecette.ItemsSource = recettes;

            
        }

        private async void btnDeconnexion_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Déconnexion", "Voulez vous vraiment vous déconnecter ?", "Oui", "Non");
            if (answer == true)
            {
                await Navigation.PushAsync(new MainPage());
            }
        }

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

        private async void btnRetourAcceuil_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Accueil(Pseudo));
        }
    }
}