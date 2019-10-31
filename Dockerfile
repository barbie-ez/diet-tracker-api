FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["/src/WeightLossTracker.Api/WeightLossTracker.Api.csproj", "/src/WeightLossTracker.Api/"]
COPY ["/src/WeightLossTracker.DataStore/WeightLossTracker.DataStore.csproj", "/src/WeightLossTracker.DataStore/"]
RUN dotnet restore "/src/WeightLossTracker.Api/WeightLossTracker.Api.csproj"
COPY . .
WORKDIR "/src/../WeightLossTracker.Api"
RUN dotnet build "/src/WeightLossTracker.Api/WeightLossTracker.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "/src/WeightLossTracker.Api/WeightLossTracker.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "/src/WeightLossTracker.Api/WeightLossTracker.Api.dll"]
