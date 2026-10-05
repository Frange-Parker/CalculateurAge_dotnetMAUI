# Calculateur d'âge .NET MAUI

Application mobile qui calcule l'âge à partir d'une date de naissance. Le projet a été réalisé en trois phases (code-behind, navigation, MVVM), puis enrichi de fonctionnalités supplémentaires.

## Fonctionnalités

### Fonctionnalités de base
- Saisie du nom et choix de la date de naissance
- Calcul de l'âge en tenant compte de l'anniversaire pas encore passé cette année
- Bouton Calculer grisé tant que le formulaire n'est pas valide
- Seconde page de résultat, avec bouton Retour

### Fonctionnalités ajoutées (Activité 6)
- Message adapté à la tranche d'âge (petite enfance, adolescence, pleine force de l'âge, etc.)
- Nombre de jours restants avant le prochain anniversaire (message spécial le jour même)
- Historique des calculs, le plus récent en haut
- Bouton Effacer qui remet tous les champs à zéro
- Refus d'une date de naissance dans le futur : message d'erreur et bouton Calculer grisé
- Navigation vers la page de résultat déclenchée depuis le ViewModel

## Phases du projet
- **Phase A** : version code-behind (logique écrite directement dans la page)
- **Phase B** : seconde page `ResultatPage` et navigation avec Shell (routes et paramètres)
- **Phase C** : réécriture en MVVM (bindings, `BaseViewModel`, `RelayCommand`, `CalculateurViewModel`)
- **Activité 6** : fonctionnalités supplémentaires, sans logique dans le code-behind

## Architecture (MVVM)

```
CalculateurAge/
    Views/
        ResultatPage.xaml          <- seconde page (vue)
        ResultatPage.xaml.cs
    ViewModels/
        BaseViewModel.cs           <- notification des changements (INotifyPropertyChanged)
        RelayCommand.cs            <- commandes liées aux boutons
        CalculateurViewModel.cs    <- état et logique de l'écran principal
        ResultatViewModel.cs       <- état de la page de résultat
    MainPage.xaml                  <- écran principal (vue)
    MainPage.xaml.cs
    AppShell.xaml / .cs            <- navigation et routes
    App.xaml / .cs
    MauiProgram.cs
```

Principe : la vue (XAML) affiche, le ViewModel garde les données et fait les calculs, et le binding relie les deux. Les ViewModels ne contiennent aucun contrôle d'interface.

## Layouts utilisés
- ScrollView : permet de faire défiler la page si le contenu dépasse l'écran
- VerticalStackLayout : empile les champs, les boutons et les résultats
- BindableLayout : génère automatiquement une ligne par calcul de l'historique

## Lancer le projet

### Prérequis
- Visual Studio 2026 (ou 2022 à jour) avec la charge de travail **« Développement d'applications mobiles avec .NET (MAUI) »**, ou le SDK .NET 10 en ligne de commande
- Cloner le dépôt, puis ouvrir le projet `CalculateurAge` (fichier `.csproj` ou `.sln`)

```bash
dotnet --version                # Vérifie que le SDK .NET 10 est installé
dotnet workload install maui    # Installe MAUI (seulement si vous n'utilisez pas Visual Studio)
```

### Cas 1 : Émulateur Android
1. Visual Studio : menu **Outils > Android > Gestionnaire d'appareils Android**, créer un appareil virtuel
2. Le sélectionner dans la barre d'outils (liste déroulante à côté du bouton ▶), puis **F5**

Nécessite la virtualisation activée dans le BIOS et Hyper-V (Windows). Si l'émulateur ne démarre pas, passer au cas 2.

### Cas 2 : Téléphone Android réel
1. **Activer le mode développeur** : Paramètres > À propos du téléphone > appuyer 7 fois sur « Numéro de build »
2. **Activer le débogage USB** : Paramètres > Options pour les développeurs > Débogage USB
3. Brancher le téléphone en USB (câble qui transmet les données, pas seulement la charge)
4. Accepter la fenêtre « Autoriser le débogage USB ? » sur le téléphone
5. Vérifier que le téléphone est détecté :

```bash
adb devices    # Doit lister l'appareil avec le statut "device"
```

6. Sélectionner le téléphone dans la liste déroulante de Visual Studio, puis **F5**

### Cas 3 : Installer un APK sur un téléphone (sans PC relié)

```bash
# Depuis le dossier du projet : génère un APK autonome
dotnet publish -f net10.0-android -c Release -p:AndroidPackageFormat=apk
```

Envoyer le fichier `.apk` (dans `bin/Release/net10.0-android/publish/`) sur le téléphone, puis l'ouvrir en autorisant « l'installation d'applications de sources inconnues ».

### Cas 4 : Windows (aucun téléphone ni émulateur)
Sous Windows, choisir la cible **« Machine Windows »** dans Visual Studio, puis **F5**.


##
Réalisé par NOLACK KAWUNJIBI Frange Parker GL5 ENSPD 2026-2027
##
