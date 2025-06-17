using FinanceSharp.Exceptions;

namespace FinanceSharp {
    public abstract unsafe partial class DoubleArray {
        /// <summary>
        ///     Is this array a scalar by checking if <see cref="Count"/> is 1. (regardless to how many properties there are).
        /// </summary>
        public virtual bool IsScalar => Count == 1;

        /// <summary>
        ///     Reshapes this <see cref="DoubleArray"/> to a shape of your choice, the shape must be 
        /// </summary>
        /// <param name="count">The count of items in this array.</param>
        /// <param name="properties">Number of properties for every item in this array.</param>
        /// <param name="copy">Should a copy be reshaped?</param>
        /// <exception cref="ReshapeException">When unable to reshape to reshape: <see cref="LinearLength"/> != (<paramref name="count"/> * <paramref name="properties"/>) then throws </exception>
        public abstract DoubleArray Reshape(int count, int properties, bool copy = true);

        /// <summary>
        ///     Slices (or wraps with a slice wrapper) the <see cref="Count"/> dimension.
        /// </summary>
        /// <param name="start">Start of interval. The interval includes this value. The default start value is 0.</param>
        /// <param name="stop">End of interval. The interval does not include this value, except in some cases where step is not an integer and floating point round-off affects the length of out.</param>
        /// <returns>Returns a sliced array shaped (newCount, <see cref="Properties"/>)</returns>
        /// <remarks>This slicing mechanism is similar to numpy's slice and will behave like the following: <code>thisArray[start:stop:1, :]</code></remarks>
        public virtual DoubleArray Slice(int start, int stop) {
            AssertTrue(start < Count, "Start index is out of range.");
            AssertTrue(stop <= Count, "Stop index is out of range.");
            return new SlicedDoubleArray(this, start, stop);
        }

        /// <summary>
        ///     Slices (or wraps with a slice wrapper) the <see cref="Count"/> dimension.
        /// </summary>
        /// <param name="stop">End of interval. The interval does not include this value, except in some cases where step is not an integer and floating point round-off affects the length of out.</param>
        /// <returns>Returns a sliced array shaped (newCount, <see cref="Properties"/>)</returns>
        /// <remarks>This slicing mechanism is similar to numpy's slice and will behave like the following: <code>thisArray[start:stop:1, :]</code></remarks>
        public DoubleArray Slice(int stop) {
            return Slice(0, stop);
        }

        /// <summary>
        ///     Slices (or wraps with a slice wrapper) the <see cref="Count"/> dimension.
        /// </summary>
        /// <param name="index">The index to slice from this <see cref="DoubleArray"/>.</param>
        /// <param name="copy">Should the returned slice be copied.</param>
        /// <returns>Returns a sliced array shaped (newCount, <see cref="Properties"/>)</returns>
        /// <remarks>This slicing mechanism is similar to numpy's slice and will behave like the following: <code>thisArray[start:stop:1, :]</code></remarks>
        public virtual DoubleArray SliceIndex(int index, bool copy = false) {
            AssertTrue(index >= 0 && index < Count, "Index is out of range.");
            if (copy && this.Properties == 1) {
                return new DoubleArrayScalar(this[index, 0]);
            } else {
                var ret = new SlicedDoubleArray(this, index, index + 1);
                if (copy)
                    return ret.Clone();
                return ret;
            }
        }
    }
}