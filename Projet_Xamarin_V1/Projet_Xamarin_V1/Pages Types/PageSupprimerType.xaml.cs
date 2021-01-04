using Projet_Xamarin_V1.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Projet_Xamarin_V1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageSupprimerType : ContentPage
    {
        public PageSupprimerType()
        {
            InitializeComponent();
            remplirPickerTypes();
        }

        private void pTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pTypes.Items.Count != 0)
            {
                eNomType.Text = pTypes.SelectedItem.ToString();
            }
        }

        private async void btnConfirmerSuppression_Clicked(object sender, EventArgs e)
        {
            if (pTypes.SelectedItem.ToString() != "" && eNomType.Text != "")
            {
                bool answer = await DisplayAlert("Suppression", "Voulez vous vraiment supprimer le type " + eNomType.Text + "?", "Oui", "Non");
                if (answer == true)
                {
                    await App.TypesRepository.SupprimerTypeAsync(eNomType.Text);
                    await DisplayAlert("Suppression", "Suppression du type : " + eNomType.Text + " effectuée avec succès!", "Ok");
                    pTypes.Items.Clear();
                    eNomType.Text = "";
                    remplirPickerTypes();
                }
            }
        }

        private async void remplirPickerTypes()
        {
            List<TypeRecette> types = await App.TypesRepository.RecupererAllTypes();
            pTypes.Items.Clear();
            foreach (var car in types)
            {
                pTypes.Items.Add(car.Nom);
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