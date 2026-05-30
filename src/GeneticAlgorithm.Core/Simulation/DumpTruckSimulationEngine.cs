using System;



namespace GeneticAlgorithm.Core.Simulation

{

    /// <summary>

    /// Discrete-event simulator for the dump-truck loading / weighing / traveling problem.

    /// </summary>

    public sealed class DumpTruckSimulationEngine

    {

        private readonly SimulationParameters _parameters;

        private readonly IOperationDurationSampler _sampler;

        private readonly Truck[] _trucks;

        private readonly Int32Queue _loadingQueue;

        private readonly Int32Queue _weighingQueue;

        private readonly Int32Queue _travelingQueue;

        private readonly bool[] _queuedForLoading;

        private readonly bool[] _queuedForWeighing;

        private readonly bool[] _queuedForTraveling;



        private float _coalRemaining;

        private float _totalTimeMinutes;

        private float _totalTimeTruck;

        private float _totalTimeLoader;

        private float _totalTimeScaler;

        private bool _tailPhasePrepared;

        private int _lastActiveTruckCount = int.MaxValue;



        public DumpTruckSimulationEngine(SimulationParameters parameters, IOperationDurationSampler sampler = null)

        {

            _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));

            _sampler = sampler ?? new TimeDistributionSampler();



            int fleetSize = _parameters.TruckFleetSize;

            _trucks = new Truck[fleetSize];

            for (int i = 0; i < fleetSize; i++)

                _trucks[i] = new Truck();



            _loadingQueue = new Int32Queue(fleetSize);

            _weighingQueue = new Int32Queue(fleetSize);

            _travelingQueue = new Int32Queue(fleetSize);

            _queuedForLoading = new bool[fleetSize];

            _queuedForWeighing = new bool[fleetSize];

            _queuedForTraveling = new bool[fleetSize];



            _coalRemaining = _parameters.CoalVolume;

        }



        public SimulationResult Run()

        {

            float fullFleetThreshold = _parameters.TruckFleetSize * _parameters.TruckLoadVolume;



            while (_coalRemaining > fullFleetThreshold)

                AdvanceTimeStep(_trucks.Length);



            while (_coalRemaining > 0)

            {

                if (!_tailPhasePrepared)

                {

                    PrepareTailPhase();

                    _tailPhasePrepared = true;

                }



                int activeTrucks = Math.Min(

                    _trucks.Length,

                    (int)Math.Ceiling(_coalRemaining / _parameters.TruckLoadVolume));

                activeTrucks = Math.Max(1, activeTrucks);



                if (activeTrucks < _lastActiveTruckCount)

                    ConstrainFleetTo(activeTrucks);



                _lastActiveTruckCount = activeTrucks;

                AdvanceTimeStep(activeTrucks);

            }



            return SimulationCostCalculator.BuildResult(

                _parameters,

                _totalTimeMinutes,

                _totalTimeTruck,

                _totalTimeLoader,

                _totalTimeScaler);

        }



        private void PrepareTailPhase()

        {

            _loadingQueue.Clear();

            _weighingQueue.Clear();

            _travelingQueue.Clear();



            for (int i = 0; i < _trucks.Length; i++)

            {

                _queuedForLoading[i] = false;

                _queuedForWeighing[i] = false;

                _queuedForTraveling[i] = false;

            }



            int activeTrucks = Math.Min(

                _trucks.Length,

                (int)Math.Ceiling(_coalRemaining / _parameters.TruckLoadVolume));

            ConstrainFleetTo(Math.Max(1, activeTrucks));

            _lastActiveTruckCount = activeTrucks;

        }



        private void ConstrainFleetTo(int activeTruckCount)

        {

            for (int i = activeTruckCount; i < _trucks.Length; i++)

                _trucks[i].Reset();



            PurgeQueueAboveIndex(_loadingQueue, activeTruckCount, _queuedForLoading);

            PurgeQueueAboveIndex(_weighingQueue, activeTruckCount, _queuedForWeighing);

            PurgeQueueAboveIndex(_travelingQueue, activeTruckCount, _queuedForTraveling);

        }



        private static void PurgeQueueAboveIndex(Int32Queue queue, int activeTruckCount, bool[] queuedFlags)

        {

            for (int i = queue.Count - 1; i >= 0; i--)

            {

                int truckIndex = queue[i];

                if (truckIndex < activeTruckCount)

                    continue;



                queue.RemoveAtSwapBack(i);

                queuedFlags[truckIndex] = false;

            }

        }



        private void AdvanceTimeStep(int activeTruckCount)

        {

            if (activeTruckCount <= 0 || _coalRemaining <= 0)

                return;



            int timeStepMinutes = CalculateMinimumTimeStep(activeTruckCount);

            if (timeStepMinutes == int.MaxValue)

                throw new InvalidOperationException("Simulation could not advance: no valid time step for the current fleet state.");



            EnqueueTrucksForCurrentState(activeTruckCount);

            ProcessLoadingQueue(timeStepMinutes);

            ProcessWeighingQueue(timeStepMinutes);

            ProcessTravelingQueue(timeStepMinutes);

            _totalTimeMinutes += timeStepMinutes;

        }



        private void EnqueueTrucksForCurrentState(int activeTruckCount)

        {

            for (int i = 0; i < activeTruckCount; i++)

            {

                Truck truck = _trucks[i];

                if (IsExcludedFromTimeStep(truck))

                    continue;



                switch (truck.State)

                {

                    case TruckState.Loading:

                        if (!_queuedForLoading[i])

                        {

                            _queuedForLoading[i] = true;

                            _loadingQueue.Enqueue(i);

                            truck.AwaitingResourceService = true;

                        }

                        break;

                    case TruckState.Weighing:

                        if (!_queuedForWeighing[i])

                        {

                            _queuedForWeighing[i] = true;

                            _weighingQueue.Enqueue(i);

                            truck.AwaitingResourceService = true;

                        }

                        break;

                    case TruckState.Traveling:

                        if (!_queuedForTraveling[i])

                        {

                            _queuedForTraveling[i] = true;

                            _travelingQueue.Enqueue(i);

                            truck.BeginTraveling(_sampler, _parameters.TravelingDistribution);

                        }

                        break;

                }

            }

        }



        private int CalculateMinimumTimeStep(int activeTruckCount)

        {

            int min = int.MaxValue;



            for (int i = 0; i < activeTruckCount; i++)

            {

                Truck truck = _trucks[i];

                if (IsExcludedFromTimeStep(truck))

                    continue;



                min = Math.Min(min, PositiveOrMax(truck.LoadingTimeLeft));

                min = Math.Min(min, PositiveOrMax(truck.WeighingTimeLeft));

                min = Math.Min(min, PositiveOrMax(truck.TravelingTimeLeft));



                if (truck.IsReadyForQueue || truck.AwaitingResourceService)

                    min = Math.Min(min, 1);

            }



            return min;

        }



        private static bool IsExcludedFromTimeStep(Truck truck) =>

            truck.LoadingTimeLeft != 0 && truck.WeighingTimeLeft != 0 && truck.TravelingTimeLeft != 0;



        private static int PositiveOrMax(int value) =>

            value != 0 ? value : int.MaxValue;



        private static void DecrementTimer(ref int timeLeft, int timeStepMinutes)

        {

            if (timeLeft <= 0)

                return;



            timeLeft = Math.Max(0, timeLeft - timeStepMinutes);

        }



        private void EnsureLoadingStarted(Truck truck)

        {

            if (truck.LoadingTimeLeft > 0)

                return;



            truck.BeginLoading(_sampler, _parameters.LoadingDistribution);

        }



        private void EnsureWeighingStarted(Truck truck)

        {

            if (truck.WeighingTimeLeft > 0)

                return;



            truck.BeginWeighing(_sampler, _parameters.WeighingDistribution);

        }



        private void ProcessLoadingQueue(int timeStepMinutes)

        {

            int slots = Math.Min((int)_parameters.LoaderCount, _loadingQueue.Count);

            for (int i = 0; i < slots; i++)

            {

                int truckIndex = _loadingQueue[i];

                Truck truck = _trucks[truckIndex];

                EnsureLoadingStarted(truck);



                int timeLeft = truck.LoadingTimeLeft;

                DecrementTimer(ref timeLeft, timeStepMinutes);

                truck.LoadingTimeLeft = timeLeft;



                if (truck.LoadingTimeLeft <= 0)

                {

                    truck.AdvanceToNextState();

                    _queuedForLoading[truckIndex] = false;

                    _loadingQueue.RemoveAtSwapBack(i);

                    i--;

                    slots--;

                }



                _totalTimeTruck += timeStepMinutes;

                _totalTimeLoader += timeStepMinutes;

            }

        }



        private void ProcessWeighingQueue(int timeStepMinutes)

        {

            int slots = Math.Min((int)_parameters.ScalerCount, _weighingQueue.Count);

            for (int i = 0; i < slots; i++)

            {

                int truckIndex = _weighingQueue[i];

                Truck truck = _trucks[truckIndex];

                EnsureWeighingStarted(truck);



                int timeLeft = truck.WeighingTimeLeft;

                DecrementTimer(ref timeLeft, timeStepMinutes);

                truck.WeighingTimeLeft = timeLeft;



                if (truck.WeighingTimeLeft <= 0)

                {

                    truck.AdvanceToNextState();

                    _queuedForWeighing[truckIndex] = false;

                    _weighingQueue.RemoveAtSwapBack(i);

                    i--;

                    slots--;

                }



                _totalTimeTruck += timeStepMinutes;

                _totalTimeScaler += timeStepMinutes;

            }

        }



        private void ProcessTravelingQueue(int timeStepMinutes)

        {

            for (int i = 0; i < _travelingQueue.Count; i++)

            {

                int truckIndex = _travelingQueue[i];

                Truck truck = _trucks[truckIndex];



                int timeLeft = truck.TravelingTimeLeft;

                DecrementTimer(ref timeLeft, timeStepMinutes);

                truck.TravelingTimeLeft = timeLeft;



                if (truck.TravelingTimeLeft <= 0)

                {

                    truck.AdvanceToNextState();

                    _queuedForTraveling[truckIndex] = false;

                    _travelingQueue.RemoveAtSwapBack(i);

                    _coalRemaining = Math.Max(0f, _coalRemaining - _parameters.TruckLoadVolume);

                    i--;

                }



                _totalTimeTruck += timeStepMinutes;

            }

        }

    }

}


