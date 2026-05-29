# Cross-platform CLI (Linux/macOS/Windows containers)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore GeneticAlgorithm.sln \
    && dotnet publish src/GeneticAlgorithm.Cli/GeneticAlgorithm.Cli.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/runtime:10.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "genetic-algorithm.dll"]
CMD ["demo"]
