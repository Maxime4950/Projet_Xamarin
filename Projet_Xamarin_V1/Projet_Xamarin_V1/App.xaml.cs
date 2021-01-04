using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using Projet_Xamarin_V1.Repositories;
using SQLite;
using Projet_Xamarin_V1.Models;

namespace Projet_Xamarin_V1
{
    public partial class App : Application
    {
        //Création du fichier de bd avec SQLITE3
        private string dbPath = Path.Combine(FileSystem.AppDataDirectory, "databaseXamarin.db3");

        public static UtilisateursRepository UtilisateursRepository { get; private set; } //Private set car on ne veut y accéder que sur cette partie du programme
        public static EtablissementsRepository EtablissementsRepository { get; private set; } //Private set car on ne veut y accéder que sur cette partie du programme
        public static RecettesRepository RecettesRepository { get; private set; } //Private set car on ne veut y accéder que sur cette partie du programme
        public static CaracteristiquesRepository CaracteristiquesRepository { get; private set; } //Private set car on ne veut y accéder que sur cette partie du programme
        public static TypesRepository TypesRepository { get; private set; } //Private set car on ne veut y accéder que sur cette partie du programme

        public static AvisEtablissementRepository AvisEtablissementRepository { get; private set; } //Private set car on ne veut y accéder que sur cette partie du programme

        public static AvisRecetteRepository AvisRecetteRepository { get; private set; } //Private set car on ne veut y accéder que sur cette partie du programme

        public App()
        {
            InitializeComponent();
            UtilisateursRepository = new UtilisateursRepository(dbPath);
            EtablissementsRepository = new EtablissementsRepository(dbPath);
            RecettesRepository = new RecettesRepository(dbPath);
            CaracteristiquesRepository = new CaracteristiquesRepository(dbPath);
            TypesRepository = new TypesRepository(dbPath);
            AvisEtablissementRepository = new AvisEtablissementRepository(dbPath);
            AvisRecetteRepository = new AvisRecetteRepository(dbPath);
            MainPage = new NavigationPage(new MainPage());
           
            //Console.WriteLine(dbPath);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
