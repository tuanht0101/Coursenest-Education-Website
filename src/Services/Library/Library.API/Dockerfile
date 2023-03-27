#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Library/Library.API/Library.API.csproj", "Services/Library/Library.API/"]
COPY ["Services/CommonLibrary/CommonLibrary.API/CommonLibrary.API.csproj", "Services/CommonLibrary/CommonLibrary.API/"]
RUN dotnet restore "Services/Library/Library.API/Library.API.csproj"
COPY ["Services/Library/Library.API/", "Services/Library/Library.API/"]
COPY ["Services/CommonLibrary/CommonLibrary.API/", "Services/CommonLibrary/CommonLibrary.API/"]
WORKDIR "/src/Services/Library/Library.API"
RUN dotnet build "Library.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Library.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Library.API.dll"]