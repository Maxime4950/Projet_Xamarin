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
    public partial class AccueilAdmin : TabbedPage
    {
        public AccueilAdmin(string pseudo)
        {
            InitializeComponent();
            remplirProfil(pseudo);
        }
        public void remplirProfil(string pseudo)
        {
            ePseudoProfil.Text = pseudo;
            string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<Utilisateurs>(); //Call Table
            var remplissage = data.Where(x => x.Pseudo == pseudo).FirstOrDefault(); //Linq Query 

            eMotDePasseProfil.Text = remplissage.MotDePasse;
            eEmailProfil.Text = remplissage.Mail;
            eDateInscriptionProfil.Text = remplissage.DateInscription.ToShortDateString();
            eDateNaissanceProfil.Text = remplissage.DateNaissance.ToShortDateString();
        }

        private async void btnDeconnexion_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Déconnexion", "Voulez vous vraiment vous déconnecter ?", "Oui", "Non");
            if (answer == true)
            {
                await Navigation.PushAsync(new MainPage());
            }
        }

        private async void btnModifierProfil_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageModificationProfil(ePseudoProfil.Text));
        }

        private async void btnGererType_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageGererTypes());
        }

        private async void btnGererCaracteristiques_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageGererCaracteristiques());
        }
    }
}