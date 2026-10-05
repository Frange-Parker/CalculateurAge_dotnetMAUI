using CalculateurAge.Views;// Pour accéder a ResultatPage


namespace CalculateurAge
{
    public partial class MainPage : ContentPage
    {
        // Constructeur : appelé à la création de la page
        public MainPage()
        {
            InitializeComponent(); // Charge le XAML
        }

        // Gestionnaire appelé au clic du bouton Calculer
        // sender = le contrôle cliqué; e = données de l'événement
        private async void OnCalculerClicked(object sender, EventArgs e)
        {
            // Validation : on refuse un nom vide.
            if (string.IsNullOrWhiteSpace(entryNom.Text))
            {
                DisplayAlert("Erreur", "Entrez un nom", "OK");
                return; // on sort sans rien calculer
            }

            //DateTime d = pickerDate.Date;
            // Date choisie : si le DatePicker est vide (null), on prend la date du jour par défaut
            DateTime d = pickerDate.Date ?? DateTime.Today;
            int age = DateTime.Today.Year - d.Year;

            // Si l'anniversaire n'est pas encore passé cette année,
            // on retire une année.
            if (d.Date > DateTime.Today.AddYears(-age)) age--;

             // On va vers ResultatPage en passant le nom et l'age dans l'URL (apres "?", separes par "&").
            // EscapeDataString protege les caracteres speciaux du nom (espace, &, accents...).
            await Shell.Current.GoToAsync(
                $"{nameof(ResultatPage)}?nom={Uri.EscapeDataString(entryNom.Text)}&age={age}");
        }
    }
}
