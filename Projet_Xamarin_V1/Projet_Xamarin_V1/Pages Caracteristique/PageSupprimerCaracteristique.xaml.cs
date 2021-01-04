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
    public partial class PageSupprimerCaracteristique : ContentPage
    {
        public PageSupprimerCaracteristique()
        {
            InitializeComponent();
            remplirPickerCaracteristiques();
        }

        private void pCaracteristiques_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pCaracteristiques.Items.Count != 0)
            {
                eNomCaracteristique.Text = pCaracteristiques.SelectedItem.ToString();
            }
        }

        private async void btnConfirmerSuppression_Clicked(object sender, EventArgs e)
        {
            if (pCaracteristiques.SelectedItem.ToString() != "" && eNomCaracteristique.Text != "")
            {
                bool answer = await DisplayAlert("Suppression", "Voulez vous vraiment supprimer la caractéristique " + eNomCaracteristique.Text + "?", "Oui", "Non");
                if (answer == true)
                {
                    await App.CaracteristiquesRepository.SupprimerCaracteristiqueAsync(eNomCaracteristique.Text);
                    await DisplayAlert("Suppression", "Suppression de la caractéristique : " + eNomCaracteristique.Text + " effectuée avec succès!", "Ok");
                    pCaracteristiques.Items.Clear();
                    eNomCaracteristique.Text = "";
                    remplirPickerCaracteristiques();
                }
            }
        }

        private async void remplirPickerCaracteristiques()
        {
            List<Caracteristiques> caracteristiques = await App.CaracteristiquesRepository.RecupererAllCaracteristiques();

            pCaracteristiques.Items.Clear();

            foreach (var car in caracteristiques)
            {
                pCaracteristiques.Items.Add(car.Nom);
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