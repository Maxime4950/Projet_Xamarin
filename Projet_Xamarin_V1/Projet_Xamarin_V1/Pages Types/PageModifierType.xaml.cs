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
    public partial class PageModifierType : ContentPage
    {
        public PageModifierType()
        {
            InitializeComponent();
            remplirPickerTypes();
        }

        private async void btnConfirmerModification_Clicked(object sender, EventArgs e)
        {
            if (pTypes.SelectedItem.ToString() != "" && eNomType.Text != "")
            {
                if(pTypes.SelectedItem.ToString() != eNomType.Text)
                {
                    if(typeExiste() == false)
                    {
                        await App.TypesRepository.ModifieTypeAsync(pTypes.SelectedItem.ToString(), eNomType.Text);
                        await DisplayAlert("Modification", "Modification du type : " + pTypes.SelectedItem.ToString() + " en : " + eNomType.Text + " effectuée avec succès!", "Ok");
                        eNomType.Text = "";
                        remplirPickerTypes();
                    }
                    else
                    {
                        await DisplayAlert("Modification","Ce type est déjà dans la base de données","Ok");
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

        private bool typeExiste()
        {
            string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<TypeRecette>(); //Call Table
            var typeExiste = data.Where(x => x.Nom == eNomType.Text).FirstOrDefault(); //Linq Query  

            if (typeExiste != null)
            {
                return true;
            }
            return false;
        }
    }
}