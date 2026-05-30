using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;

namespace GeneticAlgorithm.Core.Genetics
{
    /// <summary>
    /// Thread-safe cache of <see cref="FitnessSnapshot"/> values by (trucks, loaders, scalers).
    /// </summary>
    internal sealed class SimulationFitnessCache
    {
        private const int MaxDenseCacheCells = 5000;
        private const int MaxPrewarmCells = 5000;

        private readonly Func<int, int, int, FitnessSnapshot> _evaluate;
        private readonly int _maxTrucks;
        private readonly int _maxLoaders;
        private readonly int _maxScalers;
        private readonly ConcurrentDictionary<int, FitnessSnapshot> _denseCache;
        private readonly ConcurrentDictionary<long, FitnessSnapshot> _sparseCache;
        private readonly bool _useDenseCache;

        private long _simulationCalls;
        private long _cacheHits;

        public long SimulationCalls => _simulationCalls;
        public long CacheHits => _cacheHits;

        public SimulationFitnessCache(
            Func<int, int, int, FitnessSnapshot> evaluate,
            int maxTrucks,
            int maxLoaders,
            int maxScalers)
        {
            _evaluate = evaluate ?? throw new ArgumentNullException(nameof(evaluate));
            _maxTrucks = maxTrucks;
            _maxLoaders = maxLoaders;
            _maxScalers = maxScalers;

            long cells = (long)maxTrucks * maxLoaders * maxScalers;
            if (cells <= MaxDenseCacheCells)
            {
                _useDenseCache = true;
                _denseCache = new ConcurrentDictionary<int, FitnessSnapshot>();
            }
            else
            {
                _useDenseCache = false;
                _sparseCache = new ConcurrentDictionary<long, FitnessSnapshot>();
            }
        }

        public FitnessSnapshot GetOrEvaluate(int trucks, int loaders, int scalers)
        {
            ValidateGeneBounds(trucks, loaders, scalers);
            return _useDenseCache
                ? GetOrEvaluateDense(trucks, loaders, scalers)
                : GetOrEvaluateSparse(trucks, loaders, scalers);
        }

        public void Prewarm()
        {
            long cells = (long)_maxTrucks * _maxLoaders * _maxScalers;
            if (cells > MaxPrewarmCells)
                return;

            for (int trucks = 1; trucks <= _maxTrucks; trucks++)
            {
                for (int loaders = 1; loaders <= _maxLoaders; loaders++)
                {
                    for (int scalers = 1; scalers <= _maxScalers; scalers++)
                        GetOrEvaluate(trucks, loaders, scalers);
                }
            }
        }

        private void ValidateGeneBounds(int trucks, int loaders, int scalers)
        {
            if (trucks < 1 || trucks > _maxTrucks)
                throw new ArgumentOutOfRangeException(nameof(trucks), "Truck gene is outside configured bounds.");
            if (loaders < 1 || loaders > _maxLoaders)
                throw new ArgumentOutOfRangeException(nameof(loaders), "Loader gene is outside configured bounds.");
            if (scalers < 1 || scalers > _maxScalers)
                throw new ArgumentOutOfRangeException(nameof(scalers), "Scaler gene is outside configured bounds.");
        }

        private FitnessSnapshot GetOrEvaluateDense(int trucks, int loaders, int scalers)
        {
            int index = ToDenseIndex(trucks, loaders, scalers);
            if (_denseCache.TryGetValue(index, out FitnessSnapshot existing))
            {
                Interlocked.Increment(ref _cacheHits);
                return existing;
            }

            return _denseCache.GetOrAdd(index, _ =>
            {
                Interlocked.Increment(ref _simulationCalls);
                return _evaluate(trucks, loaders, scalers);
            });
        }

        private FitnessSnapshot GetOrEvaluateSparse(int trucks, int loaders, int scalers)
        {
            long key = PackKey(trucks, loaders, scalers);
            if (_sparseCache.TryGetValue(key, out FitnessSnapshot existing))
            {
                Interlocked.Increment(ref _cacheHits);
                return existing;
            }

            return _sparseCache.GetOrAdd(key, _ =>
            {
                Interlocked.Increment(ref _simulationCalls);
                return _evaluate(trucks, loaders, scalers);
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ToDenseIndex(int trucks, int loaders, int scalers) =>
            (trucks - 1) + (loaders - 1) * _maxTrucks + (scalers - 1) * _maxTrucks * _maxLoaders;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long PackKey(int trucks, int loaders, int scalers) =>
            ((long)trucks << 32) | ((long)loaders << 16) | (uint)scalers;
    }
}
