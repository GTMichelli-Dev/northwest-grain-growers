# Grain Management

## This is the source code for Grain management and this file is where I am putting setup stuff for the project

# 1 Images stored on the server 

_Currently in the demo I am storing them in vultr Not dure where they will be in the future_ 

[This the setup for the linux server](https://github.com/TotalScaleService/KioskCam/blob/main/Store_Images_On_Linux.md)

## Setting the server to allow access to the images
- Use static files
    Configure the program on the server
    add this to the appsettings.json
   ```json
    {
      "TicketImages": {
        "PhysicalPath": "/var/grainmanagement/ticket-images",
        "RequestPath": "/ticket-images"
      }
    }
   ```
    add this to the program.cs
   ```bash
        using Microsoft.Extensions.FileProviders;
        
        var ticketPath = builder.Configuration["TicketImages:PhysicalPath"];
        var requestPath = builder.Configuration["TicketImages:RequestPath"];
        
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(ticketPath),
            RequestPath = requestPath
        });

   ```

   Redeploy the site [Here is an example for vultr](https://github.com/TotalScaleService/tss.scaledata.net/blob/main/README.md)

   
