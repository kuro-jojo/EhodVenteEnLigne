# EhodVenteEnLigne

Ce projet possède une base de données intégrée qui sera créée lorsque l’application sera exécutée pour la première fois. Pour la créer correctement, vous devez satisfaire aux prérequis ci-dessous et mettre à jour les chaînes de connexion pour qu’elles pointent vers le serveur MSSQL qui est exécuté sur votre PC en local.

## Prérequis : 

### Base de donnée

MSSQL Developer 2019 ou Express 2019 doit être installé avec Microsoft SQL Server Management Studio (SSMS).

MSSQL : https://www.microsoft.com/fr-fr/sql-server/sql-server-downloads

SSMS : https://docs.microsoft.com/fr-fr/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16

*Remarque : les versions antérieures de MSSQL Server devraient fonctionner sans problèmes, mais elles n’ont pas été testées.*

*Dans le projet P3AddNewFunctionalityDotNetCore, ouvrez le fichier appsettings.json.*

Vous avez la section ConnectionStrings qui définit les chaînes de connexion pour les 2 bases de données utilisées dans cette application.

      "ConnectionStrings":
      {
        "P3Referential": "Server=.;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;Trusted_Connection=True;MultipleActiveResultSets=true",
        "P3Identity": "Server=.;Database=Identity;Trusted_Connection=True;MultipleActiveResultSets=true"
      }
  
**P3Referential** - chaîne de connexion à la base de données de l’application.

**P3Identity** - chaîne de connexion à la base de données des utilisateurs. Il s’agit d’une base de données indépendante, car une organisation utilise généralement plusieurs applications différentes. Au lieu
de laisser chaque application définir ses propres utilisateurs (et plusieurs identifiants et mots de passe différents pour un seul utilisateur), une organisation peut disposer d’une base de données utilisateur unique et utiliser des autorisations et des rôles pour définir les applications auxquelles l’utilisateur a accès. De cette façon, un utilisateur peut accéder à toutes les applications de l’organisation avec le même identifiant et le même mot de passe, mais il n’aura accès qu’aux bases de données et aux actions définies dans la base de données des utilisateurs.

Il existe des versions différentes de MSSQL (veuillez utiliser MSSQL pour ce projet et non une autre base de données). Lorsque vous configurez le serveur de base de données, diverses options modifient la configuration de sorte que les chaînes de connexion définies peuvent ne pas fonctionner.

Les chaînes de connexion définies dans le projet sont configurées pour MSSQL Server Standard 2019. L’installation n’a pas créé de nom d’instance, le serveur est donc simplement désigné par « . », qui désigne l’instance par défaut de MSSQL Server fonctionnant sur la machine actuelle. Pendant l’installation, c’est l’utilisateur intégré de Windows qui est configuré dans le serveur MSSQL par défaut.

Si vous avez installé MSSQL Express, la valeur à utiliser pour Server est très probablement .\SQLEXPRESS. Donc votre chaîne de connexion P3Referential serait :

    "P3Referential": "Server=.\SQLEXPRESS;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;Trusted_Connection=True;MultipleActiveResultSets=true"
  
Si vous avez des difficultés à vous connecter, essayez d’abord de vous connecter à l’aide de Microsoft SQL Server Management Studio (assurez-vous que le type d’authentification est « Authentification Windows »), ou consultez le site https://sqlserver-help.com/2011/06/19/help-whats-my-sql-server-name/.
Si le problème persiste, demandez de l’aide à votre mentor.

### Configuration

Pour pouvoir se connecter à la base de donnée, au lancement du projet si aucun administrateur n'est ajouté précédemment, un admin défini par l'utilisateur sera ajouté.
Pour se faire, il faut créer un **User Secret** au sein du projet .NET.

*Si vous n'avez pas encore installé l'outil de gestion des secrets, vous pouvez le faire en exécutant la commande suivante dans votre terminal :*
```bash
dotnet tool install --global dotnet-user-secrets
```

#### Ajout des Secrets Utilisateur
Pour ajouter les secrets utilisateur "Administrator" à votre projet, exécutez les commandes suivantes dans le répertoire racine de votre projet :
```bash
dotnet user-secrets set "Administrator:User" "VotreNomDUtilisateur"
dotnet user-secrets set "Administrator:Password" "VotreMotDePasse" (Min 8 char + char spécial + chiffres)
```
#### Utilisation 
Ces secrets sont utilisés dans [IdentitySeedData.cs](EhodVenteEnLigne/Data/IdentitySeedData.cs)

```csharp
string AdminUser = config["Administrator:User"];
string AdminPassword = config["Administrator:Password"];
```
