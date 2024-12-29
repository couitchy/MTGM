# Magic The Gathering Manager

Magic The Gathering Manager (MTGM) est un logiciel conçu pour aider les passionnés de Magic: The Gathering à gérer efficacement leurs cartes, tant pour leur collection que pour la création et la gestion de decks.
Cet outil est le seul qui gère simultanément les cartes en anglais et en français.

## Fonctionnalités

- **Gestion de collection** :
  - Saisie et suivi des cartes dans la collection.
  - Tri des cartes par divers critères (édition, couleur, rareté, etc.).
  - Estimation de la valeur des cartes.
  - Export au format classeur Excel ou page Web.
  - Gestion optimale de panier d'achat pour Cardmarket (nécessite un token de la part de Cardmarket)

- **Gestion de decks** :
  - Saisie et suivi des cartes dans les decks.
  - Suivi des victoires et défaites pour chaque deck.
  - Vérification de la compatibilité des decks avec les autorisations spécifiques de tournoi.
  - Simulation de tirage de mains et de déploiement.
  - Plateau de jeu pour tester les decks (sans gestion des règles).
  - Impression de proxys (Word) pour les cartes en attente d'être achetées.

## Avertissement
L'idée de ce projet est née il y a une vingtaine d'années, alors que je n'étais encore qu'un jeune étudiant ingénieur en électronique et en informatique.
MTGM a été mon premier projet d'envergure, il est donc autant le reflet de la passion qui m'habitait à l'époque que celui de mon inexpérience en matière de développement.
Une grande partie du code a par exemple été développée avant même qu'il soit versionné dans un dépôt.
Le jugement que porte mon œil expert d'aujourd'hui est sans appel :
- la qualité du code est déplorable et ne suit quasiment aucune des bonnes pratiques recommandées.
- l'architecture et les technologies qui portent ce logiciel sont complètement dépassées et obsolètes.

La maintenabilité d'un tel projet est donc fragile, mais il se trouve que le parti pris de l'époque d'avoir choisi des frameworks exclusivement Microsoft fait que la compatibilité du logiciel traverse le temps et les versions de Windows !

Par ailleurs, il se trouve que depuis toutes ces années, Magic: The Gathering n'est pas un jeu figé : de nouvelles éditions de cartes sortent régulièrement, accompagnées de leur lot de nouvelles mécaniques. Le temps que demande ces adaptations fréquentes remplit à 100% ce que mes activités de vie permettent de consacrer à ce projet. C'est pourquoi j'ai depuis longtemps renoncé à ré-écrire l'intégralité du code de façon plus propre et plus moderne, ce que ce projet mériterait pourtant.

Jusqu'à maintenant, le code de MTGM était sous licence propriétaire. À la fin de l'année 2024, j'ai pris la décision de le passer sous licence MIT et de le diffuser ici sur GitHub, dans l'espoir que le projet trouve un peu de sang neuf.

## Prérequis pour la compilation

Pour compiler le logiciel à partir des sources, les outils et composants suivants sont nécessaires :

- **Microsoft .NET Framework 3.5 et ≥4.0** (3.5 pour l'exécution du logiciel, 4.0 ou supérieur pour l'exécution de l'environement de développement)
- **Microsoft .NET SDK 3.5**
- **Microsoft Access database engine 2010** (version 14.0.7015.1000)
- **Microsoft Office 2003 Web Components** (version 12.0.4518.1014)
- **SharpDevelop 4.4.1** (build 9729)
- **ChartFX Lite 6.0.1551** (version 24517)
- **Inno Setup 5.6.1** (pour packager un setup permettant le déploiement chez les utilisateurs)

## Installation

1. Téléchargez les dépendances mentionnées ci-dessus depuis la pre-release [Prérequis](https://github.com/couitchy/MTGM/releases/tag/prerequisites) et installez-les sur votre machine.

**Attention :** le code source automatiquement packagé avec la pre-release n'est pas à jour, ne l'utilisez pas

2. Clonez plutôt le dépôt :
   ```bash
   git clone https://github.com/couitchy/MTGM.git
   ```
3. Ouvrez le projet dans SharpDevelop.
4. Compilez le projet et exécutez le logiciel.

## Contributions

Les contributions sont les bienvenues !
Si vous souhaitez contribuer :

1. Forkez le dépôt.
2. Créez une branche pour une nouvelle fonctionnalité ou une correction de bug :
   ```bash
   git checkout -b nouveau-travail
   ```
3. Soumettez une pull request lorsque votre travail est prêt.

## Licence

Ce projet est sous licence MIT. Vous êtes libre d'utiliser, modifier et distribuer ce logiciel sous réserve de conserver le copyright original.

Une copie de la licence est incluse dans le fichier `Licence.txt` du projet.

Tous les textes et les visuels des cartes, les logos des éditions et des symboles Magic: the Gathering © sont des marques déposées et la propriété de Wizards of the Coast (WoC), Hasbro, LLC. Magic the Gathering Manager n'est ni approuvé, ni supporté, ni affilié d'aucune manière que ce soit à WoC.
