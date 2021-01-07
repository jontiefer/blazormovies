FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source
ENV ASPNETCORE_ENVIRONMENT=Production
#ENV ASPNETCORE_URLS http://*:5000
#EXPOSE 5000
#EXPOSE 80

# copy csproj and restore as distinct layers
COPY *.sln .
COPY BlazorMovies/Server/BlazorMovies.Server.csproj ./blazormovies/Server/
COPY BlazorMovies/Client/BlazorMovies.Client.csproj ./blazormovies/Client/
COPY BlazorMovies/Shared/BlazorMovies.Shared.csproj ./blazormovies/Shared/
COPY . .
RUN dotnet restore ./blazormovies/Server/BlazorMovies.Server.csproj
RUN dotnet restore ./blazormovies/Client/BlazorMovies.Client.csproj
RUN dotnet restore ./blazormovies/Shared/BlazorMovies.Shared.csproj

# copy everything else and build app
COPY BlazorMovies/Server/. ./blazormovies/Server/
COPY BlazorMovies/Client/. ./blazormovies/Client/
COPY BlazorMovies/Shared/. ./blazormovies/Shared/
WORKDIR /source/blazormovies
RUN dotnet publish ./Server/BlazorMovies.Server.csproj -c release -o /app --no-restore


# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "BlazorMovies.Server.dll"]