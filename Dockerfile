FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 80
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["KioskApi.csproj", "./"]
RUN dotnet restore "./KioskApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "KioskApi.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "KioskApi.csproj" -c Release -o /app/publish
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KioskApi.dll"]
