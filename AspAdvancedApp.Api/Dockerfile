FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AspAdvancedApp.Api/AspAdvancedApp.Api.csproj", "AspAdvancedApp.Api/"]
COPY ["AspAdvancedApp.Data/AspAdvancedApp.Data.csproj", "AspAdvancedApp.Data/"]
COPY ["AspAdvancedApp.Core/AspAdvancedApp.Core.csproj", "AspAdvancedApp.Core/"]
RUN dotnet restore "AspAdvancedApp.Api/AspAdvancedApp.Api.csproj"
COPY . .
WORKDIR "/src/AspAdvancedApp.Api"
RUN dotnet build "AspAdvancedApp.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AspAdvancedApp.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AspAdvancedApp.Api.dll"]
