namespace GeneticAlgorithm.Core.Simulation
{
    /// <summary>
    /// Fixed-capacity LIFO-friendly queue of truck indices using an array (no per-element boxing).
    /// Supports O(1) enqueue and swap-remove dequeue.
    /// </summary>
    internal sealed class Int32Queue
    {
        private readonly int[] _indices;
        private int _count;

        /// <summary>
        /// Creates a queue that can hold up to <paramref name="capacity"/> truck indices.
        /// </summary>
        public Int32Queue(int capacity)
        {
            _indices = new int[capacity];
        }

        /// <summary>Number of truck indices currently in the queue.</summary>
        public int Count => _count;

        /// <summary>
        /// Gets the truck index at the given position without removing it.
        /// </summary>
        public int this[int position] => _indices[position];

        /// <summary>
        /// Appends a truck index to the end of the queue.
        /// </summary>
        public void Enqueue(int truckIndex)
        {
            _indices[_count++] = truckIndex;
        }

        /// <summary>
        /// Removes the entry at <paramref name="position"/> by swapping it with the last element.
        /// </summary>
        public void RemoveAtSwapBack(int position)
        {
            _indices[position] = _indices[--_count];
        }

        /// <summary>Clears all entries (retains allocated capacity).</summary>
        public void Clear() => _count = 0;
    }
}
