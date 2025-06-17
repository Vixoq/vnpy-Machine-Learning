namespace FinanceSharp {
    /// <summary>
    ///     Provides a wrapper to <see cref="DoubleArrayUnmanaged"/> that slices by shifting <see cref="DoubleArrayUnmanaged.Address"/> and manipulating start and stop.
    /// </summary>
    public unsafe class SlicedDoubleArrayUnmanaged : DoubleArrayUnmanaged {
        /// A reference to internal SlicedArray to prevent GC from automatically collecting it.
        protected DoubleArrayUnmanaged internalArray;
        protected int start;
        protected int stop;

        /// <summary>
        ///     Provides access to the internal array that is being sliced by this class.
        /// </summary>
        public DoubleArray InternalArray => internalArray;

        /// <summary>
        ///     End of interval. The interval does not include this value, except in some cases where step is not an integer and floating point round-off affects the length of out.
        /// </summary>
        public int Stop => stop;

        /// <summary>
        ///     Start of interval. The interval includes this value. The default start value is 0.
        /// </summary>
        public int Start => start;

        public SlicedDoubleArrayUnmanaged(DoubleArrayUnmanaged slicedArray, int start, int stop) : base(slicedArray.Address + start, stop - start, slicedArray.Properties, false, null) {
            internalArray = slicedArray;
            this.start = start;
            this.stop = stop;
        }
    }
}