# Reconciliation Engine

## Présentation du projet

Le Reconciliation Engine est un outil de rapprochement bancaire développé en C# .NET. Il permet de comparer automatiquement les transactions bancaires avec les écritures comptables pour identifier les correspondances.

L'application lit deux fichiers CSV (bancaire et comptable), applique un algorithme de matching avec scoring, et génère des rapports détaillés des correspondances trouvées.

## Prérequis

- .NET 10.0 SDK
- Système d'exploitation compatible (Windows, macOS, Linux)

## Comment exécuter

### Compilation
```bash
dotnet build
```
### Exécution

```bash
dotnet run <fichier_bancaire.csv> <fichier_comptable.csv>
```

### Fichiers générés

L'application génère les fichiers suivants dans le répertoire d'exécution :

- `matches.csv` : Liste des correspondances trouvées avec leurs scores
- `report.txt` : Rapport détaillé du processus de rapprochement

## Les hypothèses & choix

Les fichiers CSV respectent le format suivant : Date,Description,Amount

Les dates sont au format yyyy-MM-dd

Les erreurs de parsing sont loguées et n’arrêtent pas l’application

Scores et règles sont fixes et ordonnés pour la priorisation

Architecture : séparation Domain, Services, Program.cs pour faciliter la maintenance

## Limites (contexte production)

Formats multiples : Support uniquement du CSV (pas d'Excel, JSON, XML)

Libellés différents entre banque et comptabilité non pris en compte

Les frais bancaires et opérations manuelles complexes ne sont pas analysés

### Améliorations possibles

Gestion des libellés différents entre banque et comptabilité

Gestion des formats multiples (CSV, Excel, JSON, XML)

### À tester en plus

Gros fichiers CSV pour tester performances et mémoire

Cas de doublons et transactions exactes répétées

## Tests

### Tous les tests

```bash
dotnet test
```

### Test particulier

```bash
dotnet test <nom_du_test>
```
