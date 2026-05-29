using System;

using System.Collections.Concurrent;

using System.Runtime.CompilerServices;



namespace GeneticAlgorithm.Core.Genetics

{

    /// <summary>

    /// Caches <see cref="FitnessSnapshot"/> values by (trucks, loaders, scalers).

    /// Uses a dense array for small search spaces; falls back to a packed-key dictionary when large.

    /// </summary>

    internal sealed class SimulationFitnessCache

    {

        private const int MaxDenseCacheCells = 5000;

        private const int MaxPrewarmCells = 5000;



        private readonly Func<int, int, int, FitnessSnapshot> _evaluate;

        private readonly int _maxTrucks;

        private readonly int _maxLoaders;

        private readonly int _maxScalers;

        private readonly FitnessSnapshot[] _denseCache;

        private readonly bool[] _densePopulated;

        private readonly ConcurrentDictionary<long, FitnessSnapshot> _sparseCache;

        private readonly bool _useDenseCache;



        private long _simulationCalls;

        private long _cacheHits;



        /// <summary>Number of simulations executed on cache miss.</summary>

        public long SimulationCalls => _simulationCalls;



        /// <summary>Number of cache hits.</summary>

        public long CacheHits => _cacheHits;



        /// <summary>

        /// Creates a cache for the given search bounds and evaluation delegate.

        /// </summary>

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

                _denseCache = new FitnessSnapshot[(int)cells];

                _densePopulated = new bool[(int)cells];

            }

            else

            {

                _useDenseCache = false;

                _sparseCache = new ConcurrentDictionary<long, FitnessSnapshot>();

            }

        }



        /// <summary>

        /// Returns cached metrics for the resource triple, evaluating on miss.

        /// </summary>

        public FitnessSnapshot GetOrEvaluate(int trucks, int loaders, int scalers)

        {

            ValidateGeneBounds(trucks, loaders, scalers);



            if (_useDenseCache)

                return GetOrEvaluateDense(trucks, loaders, scalers);



            return GetOrEvaluateSparse(trucks, loaders, scalers);

        }



        /// <summary>

        /// Fills the cache for every combination in the bounded search space when the space is small.

        /// </summary>

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

            if (_densePopulated[index])

            {

                _cacheHits++;

                return _denseCache[index];

            }



            FitnessSnapshot computed = _evaluate(trucks, loaders, scalers);

            _denseCache[index] = computed;

            _densePopulated[index] = true;

            _simulationCalls++;

            return computed;

        }



        private FitnessSnapshot GetOrEvaluateSparse(int trucks, int loaders, int scalers)

        {

            long key = PackKey(trucks, loaders, scalers);

            if (_sparseCache.TryGetValue(key, out FitnessSnapshot cached))

            {

                _cacheHits++;

                return cached;

            }



            FitnessSnapshot computed = _evaluate(trucks, loaders, scalers);

            _sparseCache[key] = computed;

            _simulationCalls++;

            return computed;

        }



        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        private int ToDenseIndex(int trucks, int loaders, int scalers) =>

            (trucks - 1) + (loaders - 1) * _maxTrucks + (scalers - 1) * _maxTrucks * _maxLoaders;



        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        private static long PackKey(int trucks, int loaders, int scalers) =>

            ((long)trucks << 32) | ((long)loaders << 16) | (uint)scalers;

    }

}


