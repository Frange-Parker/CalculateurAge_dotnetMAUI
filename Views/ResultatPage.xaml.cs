using CalculateurAge.ViewModels;

namespace CalculateurAge.Views;

public partial class ResultatPage : ContentPage
{
   public ResultatPage()
    {
        InitializeComponent();

        // Relie la page à son ViewModel : les {Binding} du XAML vont chercher leurs valeurs ici.
        // Shell remplira ensuite Message grâce à [QueryProperty] sur le ViewModel.
        BindingContext = new ResultatViewModel();
    }
}