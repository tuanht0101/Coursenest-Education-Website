#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Identity/Identity.API/Identity.API.csproj", "Services/Identity/Identity.API/"]
COPY ["Services/CommonLibrary/CommonLibrary.API/CommonLibrary.API.csproj", "Services/CommonLibrary/CommonLibrary.API/"]
RUN dotnet restore "Services/Identity/Identity.API/Identity.API.csproj"
COPY ["Services/Identity/Identity.API/", "Services/Identity/Identity.API/"]
COPY ["Services/CommonLibrary/CommonLibrary.API/", "Services/CommonLibrary/CommonLibrary.API/"]
WORKDIR "/src/Services/Identity/Identity.API"
RUN dotnet build "Identity.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Identity.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.API.dll"]