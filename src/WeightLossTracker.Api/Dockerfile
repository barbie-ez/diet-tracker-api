FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["../WeightLossTracker.Api/WeightLossTracker.Api.csproj", "../WeightLossTracker.Api/"]
COPY ["../WeightLossTracker.DataStore/WeightLossTracker.DataStore.csproj", "../WeightLossTracker.DataStore/"]
RUN dotnet restore "../WeightLossTracker.Api/WeightLossTracker.Api.csproj"
COPY . .
WORKDIR "/src/../WeightLossTracker.Api"
RUN dotnet build "WeightLossTracker.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WeightLossTracker.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WeightLossTracker.Api.dll"]