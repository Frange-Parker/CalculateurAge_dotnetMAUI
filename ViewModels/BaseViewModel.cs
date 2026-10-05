using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CalculateurAge.ViewModels;


//classe mère de tous les ViewModels
public class BaseViewModel : INotifyPropertyChanged
{
    //L'évènement: le moteur binding s'y  abonne
    public event PropertyChangedEventHandler? PropertyChanged;

    //Previent la vue qu'une propriété a changé
    // ?. : ne fait rien si personne n'est abonné
    protected void OnPropertyChanged(
        [CallerMemberName] string? nom = null
    ) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nom));


    //Affecte une valeur ET notifie, en une seule ligne
    //Renvoie true si la valeur a réellement changé
    protected bool SetField<T>(ref T champ, T valeur, [CallerMemberName] string? nom = null)
    {

        //Garde fou: eveite les notifications inutiles
        //et les boucles infinies en mode TwoWay
        if (EqualityComparer<T>.Default.Equals(champ, valeur)) return false;
        champ = valeur;
        OnPropertyChanged(nom);
        return true;
    }
}
