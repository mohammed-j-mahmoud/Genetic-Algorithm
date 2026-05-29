namespace GeneticAlgorithm.Core.Simulation

{

    /// <summary>

    /// A single truck moving through loading, weighing, and traveling phases.

    /// </summary>

    internal sealed class Truck

    {

        private TruckState _state = TruckState.Loading;



        /// <summary>Current phase in the operation cycle.</summary>

        public TruckState State

        {

            get => _state;

            set => _state = (int)value >= 3 ? TruckState.Loading : value;

        }



        /// <summary>Remaining loading time for the current visit (minutes).</summary>

        public int LoadingTimeLeft { get; set; }



        /// <summary>Remaining weighing time for the current visit (minutes).</summary>

        public int WeighingTimeLeft { get; set; }



        /// <summary>Remaining travel time for the current trip (minutes).</summary>

        public int TravelingTimeLeft { get; set; }



        /// <summary>True when queued for a loader or scale but service has not started yet.</summary>

        public bool AwaitingResourceService { get; set; }



        /// <summary>

        /// Returns true when the truck is idle and ready to enter a queue.

        /// </summary>

        public bool IsReadyForQueue =>

            !AwaitingResourceService

            && LoadingTimeLeft == 0

            && WeighingTimeLeft == 0

            && TravelingTimeLeft == 0;



        /// <summary>Assigns a sampled loading duration.</summary>

        public void BeginLoading(TimeDistributionSampler sampler, DiscreteDistribution distribution)

        {

            LoadingTimeLeft = sampler.SampleDurationMinutes(distribution);

            AwaitingResourceService = false;

        }



        /// <summary>Assigns a sampled weighing duration.</summary>

        public void BeginWeighing(TimeDistributionSampler sampler, DiscreteDistribution distribution)

        {

            WeighingTimeLeft = sampler.SampleDurationMinutes(distribution);

            AwaitingResourceService = false;

        }



        /// <summary>Assigns a sampled travel duration.</summary>

        public void BeginTraveling(TimeDistributionSampler sampler, DiscreteDistribution distribution)

        {

            TravelingTimeLeft = sampler.SampleDurationMinutes(distribution);

        }



        /// <summary>Advances to the next phase after the current operation completes.</summary>

        public void AdvanceToNextState()

        {

            AwaitingResourceService = false;

            State = (TruckState)((int)State + 1);

        }



        /// <summary>Clears timers and returns the truck to the loading phase.</summary>

        public void Reset()

        {

            _state = TruckState.Loading;

            LoadingTimeLeft = 0;

            WeighingTimeLeft = 0;

            TravelingTimeLeft = 0;

            AwaitingResourceService = false;

        }

    }

}


