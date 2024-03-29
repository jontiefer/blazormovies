# Blazor Movies Sample Application
This is a sample movie application we can use for our Dev-Ops training.  The application is a Blazor Web-Assembly application hosted by an ASP.Net 5.0 server back-end service.  The application utilizes a PostGres and S3 data store.

# Featured Repos

* [dotnet/sdk](https://hub.docker.com/_/microsoft-dotnet-sdk/): .NET SDK
* [dotnet/aspnet](https://hub.docker.com/_/microsoft-dotnet-aspnet/): ASP.NET Core Runtime
* [postgres](https://hub.docker.com/_/postgres?tab=tags&page=1&ordering=last_updated): PostGres 13.1

# Blazor Movies Application Structure
The BlazorMovies application contains the following project files that are specific to the solution:&nbsp;
* BlazorMovies.sln (The solution of the BlazorMovies web application, which contains the Blazor WebAssembly client project, the ASP.Net 5.0 Server project and the shared .Net 5.0 class libarries)
* BlazorMovies.Server.csproj (The project containing the ASP.Net 5.0 Server dll that will host the Blazor WebAssembly application and contains the API endpoints of the application)
* BlazorMovies.Client.csproj (The project that contains the Blazor WebAssembly client SPA application.  It will utilize .Net HTTP library to communicate with the ASP.Net 5.0 server.)
* BlazorMovies.Shared.csproj (This project contains the shared .Net libraries used within the application.  In this application it will be to contain the Entity and DTO class files.)

# Compilation Instructions
Here are the instrcutions to compile the application using the DotNet CLI directly and publish it to an output folder so it can be deployed to a container or web server.

Run the following command in the solution directory (root of application):
```console
dotnet restore
```

Navigate to the Server project folder of the BlazorMovies application:
```console
/BlazorMovies/BlazorMovies/Server
```

Run the following commands in the Server project directory:
```console
dotnet publish BlazorMovies.Server.csproj -c release -{Output directory of web application}
```

## Additional Instructions for AWS RDS Compilation
If the application is to be run in using an AWS RDS database, a small modification to the Startup.cs file in the BlazorMovies.Server project must be made.  This is because I had some issues figuring out how to properly use Environment Variables with Docker that will be remedied later, once I understand how it works.  For the time being, perform this simple modification.&nbsp;
If running in Docker go the Startup.cs file and uncomment the following line at the top of the file if it is comment out:
```console
 #define _USERDSDB
```
  
# Launch Instructions
To launch the BlazorMovies application and start the Kestrel server, run the following command in the root folder containing the deployed web application:
```console
dotnet BlazorMovies.Server.dll
```

To view the BlazorMovies application home page navigate to the following address on the browser:
```console
http://localhost:5000
```

# Docker Script to Run Container
I created a script that can be used to run the BlazorMovies image, but is not included in the Git Repository.   The name of the script file is BlazorMoviesDockerRun.sh.  This script contains all the initialization settings, including AWS environment variables required for testing the BlazorMovies docker container on the local machine.
