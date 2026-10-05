using CalculateurAge.ViewModels;

namespace CalculateurAge;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();                       

        // Objet où tous les {Binding} vont chercher leurs valeurs
        BindingContext = new CalculateurViewModel(); 
    }
}