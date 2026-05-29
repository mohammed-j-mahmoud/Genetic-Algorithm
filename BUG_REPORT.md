# Bug Report — Genetic-Algorithm Repository

**Audit date:** 2026-05-28  
**Scope:** `src/GeneticAlgorithm.Desktop/`, `src/GeneticAlgorithm.Core/`  
**Build:** .NET 10 (`GeneticAlgorithm.sln`)

---

## Executive summary

| Severity | Count |
|----------|-------|
| Critical | 3 |
| High | 7 |
| Medium | 8 |
| Low | 7 |

The most impactful issue for **trust in numbers** is the simulation **time-step mismatch** (clock advances by `min` minutes while queue timers only decrement by 1). The most impactful for **stability** is **zero loaders/scales** (infinite loop) and **unvalidated UI parsing** (crashes).

---

## Critical

### 1. Simulation clock vs. queue timer mismatch

**Location:** `DumpTruckSimulationEngine.AdvanceTimeStep`, `ProcessLoadingQueue`, `ProcessWeighingQueue`, `ProcessTravelingQueue`

Each step computes `timeStepMinutes = min` (e.g. 10) and adds that value to `_totalTimeMinutes` and utilization totals, but active queue entries only do `TimeLeft -= 1`. Simulated calendar time runs faster than operation progress whenever `min > 1`, skewing days, costs, and utilization.

---

### 2. Zero loaders or zero scales → infinite loop

**Location:** `SimulationParameters.Validate`, `ProcessLoadingQueue` / `ProcessWeighingQueue`

`LoaderCount` and `ScalerCount` may be 0. With 0 loaders, trucks enter the loading queue but never receive service; timers never reach 0 and the main loop never terminates.

---

### 3. Coal volume can go negative

**Location:** `ProcessTravelingQueue` — `_coalRemaining -= TruckLoadVolume`

Multiple trips completing in the same step can drive `_coalRemaining` below zero before the loop exits.

---

## High

### 4. Mutation rate text box ignored

**Location:** `OptimizationView.button2_Click`, `txtbxMutationRate`

The UI exposes a mutation rate field, but the GA runner always used the default `0.01`.

---

### 5. Distribution grid loading is fragile

**Location:** `OptimizationView` (distribution save handler)

Uses `int.Parse` / `double.Parse` on cell values with no null/empty checks. Empty or invalid cells cause `FormatException`. Users must click **Add to Simulation**; otherwise built-in defaults apply silently.

---

### 6. Probability weights not normalized

**Location:** `DiscreteDistribution.BuildCumulative`, `dataGridView_CellValidating`

Validation only rejects `sum > 1`. If rows sum to less than 1, extra probability mass effectively falls through to the last bucket, skewing sampled durations.

---

### 7. Single simulation blocks the UI thread

**Location:** `OptimizationView` (simulation run handler)

GA runs on a background task; the Simulation tab called the simulation engine synchronously on the UI thread.

---

### 8. No robust input validation on GA / simulation fields

**Location:** `OptimizationView` — GA, simulation, and demo handlers

Bare `Parse` calls with no `TryParse`, range checks, or culture-invariant parsing.

---

### 9. Inactive trucks retain stale state in tail phase

**Location:** `DumpTruckSimulationEngine.Run`

After the full-fleet phase, trucks with index ≥ `activeTruckCount` keep queue flags and timers from the prior phase.

---

### 10. Utilization can exceed 1.0

**Location:** `SimulationCostCalculator.SafeUtilization` plus per-step accumulation

Resource minutes are over-counted relative to wall-clock time; displayed utilization can exceed 100% while labels imply a percentage.

---

## Medium

### 11. Progress bar updates only when the chart updates

**Location:** `RunGeneticAlgorithmAsync` — progress handler skips updates unless `UpdateChart` is true (every 5 generations).

---

### 12. Sparse fitness cache key collision

**Location:** `SimulationFitnessCache.PackKey` — bit packing with 10-bit fields

If any gene bound exceeds 1023, different (trucks, loaders, scalers) triples can collide and return wrong cached fitness.

---

### 13. Loading vs. weighing completion inconsistency

**Location:** `ProcessLoadingQueue` uses `== 0`; `ProcessWeighingQueue` uses `<= 0`

---

### 14. `DNA.Genes` exposes mutable internal array

**Location:** `DNA.Genes` returns `_genes` directly

External mutation can desynchronize genes from cached fitness.

---

### 15. Prewarm runs full search space on startup

**Location:** `GeneticAlgorithmRunner` constructor → `Prewarm()`

Large max bounds cause extreme startup delay before generation 1.

---

### 16. Validation message contradicts rule

**Location:** `dataGridView_CellValidating` — message says “non-negative integer” but code requires `> 0`.

---

### 17. Locale-sensitive parsing

**Location:** All `Parse` calls use current culture

Decimal-comma locales can break probability entry.

---

### 18. `fullFleetThreshold` uses float `TruckCount` vs. integer fleet size

**Location:** `DumpTruckSimulationEngine.Run`

Non-integer truck counts (if passed) desync threshold from `TruckFleetSize`.

---

## Low

| # | Issue |
|---|--------|
| 19 | GA facade type naming was unclear |
| 20 | Column header typo: **Propability** |
| 21 | `GeneticAlgorithm.Dispose()` is empty |
| 22 | Chart bars at X=0 and X=2 with X axis disabled |
| 23 | No automated tests |
| 24 | `lib/System.Resources.Extensions.dll` checked in for build |
| 25 | `CrossOver()` allocates a new `DNA` (hot path uses `ReproduceWith`) |

---

## Previously fixed (before this audit pass)

| Issue | Status |
|--------|--------|
| GA ran on UI thread | Fixed — `async` + `Task.Run` |
| Unsafe `Parallel.For` + `List.Add` | Fixed |
| Offspring crossover without `Evaluate()` | Fixed |
| Wrong crossover constructor bounds | Fixed (refactor) |
| Roulette selection returning `null` | Fixed |

---

## Recommended fix order

1. Simulation time-step decrement alignment  
2. Reject 0 loaders / 0 scales  
3. Clamp `_coalRemaining` at 0  
4. Wire `txtbxMutationRate`  
5. Normalize distribution weights on save  
6. Background-thread simulation  
7. `TryParse` + validation on all numeric inputs  
8. Tail-phase fleet reset and queue purge  
9. Cache key / prewarm guards  
10. Read-only gene exposure  

---

---

## Remediation status

**All critical, high, and medium issues have been fixed.** See [FIXES.md](FIXES.md) for per-item changes, files touched, and round-2 review fixes.

| Severity | Fixed |
|----------|-------|
| Critical | 3/3 |
| High | 7/7 |
| Medium | 8/8 |
| Low | 7/7 |

*Original audit snapshot. All items remediated 2026-05-28 — see [FIXES.md](FIXES.md).*
