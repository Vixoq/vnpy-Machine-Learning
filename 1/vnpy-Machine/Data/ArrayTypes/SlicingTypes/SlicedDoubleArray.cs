using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using FinanceSharp.Delegates;
using FinanceSharp.Exceptions;

namespace FinanceSharp {
    /// <summary>
    ///     Provides a wrapping <see cref="DoubleArray"/> to contiguously slice.
    /// </summary>
    public class SlicedDoubleArray : DoubleArray {
        //TODO: Add this class as a unit test inherieting base test class for double arrays.
        protected DoubleArray internalArray;
        protected int start;
        protected int stop;

        public override int Count { get; protected internal set; }

        public override int Properties { get; protected internal set; }

        public SlicedDoubleArray(DoubleArray slicedArray, int start, int stop) : base(stop - start, slicedArray.Properties) {
            this.internalArray = slicedArray;
            this.start = start;
            this.stop = stop;
        }

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected int AdjustIndex(int index) {
            AssertTrue(index < stop, "Index out of range");
            return checked(start + index);
        }

        /// <summary>
        ///     Provides a pinnable reference for fixing a <see cref="DoubleArray"/>.
        /// </summary>
        public override ref double GetPinnableReference() {
            return ref internalArray.GetPinnableReference(start); //start == AdjustIndex(0)
        }

        /// <summary>
        ///     Provides a pinnable reference for fixing a <see cref="DoubleArray"/> at a specific index (of <see cref="DoubleArray.Count"/> dimension).
        /// </summary>
        /// <example>
        /// <code>
        ///     fixed (double* pointer = &amp;arr.GetPinnableReference(0)) {
        ///         //use pointer to your needs
        ///     }
        /// </code>
        /// </example>
        public override ref double GetPinnableReference(int index) {
            return ref internalArray.GetPinnableReference(AdjustIndex(index));
        }

        /// <summary>
        ///     Reshapes this <see cref="DoubleArray"/> to a shape of your choice, the shape must be 
        /// </summary>
        /// <param name="count">The count of items in this array.</param>
        /// <param name="properties">Number of properties for every item in this array.</param>
        /// <param name="copy">Should a copy be reshaped?</param>
        /// <exception cref="ReshapeException">When unable to reshape to reshape: <see cref="DoubleArray.LinearLength"/> != (<paramref name="count"/> * <paramref name="properties"/>) then throws </exception>
        /// <remarks>A sliced array always copies.</remarks>
        public override DoubleArray Reshape(int count, int properties, bool copy = true) {
            return Clone().Reshape(count, properties, false);
        }

        /// <summary>
        ///     Clones current DoubleArray.
        /// </summary>
        /// <returns>A new copy of this.</returns>
        public override unsafe DoubleArray Clone() {
            var ret = new DoubleArray2DManaged(Count, Properties);
            fixed (double* src = internalArray, dst = ret)
                Unsafe.CopyBlock(dst, src + start, (uint) (sizeof(double) * LinearLength));

            return ret;
        }

        public override unsafe void ForEach(ReferenceForFunctionHandler function) {
            fixed (double* src = internalArray) {
                for (int i = start; i < stop; i++) {
                    function(ref src[i]);
                }
            }
        }

        /// <summary>
        ///     A comparison method used when <see cref="DoubleArray.Equals(FinanceSharp.DoubleArray)"/> is called from <see cref="IEquatable{T}"/> to compare same types of <see cref="DoubleArray"/>.
        /// </summary>
        /// <param name="other">An DoubleArray to compare to this.</param>
        /// <returns>Are <see cref="this"/> equals to <see cref="other"/>. Null returned when test was not performed.</returns>
        protected internal override bool? IsEqualExactlyTo(DoubleArray other) {
            return internalArray.IsEqualExactlyTo(other);
        }

        /// <summary>
        ///     Additional hashcode for inherieted classes.
        /// </summary>
        /// <returns></returns>
        protected internal override int ComputeHashCode() {
            return unchecked(internalArray.ComputeHashCode() ^ 397 * start ^ 397 * stop);
        }

        /// <summary>
        ///     Scalar overload.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public override double this[int property] {
            get => internalArray[start, property];
            set => internalArray[start, property] = value;
        }

        public override double this[int index, int property] {
            get => internalArray[start + index, property];
            set => internalArray[start + index, property] = value;
        }
    }
}