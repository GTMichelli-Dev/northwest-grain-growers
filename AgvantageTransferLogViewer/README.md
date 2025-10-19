# Install the transfer log viewer
To publish it on iis
1. install the Hostable Web Core  https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-aspnetcore-8.0.21-windows-hosting-bundle-installer
2. publish the project

   <img width="703" height="557" alt="image" src="https://github.com/user-attachments/assets/d45c7f71-3bbb-4270-a736-697b3a3ddfc8" />
3. Make the web config file like this
```web.config
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <location path="." inheritInChildApplications="false">
    <system.webServer>
      <handlers>
        <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
      </handlers>
      <aspNetCore hostingModel="InProcess"
            processPath="dotnet"
            arguments=".\AgvantageTransferLogViewer.dll"
            stdoutLogEnabled="true"
            stdoutLogFile=".\logs\stdout" />
    </system.webServer>
  </location>
</configuration>
<!--ProjectGuid: 77c217da-71c2-45a0-8671-89551ee5b1c0-->
```
