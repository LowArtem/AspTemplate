FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["FiveHeadsApp.Api/FiveHeadsApp.Api.csproj", "FiveHeadsApp.Api/"]
COPY ["FiveHeadsApp.Data/FiveHeadsApp.Data.csproj", "FiveHeadsApp.Data/"]
COPY ["FiveHeadsApp.Core/FiveHeadsApp.Core.csproj", "FiveHeadsApp.Core/"]
RUN dotnet restore "FiveHeadsApp.Api/FiveHeadsApp.Api.csproj"
COPY . .
WORKDIR "/src/FiveHeadsApp.Api"
RUN dotnet build "FiveHeadsApp.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FiveHeadsApp.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FiveHeadsApp.Api.dll"]
