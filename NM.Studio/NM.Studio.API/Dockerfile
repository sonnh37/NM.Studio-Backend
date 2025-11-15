FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NM.Studio.API/NM.Studio.API.csproj", "NM.Studio.API/"]
COPY ["NM.Studio.Data/NM.Studio.Data.csproj", "NM.Studio.Data/"]
COPY ["NM.Studio.Domain/NM.Studio.Domain.csproj", "NM.Studio.Domain/"]
COPY ["NM.Studio.Services/NM.Studio.Services.csproj", "NM.Studio.Services/"]
COPY ["NM.Studio.Validations/NM.Studio.Validations.csproj", "NM.Studio.Validations/"]
RUN dotnet restore "NM.Studio.API/NM.Studio.API.csproj"
COPY . .
WORKDIR "/src/NM.Studio.API"
RUN dotnet build "NM.Studio.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "NM.Studio.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NM.Studio.API.dll"]
