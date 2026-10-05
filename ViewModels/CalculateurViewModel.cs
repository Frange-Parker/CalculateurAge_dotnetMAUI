using System;
using System.Collections.Generic;
using System.Text;

namespace CalculateurAge.ViewModels;

// Contient l'ÉTAT de l'écran et les ACTIONS possibles
public class CalculateurViewModel : BaseViewModel
{
    // Champs privés : la vraie donnée.
    private string _nom = "";                                         // Nom saisi
    private DateTime? _dateNaissance = DateTime.Today.AddYears(-20);  // Date choisie (nullable, comme DatePicker.Date)
    private string _resultat = "";                                    // Phrase de résultat
    private bool _resultatVisible;                                    // Zone de résultat visible ?

    // Propriétés publiques : ce que le XAML voit.
    public string Nom
    {
        get => _nom;
        set { if (SetField(ref _nom, value))      // Si le nom a changé...
                  CalculerCommand.Rafraichir(); } // ...le bouton se met à jour (grisé ou actif)
    }

    public DateTime? DateNaissance
    {
        get => _dateNaissance;
        set => SetField(ref _dateNaissance, value);
    }

    public string Resultat
    {
        get => _resultat;
        set => SetField(ref _resultat, value);
    }

    public bool ResultatVisible
    {
        get => _resultatVisible;
        set => SetField(ref _resultatVisible, value);
    }

    // Liée à Button.Command dans le XAML.
    public RelayCommand CalculerCommand { get; }

    public CalculateurViewModel()
    {
        // Calculer n'est possible que si le nom n'est pas vide.
        CalculerCommand = new RelayCommand(Calculer, () => !string.IsNullOrWhiteSpace(Nom));
    }

    // La logique métier : aucun contrôle d'interface ici.
    private void Calculer()
    {
        DateTime naissance = (DateNaissance ?? DateTime.Today).Date;   // Date de naissance
        int age = DateTime.Today.Year - naissance.Year;                // Âge brut

        if (naissance > DateTime.Today.AddYears(-age)) age--;          // Anniversaire pas encore passé : -1 an

        Resultat = $"{Nom}, vous avez {age} ans";                      // Met à jour le texte
        ResultatVisible = true;                                        // Affiche la zone de résultat
    }
}