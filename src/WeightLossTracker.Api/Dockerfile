FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["../WeightLossTracker.Api/WeightLossTracker.Api.csproj", "../WeightLossTracker.Api/"]
COPY ["../WeightLossTracker.DataStore/WeightLossTracker.DataStore.csproj", "../WeightLossTracker.DataStore/"]
RUN dotnet restore "../WeightLossTracker.Api/WeightLossTracker.Api.csproj"
COPY . .
WORKDIR "/src/../WeightLossTracker.Api"
RUN dotnet build "WeightLossTracker.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "WeightLossTracker.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WeightLossTracker.Api.dll"]