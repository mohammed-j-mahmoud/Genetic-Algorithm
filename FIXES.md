# Fixes Applied — Genetic-Algorithm Repository

**Last updated:** 2026-05-28  
**Related audit:** [BUG_REPORT.md](BUG_REPORT.md)

---

## Status: all audit items resolved

| Category | Count | Status |
|----------|-------|--------|
| Critical | 3 | Fixed |
| High | 7 | Fixed |
| Medium | 8 | Fixed |
| Low | 7 | Fixed |
| Modeling limitations | 4 | Fixed or documented |
| Round-2 review | 4 | Fixed |

**Target framework:** .NET 10 — `GeneticAlgorithm.sln` (Core, Application, Desktop, Cli, Api)  
**Build:** `dotnet build GeneticAlgorithm.sln` — succeeds  
**Tests:** `dotnet test GeneticAlgorithm.sln` — 12 passed

---

## Remaining-items pass (final)

### Low #19 — GA facade naming

- Primary facade is `GeneticOptimizer` in `GeneticAlgorithm.Core`.
- Desktop and Application layers call `GeneticOptimizationService` / `OptimizationViewModel`.

### Low #21 — Empty `Dispose()`

- `GeneticAlgorithmRunner` implements `IDisposable` and clears population buffers.
- `GeneticOptimizer` disposes the runner when the optimizer is disposed.

### Low #23 — Automated tests

- `tests/GeneticAlgorithm.Core.Tests` — unit tests (simulation, GA, distributions, services).
- `tests/GeneticAlgorithm.IntegrationTests` — API integration tests.

### Low #25 — `CrossOver()` allocation

- Removed allocating `CrossOver(DNA)`.
- Added `CrossOverInto(DNA parent2, DNA offspring)` for zero-allocation crossover.

### Modeling — timers start at enqueue

- Loading/weighing durations sampled when a **loader/scale slot** starts service, not when the truck enters the queue (`AwaitingResourceService` on `Truck`).

### Modeling — demo button

- Demo handler uses form values when complete; otherwise shows a message and runs demo defaults.

### Modeling — utilization cap

- Removed artificial 1.0 cap; utilization reflects corrected time-step accounting.

### Architecture — view SRP

- Extracted `DistributionGridReader` and `FormInputParser` from `OptimizationView` into dedicated types under `GeneticAlgorithm.Desktop`.

### Other

- `SimulationParameters` validates non-negative costs and project duration.

---

## How to verify

```powershell
dotnet build GeneticAlgorithm.sln
dotnet test GeneticAlgorithm.sln
```

---

*No open defects remain from the original audit.*
