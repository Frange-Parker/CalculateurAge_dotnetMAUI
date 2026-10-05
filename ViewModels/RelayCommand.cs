using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace CalculateurAge.ViewModels;

//Transforme un eméthode en objet liable à un bouton
public class RelayCommand : ICommand
{
    private readonly Action _executer;            // QUOI faire quand on clique
    private readonly Func<bool>? _peutExecuter;   // SI c'est possible

    // Constructeur
    public RelayCommand(Action executer, Func<bool>? peutExecuter = null)
    {
        _executer = executer; 
        _peutExecuter = peutExecuter;
    }

    // Le bouton appelle cette methode et se grise si elle renvoie false.
    public bool CanExecute(object? parametre)
        => _peutExecuter?.Invoke() ?? true;

    //Exécute l'action au clic
    public void Execute(object? parametre) => _executer();

    // Événement auquel le bouton s'abonne pour savoir quand re-poser la question "CanExecute ?".
    public event EventHandler? CanExecuteChanged;

    // À appeler quand l'état change (ex : le nom a été modifié) pour forcer le bouton a reposer la question
    public void Rafraichir()
        => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
