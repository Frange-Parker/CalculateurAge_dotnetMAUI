using System.Collections.ObjectModel;

namespace CalculateurAge.ViewModels;

public class CalculateurViewModel : BaseViewModel
{
    // CHAMPS PRIVÉS
    private string _nom = "";                                       // Nom saisi
    private DateTime? _dateNaissance = DateTime.Today.AddYears(-20); // Date choisie (nullable, car DatePicker.Date l'est)
    private string _resultat = "";                                  // Phrase "Nom, vous avez X ans"
    private string _statut = "";                                    // "Majeur" ou "Mineur"
    private string _prochainAnniversaire = "";                      // Jours restants avant l'anniversaire
    private string _messageErreur = "";                             // Message de validation
    private bool _resultatVisible;                                  // Faut-il afficher la zone de résultat ?



    // PROPRIÉTÉS PUBLIQUES : ce que le XAML voit via {Binding}

    // Nom saisi. À chaque frappe, on revalide et on met à jour l'état du bouton
    public string Nom
    {
        get => _nom;
        set
        {
            if (SetField(ref _nom, value)) // Si la valeur a changé (et que la vue est prévenue)...
                Valider();                 // ...on revalide le formulaire
        }
    }

    // Date de naissance choisie    
    public DateTime? DateNaissance
    {
        get => _dateNaissance;
        set
        {
            if (SetField(ref _dateNaissance, value))
                Valider();
        }
    }

    // Phrase de résultat affichée à l'écran.
    public string Resultat
    {
        get => _resultat;
        set => SetField(ref _resultat, value);
    }

    // FONCTIONNALITÉ 1 : "Majeur" ou "Mineur".
    public string Statut
    {
        get => _statut;
        set => SetField(ref _statut, value);
    }

    // FONCTIONNALITÉ 2 : nombre de jours avant le prochain anniversaire.
    public string ProchainAnniversaire
    {
        get => _prochainAnniversaire;
        set => SetField(ref _prochainAnniversaire, value);
    }

    // Message d'erreur de validation (vide s'il n'y a pas d'erreur).
    public string MessageErreur
    {
        get => _messageErreur;
        set
        {
            // Quand le message change, on prévient aussi ErreurVisible, qui en dépend.
            if (SetField(ref _messageErreur, value))
                OnPropertyChanged(nameof(ErreurVisible));
        }
    }

    // Propriété calculée : vrai s'il y a un message d'erreur à montrer.
    public bool ErreurVisible => !string.IsNullOrEmpty(MessageErreur);

    // Indique si la zone de résultat doit être visible.
    public bool ResultatVisible
    {
        get => _resultatVisible;
        set => SetField(ref _resultatVisible, value);
    }

    // FONCTIONNALITÉ 3 : historique des calculs.
    // ObservableCollection prévient la vue à chaque Insert/Remove.
    public ObservableCollection<string> Historique { get; } = new();



    // COMMANDES : les actions liées aux boutons
    public RelayCommand CalculerCommand { get; }// Bouton "Calculer"
    public RelayCommand EffacerCommand { get; }// FONCTIONNALITÉ 4 : bouton "Effacer"
    public RelayCommand AfficherDetailsCommand { get; }// FONCTIONNALITÉ 5 : ouvre la page de résultat


    // Constructeur : on associe chaque commande à sa méthode.
    public CalculateurViewModel()
    {
        // Calculer : possible seulement si le formulaire est valide
        CalculerCommand = new RelayCommand(Calculer, () => EstValide);

        // Effacer : toujours possible.
        EffacerCommand = new RelayCommand(Effacer);

        // Détails : le "async () =>" permet d'attendre la navigation sans bloquer l'écran
        AfficherDetailsCommand = new RelayCommand(async () => await AfficherDetails());
    }




    // VALIDATION

    // Le formulaire est valide si le nom n'est pas vide, la date existe et n'est pas dans le futur
    private bool EstValide =>
        !string.IsNullOrWhiteSpace(Nom)
        && DateNaissance is not null
        && DateNaissance.Value.Date <= DateTime.Today;

    // Met à jour le message d'erreur et l'état du bouton Calculer.
    private void Valider()
    {
        // Cas d'erreur à signaler : date de naissance dans le futur (refusée).
        if (DateNaissance is not null && DateNaissance.Value.Date > DateTime.Today)
        {
            MessageErreur = "La date de naissance ne peut pas être dans le futur.";
        }
        else
        {
            MessageErreur = ""; // Sinon, aucun message
        }

        CalculerCommand.Rafraichir();// Le bouton se grise ou s'active selon EstValide
    }



    // LOGIQUE MÉTIER

    // Calcule l'âge et met à jour toutes les propriétés liées à l'affichage.
    private void Calculer()
    {
        DateTime naissance = (DateNaissance ?? DateTime.Today).Date;  // Date de naissance
        DateTime aujourdhui = DateTime.Today;                         // Date du jour

        int age = aujourdhui.Year - naissance.Year;

        // Si l'anniversaire de cette année n'est pas encore passé, on retire un an.
        if (naissance.AddYears(age) > aujourdhui) age--;

        // Phrase principale : "an" au singulier pour 0 ou 1, "ans" sinon
        Resultat = $"{Nom}, vous avez {age} {(age <= 1 ? "an" : "ans")}";

        // Message adapté à la tranche d'âge
        Statut = ObtenirStatut(age);

        // Prochain anniversaire = date de naissance + (âge + 1) ans.
        DateTime prochain = naissance.AddYears(age + 1);
        int jours = (prochain - aujourdhui).Days; // Nombre de jours restants

        // Si c'est aujourd'hui l'anniversaire, message spécial.
        ProchainAnniversaire = naissance.AddYears(age) == aujourdhui
            ? "\nEt c'est votre jour: \n🌼 Joyeux anniversaire ! 🌼\n"
            : $"\nProchain anniversaire dans {jours} jour(s)";

        // Ajoute le calcul en tête de l'historique (index 0 = le plus récent en haut).
        Historique.Insert(0, $"\n{Nom} : {age} ans (né(e) le {naissance:dd/MM/yyyy})");

        ResultatVisible = true; // Affiche la zone de résultat
    }

    // Remet tous les champs à zéro.
    private void Effacer()
    {
        Nom = "";
        DateNaissance = DateTime.Today.AddYears(-20);     // Date par défaut
        Resultat = "";
        Statut = "";
        ProchainAnniversaire = "";
        ResultatVisible = false;
    }

    // Navigue vers ResultatPage en lui passant le résultat.
    private async Task AfficherDetails()
    {
        DateTime naissance = (DateNaissance ?? DateTime.Today).Date;  // Date de naissance

        // Texte complet à afficher sur la page suivante
        string message = $"\n\n{Resultat}\nVotre date de naissance est le {naissance:dd/MM/yyyy}\n{Statut}\n{ProchainAnniversaire}";

        // "ResultatPage" = nom de la route enregistrée dans AppShell.xaml.cs.
        // Le dictionnaire transmet le paramètre "message" (sans l'écrire dans l'URL)
        await Shell.Current.GoToAsync("ResultatPage", new Dictionary<string, object>
        {
            { "message", message }
        });
    }

    // Renvoie un message selon la tranche d'âge.
    // "switch" avec "<" : on teste les cas dans l'ordre, le premier qui correspond est utilisé.
    private static string ObtenirStatut(int age) => age switch
    {
        < 3 => "C'est le tout début de l'aventure : vous êtes dans la petite enfance !",   // 0 à 2 ans
        < 13 => "Vous êtes en pleine enfance : tout est à découvrir !",                     // 3 à 12 ans
        < 18 => "Vous êtes à l'adolescence : une période de grands changements !",          // 13 à 17 ans
        < 25 => "Vous avez atteint la majorité : tout un monde s'ouvre à vous !",           // 18 à 24 ans
        < 40 => "Vous êtes en pleine force de l'âge : les projets s'enchaînent !",          // 25 à 39 ans
        < 60 => "Vous avez de l'expérience à revendre !",                                   // 40 à 59 ans
        < 80 => "Vous avez traversé bien des chapitres de vie : quelle sagesse !",          // 60 à 79 ans
        _ => "Vous êtes une véritable légende vivante !"                                 // 80 ans et plus
    };
}

