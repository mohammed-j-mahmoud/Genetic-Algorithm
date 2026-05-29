# Genetic Algorithm — Dump Truck Optimization

Discrete-event simulation + genetic algorithm for optimizing trucks, loaders, and scales.

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download) (see `global.json`)
- **Windows** — only for the WinForms desktop app
- **Linux/macOS/Docker** — use CLI or API (no GUI)

## Quick start

```powershell
# Build & test
dotnet build GeneticAlgorithm.sln
dotnet test GeneticAlgorithm.sln

# Cross-platform CLI (Linux/macOS/Windows)
dotnet run --project src/GeneticAlgorithm.Cli -- ga 20
dotnet run --project src/GeneticAlgorithm.Cli -- sim

# HTTP API
dotnet run --project src/GeneticAlgorithm.Api
# POST http://localhost:5000/api/simulation  (see docs/ARCHITECTURE.md)

# Windows GUI only
dotnet run --project src/GeneticAlgorithm.Desktop
```

## Docker (Linux)

From repo root:

```bash
docker compose -f docker-compose.yml up --build genetic-algorithm-cli
docker compose -f docker-compose.yml up --build genetic-algorithm-api
```

API exposed on port **8080**.

## Documentation

- [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md) — solution layout, MVVM, Docker, tests
- [docs/ACCOMPLISHMENTS.md](docs/ACCOMPLISHMENTS.md) — what was changed in the modernization pass
- [BUG_REPORT.md](BUG_REPORT.md) / [FIXES.md](FIXES.md) — earlier bug audit

## Solution projects

| Project | Purpose |
|---------|---------|
| `GeneticAlgorithm.Core` | Simulation + genetics engine |
| `GeneticAlgorithm.Application` | Application services |
| `GeneticAlgorithm.Desktop` | WinForms UI (MVVM) |
| `GeneticAlgorithm.Cli` | Headless runner |
| `GeneticAlgorithm.Api` | REST API |
