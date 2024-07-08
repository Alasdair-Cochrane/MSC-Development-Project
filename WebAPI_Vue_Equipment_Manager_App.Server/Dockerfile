#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
# https://stackoverflow.com/questions/28372328/how-to-install-the-google-cloud-sdk-in-a-docker-image
# Downloading gcloud package



FROM mcr.microsoft.com/dotnet/sdk:8.0 AS with-node
RUN apt-get update
RUN apt-get install curl
RUN curl -sL https://deb.nodesource.com/setup_20.x | bash
RUN apt-get -y install nodejs



FROM with-node AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WebAPI_Vue_Equipment_Manager_App.Server/WebAPI_Vue_Equipment_Manager_App.Server.csproj", "WebAPI_Vue_Equipment_Manager_App.Server/"]
COPY ["webapi_vue_equipment_manager_app.client/webapi_vue_equipment_manager_app.client.esproj", "webapi_vue_equipment_manager_app.client/"]
RUN dotnet restore "./WebAPI_Vue_Equipment_Manager_App.Server/WebAPI_Vue_Equipment_Manager_App.Server.csproj"
COPY . .
WORKDIR "/src/WebAPI_Vue_Equipment_Manager_App.Server"
RUN dotnet build "./WebAPI_Vue_Equipment_Manager_App.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebAPI_Vue_Equipment_Manager_App.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebAPI_Vue_Equipment_Manager_App.Server.dll"]
