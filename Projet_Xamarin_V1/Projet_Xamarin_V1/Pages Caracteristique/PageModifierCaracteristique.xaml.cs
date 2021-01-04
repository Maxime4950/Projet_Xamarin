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
    public partial class PageModifierCaracteristique : ContentPage
    {
        public PageModifierCaracteristique()
        {
            InitializeComponent();
            remplirPickerCaracteristiques();
        }

        private async void btnConfirmerModification_Clicked(object sender, EventArgs e)
        {
            if (pCaracteristiques.SelectedItem.ToString() != "" && eNomCaracteristique.Text != "")
            {
                if (pCaracteristiques.SelectedItem.ToString() != eNomCaracteristique.Text)
                {
                    if (CaracteristiqueExiste() == false)
                    {
                        await App.CaracteristiquesRepository.ModifierCaracteristiquesAsync(pCaracteristiques.SelectedItem.ToString(), eNomCaracteristique.Text);
                        await DisplayAlert("Modification", "Modification de la caractéristique : " + pCaracteristiques.SelectedItem.ToString() + " en : " + eNomCaracteristique.Text + " effectuée avec succès!", "Ok");
                        eNomCaracteristique.Text = "";
                        remplirPickerCaracteristiques();
                    }
                    else
                    {
                        await DisplayAlert("Modification", "Cette caractéristique est déjà dans la base de données", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Modification", "Le nouveau nom est identique au nom actuel !", "Ok");
                }
            }
            else
            {
                await DisplayAlert("Modification", "Modification échouée car champ(s) vide(s) !", "Ok");
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

        private bool CaracteristiqueExiste()
        {
            string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<Caracteristiques>(); //Call Table
            var CaractExiste = data.Where(x => x.Nom == eNomCaracteristique.Text).FirstOrDefault(); //Linq Query  

            if (CaractExiste != null)
            {
                return true;
            }
            return false;
        }
    }
}