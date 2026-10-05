namespace CalculateurAge.ViewModels;

// Relie le paramètre "message" de la navigation à la propriété Message.
[QueryProperty(nameof(Message), "message")]
public class ResultatViewModel : BaseViewModel
{
    private string _message = "";// Texte reçu de la page précédente

    // Avec SetField, l'écran se met à jour tout seul : plus besoin de OnAppearing.
    public string Message
    {
        get => _message;
        set => SetField(ref _message, value);
    }

    public RelayCommand RetourCommand { get; }    // Bouton "Retour"

    public ResultatViewModel()
    {
        // ".." revenir à la page précédente.
        RetourCommand = new RelayCommand(async () => await Shell.Current.GoToAsync(".."));
    }
}
