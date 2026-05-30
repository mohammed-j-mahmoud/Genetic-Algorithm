# Truck Fleet Problem

Discrete-event simulation and optimization for dump-truck fleets: find a good mix of **trucks**, **loaders**, and **scalers** to move material at minimum cost.

The desktop app is titled **Truck Fleet Problem**. It simulates loading, weighing, and travel times, then searches for fleet sizes using several methods you can compare side by side.

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
dotnet run --project src/GeneticAlgorithm.Cli
# At truck-fleet> prompt: help | simulation | exhaustive-search | genetic-algorithm | genetic-search | ...

dotnet run --project src/GeneticAlgorithm.Cli -- simulation --coal 10000 --trucks 6 --load-per-truck 20
dotnet run --project src/GeneticAlgorithm.Cli -- exhaustive-search --max-trucks 6 --max-loaders 2 --max-scalers 2

# HTTP API (Visual Studio HTTPS profile)
dotnet run --project src/GeneticAlgorithm.Api
# https://localhost:7190/  — docs + GET /api/endpoints
# http://localhost:5296/   — HTTP profile
# Docker: http://localhost:8080/

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

---

## CLI (`truck-fleet-problem`)

Same desktop tabs as the WinForms app and the HTTP API.

| Command | Desktop tab | API route |
|---------|-------------|-----------|
| `simulation` | Simulation | `POST /api/simulation` |
| `genetic-algorithm` | Genetic Algorithm | `POST /api/genetic-algorithm` |
| `exhaustive-search` | Exhaustive Search | `POST /api/exhaustive-search` |
| `genetic-search` | Genetic Search | `POST /api/genetic-search` |
| `surrogate-search` | Surrogate Search | `POST /api/surrogate-search` |
| `dynamic-programming-search` | Dynamic Programming Search | `POST /api/dynamic-programming-search` |

**Interactive shell** (no arguments): type commands at `truck-fleet>` — do **not** prefix with `truck-fleet-problem`.

**One-shot** (scripts): `truck-fleet-problem simulation --coal 10000 --load-per-truck 20 ...`

**Required flags:** include `--load-per-truck` (demo default `20`) and positive `--trucks`, `--loaders`, `--scalers`.

Type **`help`** at the prompt for full options, aliases, and copy-paste templates.

---

## Desktop app — tab guide

### What problem are we solving?

You need to move a volume of material (m³). **Trucks** haul loads, **loaders** fill trucks, and **scalers** weigh them. More equipment costs more per day; too little equipment causes **delay penalties** if the job runs past the planned duration.

The app estimates **total cost**, **total days**, and **fitness** (higher is better) for each fleet combination.

### Recommended order for new users

1. **Distribution** — set how long loading, weighing, and travel take  
2. **Simulation** — test one fleet size manually  
3. **Genetic Algorithm** — classic evolutionary search  
4. **Exhaustive Search**, **Genetic Search**, **Surrogate Search**, **Dynamic Programming Search** — compare alternative optimizers using the same inputs  

---

### Tab: Distribution

**Purpose:** Define random timing for the simulation.

Three tables (Loading, Weighing, Traveling) list **Time (min)** and **Probability**. Example: “10 minutes, 50%” means half the operations take about 10 minutes.

**Action:** Fill the tables, then save. All other tabs use these distributions.

**Stop button:** Not shown — this tab only saves settings; it does not run a long calculation.

---

### Tab: Simulation

**Purpose:** Run **one** fleet configuration (exact truck/loader/scaler counts) and see costs, utilization, and a cost chart.

**Inputs:** Material volume, costs, project duration, fleet counts, etc. (with units on each label).

**Actions:** **Run Simulation** · **Clear Fields** · **Stop** (cancels a run in progress)

**Outputs:** Truck/loader/scaler costs, delay cost, total cost, utilization ratios.

---

### Tab: Genetic Algorithm

**Purpose:** Search for a good fleet using a **genetic algorithm** (evolution over many **generations**).

**How it searches:** Samples the search space (does not try every combo). Each fitness check runs a **seeded stochastic simulation** (realistic random times). Results for the same `(trucks, loaders, scalers)` are **cached** so identical combos are not simulated twice.

**Chart:** **Best fitness in each generation** — the best score in the current generation’s population (not necessarily a new all-time record every time).

**Actions:** **Run Demo Using Default Values** · **Run Genetic Algorithm** · **Stop**

**When stopped early:** The **Best result** box and stats keep the **best solution found so far** (generations completed, simulations, cache hits).

**Shared inputs:** Material, costs, **max** trucks/loaders/scalers, population, generations, mutation rate (same values used by the three search tabs below).

---

### Tab: Exhaustive Search

**Purpose:** Try **every** combination within max trucks × loaders × scalers.

| Step | Method |
|------|--------|
| Search | Brute force all combos |
| During search | Expected-time simulation (fast averages) + cache |
| Final step | One seeded stochastic verify on the best combo |

**Best when:** Small search bounds (exhaustive is slow if bounds are huge).

**Actions:** **Run Exhaustive Search** · **Stop**

**When stopped early:** Shows the best combo found among combinations already evaluated (expected-time metrics until verify would have run).

---

### Tab: Genetic Search

**Purpose:** Faster search for medium/large bounds — GA on fast fitness, then verify finalists.

| Step | Method |
|------|--------|
| Search | Genetic algorithm |
| During search | Expected-time simulation + cache |
| Final step | Parallel seeded stochastic verify on **top 5** candidates |

**Actions:** **Run Genetic Search** · **Stop**

**When stopped early:** Shows the best GA result so far (expected-time metrics; verify step is skipped if stopped during GA).

---

### Tab: Surrogate Search

**Purpose:** Rank every fleet combo, then stochastic-verify the **top 5** (unlike Exhaustive/DP, which verify only the single best).

| Step | Method |
|------|--------|
| Search (≤ 50,000 combos) | Expected-time simulation + cache — **same ranking model as Exhaustive Search and DP** |
| Search (> 50,000 combos) | Improved **pipelined surrogate formula** (loader/scaler/truck bottlenecks, 480-minute work days) |
| Final step | Parallel seeded stochastic verify on **top 5** candidates |

**Best when:** You want the same ranking as exhaustive on normal bounds, but prefer verifying several finalists instead of one.

**Actions:** **Run Surrogate Search** · **Stop**

**When stopped early:** Shows the best combo from combinations ranked so far.

---

### Tab: Dynamic Programming Search

**Purpose:** Find the **provably best fleet under expected-time simulation** within your max bounds.

| Step | Method |
|------|--------|
| Search | Bottom-up **3D DP table** — each cell stores the best fitness with at most *t* trucks, *l* loaders, *s* scalers |
| During search | Expected-time discrete-event simulation + cache (same model as Exhaustive Search, not the surrogate formula) |
| Final step | One seeded stochastic verify on the DP optimum |

**Not the same as Surrogate Search:** DP always uses the **full expected-time simulator** and guarantees the best expected-time solution when the table completes. Surrogate Search uses the same simulator for ranking when the search space is small (≤ 50,000 combos); above that it falls back to a fast approximate formula.

**Actions:** **Run Dynamic Programming Search** · **Stop**

**When stopped early:** Shows the best result from DP cells filled so far (expected-time metrics).

---

## Comparing search methods

| Tab | Search | Speed | Final verify |
|-----|--------|-------|--------------|
| Genetic Algorithm | Evolution + stochastic sim | Medium | Built into search (cached) |
| Exhaustive Search | Every combo | Slowest if bounds are large | 1× stochastic on best |
| Genetic Search | Evolution + expected-time | Fast | Top 5 stochastic |
| Surrogate Search | Expected-time sim (small bounds) or pipelined formula (huge bounds) | Fast | Top 5 stochastic |
| Dynamic Programming Search | 3D DP + expected-time sim | Same work as exhaustive* | 1× stochastic on best |

\*DP evaluates the same number of fleet combos as exhaustive search, but builds an explicit optimal substructure table instead of a flat loop.

Use the **Best result** box on each tab with the **same inputs** on the Genetic Algorithm tab to compare.

---

## Cache (Genetic Algorithm tab stats)

- **Simulations** — unique `(trucks, loaders, scalers)` combos actually simulated  
- **Cache hits** — times a combo was reused from memory  
- **Generations run** — how many GA generations completed  
- **Best found at generation** — when the overall best solution last improved  

---

## Documentation

- [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md) — solution layout, MVVM, Docker, tests
- [docs/ACCOMPLISHMENTS.md](docs/ACCOMPLISHMENTS.md) — modernization notes
- [BUG_REPORT.md](BUG_REPORT.md) / [FIXES.md](FIXES.md) — earlier bug audit

## Solution projects

| Project | Purpose |
|---------|---------|
| `GeneticAlgorithm.Core` | Simulation + genetics engine |
| `GeneticAlgorithm.Application` | Application services |
| `GeneticAlgorithm.Desktop` | WinForms UI (MVVM) |
| `GeneticAlgorithm.Cli` | Headless runner |
| `GeneticAlgorithm.Api` | REST API |
