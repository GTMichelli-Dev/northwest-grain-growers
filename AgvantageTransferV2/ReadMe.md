## Create Windows Service Installer 
### All the Agvantage scripts are in the [Agvantage Folder](https://github.com/TotalScaleService/NorthwestGrainGrowers/tree/e378a03586d032bb9ff2997cf1d8bc2b284f2b3c/AgvantageTransfer/Agvantage "Git Hub Repo")
 
Build the Windows Service installer using the following steps:
1. This command publishes the application in Release configuration for Windows x64 runtime as a self-contained single file, including native libraries for self-extraction.
```
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```


2. Open commandline as an Administrator 
```cmd
sc create AgvantageTransfer binPath= "C:\Agvantage\Service\AgvantageTransfer.exe" start= auto obj= "NT AUTHORITY\LocalService" password= ""
sc config  AgvantageTransfer start= delayed-auto
sc description AgvantageTransfer "Imports from Agvantage and syncs carriers/producers/crops/seed."
sc config AgvantageTransfer start= delayed-auto
sc failure     AgvantageTransfer reset= 86400 actions= restart/600/restart/600/restart/600
sc failureflag AgvantageTransfer 1
sc qc      AgvantageTransfer
sc start   AgvantageTransfer
```



3. To Remove the Service:
```cmd
sc query "Agvantage Transfer"
sc stop  "AgvantageTransfer"
sc delete "AgvantageTransfer"
```
