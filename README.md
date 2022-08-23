# EletricityFunction


Tak fordi du har interesse i min ELOverbliks applikation. Denne version er udarbejdet over længere tid, og indeholder både ny og gammel kode.
Jeg tager meget gerne i mod ris/ros til at gøre den endnu mere robust og lækker.

Applikationen er lavet ud fra at inddrage så mange Azure teknologier som muligt. Det kunne helt sikkert godt laves på en mere elegant og smart måde!
Jeg har valgt at gøre brug af:

- Azure Function (C#)
- Azure SQL Database
- Azure Data Factory
- Azure Blob Storage
- Microsoft PowerBI

Kontakt mig endelig på ten@energinet.dk for spørgsmål, ideer eller hvis der mangler noget:) 

# LOCAL.SETTINGS.JSON
Strukturen på local.settings.json og den struktur som bliver brugt i Azure Function er som vist herunder:

Azure function App Settings skrives således: SQLSettings:SQLUser osv..


 "SQLSettings": 
 { "SQLUser":  "xxxxxxxxxxx",
 "SQLPassword": "xxxxxxxxxxx",
    "SQLServer": "xxxxxxxxxxx",
    "SQLDatabase" : "xxxxxxxxxxx"
  },
  "BlobStorageSettings": {
    "StorageKey": "xxxxxxxxxxx",
    "StorageName": "xxxxxxxxxxx"
  },
  "ELOverblikSettings": {
    "MeteringKey": "xxxxxxxxxxx",
    "MeteringToken": "xxxxxxxxxxx"
  }
