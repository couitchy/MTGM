# Magic The Gathering Manager

Magic The Gathering Manager est un logiciel conçu pour aider les passionnés de Magic: The Gathering à gérer efficacement leurs cartes, tant pour leur collection que pour la création et la gestion de decks.
Cet outil est le seul qui gère simultanément les cartes en anglais et en français.

## Fonctionnalités

- **Gestion de collection** :
  - Saisie et suivi des cartes dans la collection.
  - Tri des cartes par divers critères (édition, couleur, rareté, etc.).
  - Estimation de la valeur des cartes et de la collection.
  - Export Excel ou Web.
  - Gestion optimale de panier d'achat pour Cardmarket (nécessite un token de la part de Cardmarket)

- **Gestion de decks** :
  - Saisie et suivi des cartes dans les decks.
  - Suivi des victoires et défaites pour chaque deck.
  - Vérification de la compatibilité des decks avec les autorisations spécifiques de tournoi.
  - Simulation de tirage de mains et de déploiement
  - Plateau de jeu pour tester les decks (sans gestion des règles)
  - Impression de proxys (Word) pour les cartes en attente d'achat.

## Prérequis pour la compilation

Pour compiler le logiciel à partir des sources, les outils et composants suivants sont nécessaires :

- **Microsoft .NET Framework 3.5**
- **Microsoft .NET SDK 3.5**
- **Microsoft Access database engine 2010** (version 14.0.7015.1000)
- **Microsoft Office 2003 Web Components** (version 12.0.4518.1014)
- **SharpDevelop 4.4.1** (build 9729)
- **ChartFX Lite 6.0.1551** (version 24517)
- (Optionnel) **Inno Setup 5.6.1** pour compiler le programme d'installation.

## Installation

1. Téléchargez les dépendances mentionnées ci-dessus et installez-les sur votre machine.
2. Clonez ce dépôt :
   ```bash
   git clone https://github.com/couitchy/MTGM.git
   ```
3. Ouvrez le projet dans SharpDevelop.
4. Compilez le projet et exécutez le logiciel.

## Contributions

Les contributions sont les bienvenues ! Si vous souhaitez contribuer :

1. Forkez le dépôt.
2. Créez une branche pour votre fonctionnalité ou correction de bug :
   ```bash
   git checkout -b nouvelle-fonctionnalite
   ```
3. Soumettez une pull request lorsque votre travail est prêt.

## Licence

Ce projet est sous licence MIT. Vous êtes libre d'utiliser, modifier et distribuer ce logiciel sous réserve de conserver le copyright original.

Une copie de la licence est incluse dans le fichier `Licence.txt` du projet.

Tous les textes et les visuels des cartes, les logos des éditions et des symboles Magic: the Gathering © sont des marques déposées et la propriété de Wizards of the Coast (WoC), Hasbro, LLC. Magic the Gathering Manager n'est ni approuvé, ni supporté, ni affilié d'aucune manière que ce soit à WoC.
