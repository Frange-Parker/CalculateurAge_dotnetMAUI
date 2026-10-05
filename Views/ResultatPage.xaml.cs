namespace CalculateurAge.Views;

// Relie les paramètres "nom" et "age" de l'URL de navigation aux propriétés Nom et Age.
[QueryProperty(nameof(Nom), "nom")]
[QueryProperty(nameof(Age), "age")]
public partial class ResultatPage : ContentPage
{
    // Ces proprietes sont remplies par la navigation, APRÈS le constructeur.
    public string Nom { get; set; } = "";
    public string Age { get; set; } = "";

    // Construit l'arbre visuel decrit par le XAML.
    public ResultatPage() => InitializeComponent();

    // Appele a CHAQUE affichage de la page : c'est ici que les parametres sont deja disponibles.
    protected override void OnAppearing()
    {
        base.OnAppearing();                                  // Comportement standard
        lblMessage.Text = $"{Nom}, vous avez {Age} ans";     // Affiche le message
    }

    // ".." = revenir a la page precedente.
    private async void OnRetourClicked(object? s, EventArgs e)
        => await Shell.Current.GoToAsync("..");
}