﻿using Projet_Xamarin_V1.Models;
using Projet_Xamarin_V1.Pages_Recette;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Projet_Xamarin_V1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Accueil : TabbedPage
    {
        public Accueil(string pseudo)
        {
            InitializeComponent();
            //Rempli le profil en fonction du pseudo de connexion
            remplirProfil(pseudo);
            remplirLvEtablissement();
            remplirlvRecette();
            remplirPickerVille(); //Pour le filtrage sur les établissements
            remplirPickerCaract(); //Pour le filtrage sur les caractéristiques
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


        private async void btnModifierProfil_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageModificationProfil(ePseudoProfil.Text));
        }

        private async void btnSupprimerProfil_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Question?", "Voulez vous vraiment supprimer votre compte ?", "Oui", "Non");
            if (answer == true)
            {
                //Connexion bd
                string dpPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3"); //Call Database  
                var db = new SQLiteConnection(dpPath);

                //Récupération de l'id utilisateur
                var dataUser = db.Table<Utilisateurs>(); //Call Table
                var user = dataUser.Where(x => x.Pseudo == ePseudoProfil.Text).FirstOrDefault(); //Linq Query

                //Suppression des relations
                SupprimerAvisRecettesUser(user.Id);
                SupprimerAvisEtablissementsUser(user.Id);
                SupprimerEtablissementsUser(user.Id);
                SupprimerRecettesUser(user.Id);


                //Suppression de l'utilisateur
                bool rep = await App.UtilisateursRepository.SupprimerUtilisateurAsync(ePseudoProfil.Text);
                if(rep == true)
                {
                    await DisplayAlert("Suppression", "La suppression à été effectuée avec succès : redirection vers la page de connexion.", "OK");
                    await Navigation.PushAsync(new MainPage());
                }
                else
                {
                    await DisplayAlert("Suppression", "La suppression à connu une erreur.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Suppression", "Suppression annulée", "OK");
            }
        }

        private async void SupprimerAvisRecettesUser(int id)
        {
            //Suppression des avis recettes
            List<AvisRecette> liAvisRecettesUser = await App.AvisRecetteRepository.RecupererAllAvisRecetteUser(id);

            foreach (var avis in liAvisRecettesUser)
            {
                await App.AvisRecetteRepository.SupprimerAvisRecettesUserAsync(avis.Id);
            }
        }

        private async void SupprimerAvisEtablissementsUser(int id)
        {
            //Suppression des avis recettes
            List<AvisEtablissement> liAvisEtablissements = await App.AvisEtablissementRepository.RecupererAllAvisEtablissements(id);

            foreach (var avis in liAvisEtablissements)
            {
                await App.AvisEtablissementRepository.SupprimerAvisEtablissementsUserAsync(avis.Id);
            }
        }

        private async void SupprimerEtablissementsUser(int id)
        {
            //Suppression des avis recettes
            List<Etablissements> liEtablissements = await App.EtablissementsRepository.RecupererAllEtablissementsUserID(id);

            foreach (var etab in liEtablissements)
            {
                await App.EtablissementsRepository.SupprimerEtablissementUserAsync(etab.Id);
            }
        }

        private async void SupprimerRecettesUser(int id)
        {
            //Suppression des avis recettes
            List<Recettes> liRecettes = await App.RecettesRepository.RecupererAllRecettesUserID(id);

            foreach (var recette in liRecettes)
            {
                await App.EtablissementsRepository.SupprimerEtablissementUserAsync(recette.Id);
            }
        }

        private async void AjoutEtablissement_Clicked(object sender, EventArgs e)
        {
            string pseudo = ePseudoProfil.Text;
            await Navigation.PushAsync(new PageAjoutEtablissement(pseudo));
        }

        private async void btnDeconnexion_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Déconnexion", "Voulez vous vraiment vous déconnecter ?", "Oui", "Non");
            if (answer == true)
            {
                await Navigation.PushAsync(new MainPage());
            }
        }

        private async void btnGererEtablissementsProfil_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageGererEtablissementsUser(ePseudoProfil.Text));
        }
        
        private async void remplirLvEtablissement()
        {
            List<Etablissements> etablissements = await App.EtablissementsRepository.RecupererAllEtablissements();
            lvEtablissement.ItemsSource = etablissements;
        }

        private void lvEtablissement_Refreshing(object sender, EventArgs e)
        {
            remplirLvEtablissement();
            lvEtablissement.EndRefresh();
        }

        private async void lvEtablissement_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                Etablissements etablissements = (Etablissements)lvEtablissement.SelectedItem;

                if (etablissements == null)
                    return;

                await Navigation.PushAsync(new PageEtablissementDetail(etablissements.Id, ePseudoProfil.Text));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async void remplirlvRecette()
        {
            List<Recettes> recettes = await App.RecettesRepository.RecupererAllRecettes();
            lvRecette.ItemsSource = recettes;

        }

        private void lvRecette_Refreshing(object sender, EventArgs e)
        {
            remplirlvRecette();
            lvRecette.EndRefresh();
        }

        private async void lvRecette_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                Recettes recettes = (Recettes)lvRecette.SelectedItem;
                if (recettes == null)
                    return;
                await Navigation.PushAsync(new PageRecetteDetail(recettes.Id, ePseudoProfil.Text));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async void btnAjoutRecette_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageAjouterRecette(ePseudoProfil.Text));
        }

        private async void btnGererRecettesProfil_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageGererRecetteUser(ePseudoProfil.Text));
        }

        private void btnGererAvisRecettes_Clicked(object sender, EventArgs e)
        {

        }

        private void btnGererAvisEtablissements_Clicked(object sender, EventArgs e)
        {

        }

        #region Filtre ville sur les établissements
        public async void remplirPickerVille()
        {
            List<Etablissements> etablissements = await App.EtablissementsRepository.RecupererAllEtablissements();

            pVille.Items.Clear();

            foreach (var etabl in etablissements)
            {
                if (!pVille.Items.Contains(etabl.Ville))
                {
                    pVille.Items.Add(etabl.Ville);
                }
            }
        }
        private async void btnFiltrerVille_Clicked(object sender, EventArgs e)
        {
            if (pVille.SelectedItem != null)
            {
                List<Etablissements> etablissements = await App.EtablissementsRepository.RecupererAllEtablissementsVille(pVille.SelectedItem.ToString());
                lvEtablissement.ItemsSource = etablissements;
            }
            else
            {
                await DisplayAlert("Filtre", "Aucune données de filtrage", "Ok");
            }
        }


        private void btnAnnulerFiltrerVille_Clicked(object sender, EventArgs e)
        {
            pVille.SelectedItem = null;
            remplirLvEtablissement();
        }

        #endregion

        #region Filtre caractéristique sur les recettes
        private async void btnFiltrerCaracteristiques_Clicked(object sender, EventArgs e)
        {
            if (pCaracteristiques.SelectedItem != null)
            {
                List<Recettes> recettes = await App.RecettesRepository.RecupererAllRecettesCaract(pCaracteristiques.SelectedItem.ToString());
                lvRecette.ItemsSource = recettes;
            }
            else
            {
                await DisplayAlert("Filtre", "Aucune données de filtrage", "Ok");
            }
        }

        private void btnAnnulerFiltreCaract_Clicked(object sender, EventArgs e)
        {
            pCaracteristiques.SelectedItem = null;
            remplirlvRecette();
        }

        public async void remplirPickerCaract()
        {
            List<Caracteristiques> caracteristiques = await App.CaracteristiquesRepository.RecupererAllCaracteristiques();

            pCaracteristiques.Items.Clear();

            foreach (var carac in caracteristiques)
            {
                pCaracteristiques.Items.Add(carac.Nom);
            }
        }
        #endregion
    }
}