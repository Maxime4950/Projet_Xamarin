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
    public partial class PageGererEtablissementsUser : ContentPage
    {
        string Pseudo = "";
        public PageGererEtablissementsUser(string pseudo)
        {
            InitializeComponent();
            Pseudo = pseudo;
            remplirLvEtablissementsUser();
        }

        private async void remplirLvEtablissementsUser()
        {
            List<Etablissements> etablissements = await App.EtablissementsRepository.RecupererAllEtablissementsUser(Pseudo);
            lvEtablissement.ItemsSource = etablissements;
        }

        private async void btnDeconnexion_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Déconnexion", "Voulez vous vraiment vous déconnecter ?", "Oui", "Non");
            if (answer == true)
            {
                await Navigation.PushAsync(new MainPage());
            }
        }

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

        private async void btnRetourAccueil_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Accueil(Pseudo));
        }
    }
}