# Architecture & delivery plan

## Goals

1. Clear, consistent naming across layers (`GeneticOptimizer`, `OptimizationView`, `TruckState`, etc.)
2. MVVM on the desktop UI (WinForms — this repo never used WPF; MVVM applies the same patterns)
3. Cross-platform execution via **CLI** and **HTTP API** (Linux/macOS/Windows + Docker)
4. Windows GUI remains optional (`GeneticAlgorithm.Desktop`)
5. Broad automated test coverage (unit + integration)

## Solution layout

```
GeneticAlgorithm.sln
src/
  GeneticAlgorithm.Core/          # Simulation + genetics (net10.0, no UI)
  GeneticAlgorithm.Application/   # Use-case services & DTOs
  GeneticAlgorithm.Desktop/       # WinForms + MVVM (net10.0-windows)
  GeneticAlgorithm.Cli/           # Cross-platform console (net10.0)
  GeneticAlgorithm.Api/           # REST API for automation & integration tests
tests/
  GeneticAlgorithm.Core.Tests/    # Unit tests
  GeneticAlgorithm.IntegrationTests/  # API tests (WebApplicationFactory)
docker/
  Dockerfile                      # CLI image
  Dockerfile.api                  # API image
docker-compose.yml
```

## Layering

| Layer | Responsibility |
|-------|----------------|
| **Core** | `DumpTruckSimulation`, `DumpTruckSimulationEngine`, `GeneticOptimizer` (GeneticSharp), `DNA`, distributions |
| **Application** | `SimulationService`, `GeneticOptimizationService`, `DistributionNormalizer`, request/result models |
| **Desktop** | `OptimizationView` (view), `OptimizationViewModel` (view model), `FormInputParser`, grids |
| **Cli** | Interactive shell + one-shot commands — see `help` for full list |
| **Api** | REST routes — see `GET /` or `GET /api/endpoints` |

## MVVM (desktop)

- **View:** `OptimizationView` — controls, charts, event wiring only
- **ViewModel:** `OptimizationViewModel` — calls Application services, no `MessageBox` / `DataGridView`
- **Model:** Application DTOs (`SimulationRequest`, `GeneticOptimizationRequest`, …)

Binding is intentionally light (WinForms); business logic no longer lives in the view code-behind for GA/simulation runs.

## Cross-platform

| Platform | How to run |
|----------|------------|
| **Linux / macOS / Docker** | `dotnet run --project src/GeneticAlgorithm.Cli` or Docker Compose |
| **Windows GUI** | `dotnet run --project src/GeneticAlgorithm.Desktop` |
| **HTTP clients** | `GeneticAlgorithm.Api` — `https://localhost:7190`, `http://localhost:5296`, or Docker `http://localhost:8080` |

### CLI commands (match API)

| Command | API |
|---------|-----|
| `simulation` | `POST /api/simulation` |
| `genetic-algorithm` | `POST /api/genetic-algorithm` |
| `exhaustive-search` | `POST /api/exhaustive-search` |
| `genetic-search` | `POST /api/genetic-search` |
| `surrogate-search` | `POST /api/surrogate-search` |
| `dynamic-programming-search` | `POST /api/dynamic-programming-search` |

Run with no args for interactive shell (`truck-fleet>`). Type `help` for options and templates.

WinForms does **not** run on Linux; CLI/API share the same Core + Application logic.

## Docker

From repository root:

```bash
docker compose -f docker-compose.yml up --build genetic-algorithm-cli
docker compose -f docker-compose.yml up --build genetic-algorithm-api
```

## Tests

| Project | Scope |
|---------|--------|
| `GeneticAlgorithm.Core.Tests` | Engine, parameters, normalizer, optimizer, roulette selection, application services |
| `GeneticAlgorithm.IntegrationTests` | API + CLI integration tests (WinForms parity) |

```bash
dotnet test GeneticAlgorithm.sln
```

## Build requirements

- .NET 10 SDK (`global.json` pins 10.0.300)
- Windows SDK only for `GeneticAlgorithm.Desktop`
