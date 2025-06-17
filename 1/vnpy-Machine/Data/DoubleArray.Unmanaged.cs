using System;
using System.Runtime.CompilerServices;

namespace FinanceSharp {
    public abstract unsafe partial class DoubleArray {
        /// <summary>
        ///     The bytes size of this array.
        /// </summary>
        public int SizeOf => sizeof(double) * LinearLength;

        /// <summary>
        ///     The total number of double values in the array.
        /// </summary>
        public int LinearLength => Count * Properties;

        /// <summary>
        ///     Wraps this DoubleArray with <see cref="Span{T}"/>.
        /// </summary>
        /// <remarks>Best practice to use AsDoubleSpan safely is first using fixed operator on the array and then access <see cref="AsDoubleSpan"/>. See examples.</remarks>
        /// <example>
        /// <code>
        ///     fixed (double* _ = doublearray) {
        ///         doublearray.AsDoubleSpan.Fill(10d);
        ///         double[] ret = doublearray.AsDoubleSpan.ToArray();
        ///     }
        /// </code>
        /// </example>
        public virtual Span<double> AsDoubleSpan => new Span<double>(Unsafe.AsPointer(ref GetPinnableReference()), LinearLength);

        /// <summary>
        ///     Provides a pinnable reference for fixing a <see cref="DoubleArray"/>.
        /// </summary>
        public abstract ref double GetPinnableReference();

        /// <summary>
        ///     Provides a pinnable reference for fixing a <see cref="DoubleArray"/> at a specific index (of <see cref="Count"/> dimension).
        /// </summary>
        /// <example>
        /// <code>
        ///     fixed (double* pointer = &amp;arr.GetPinnableReference(0)) {
        ///         //use pointer to your needs
        ///     }
        /// </code>
        /// </example>
        public abstract ref double GetPinnableReference(int index);

        /// <summary>
        ///     Copies content of this array to <paramref name="target"/>.
        /// </summary>
        /// <param name="target">The target array, must be matching <see cref="LinearLength"/> with this.</param>
        public virtual void CopyTo(DoubleArray target) {
            fixed (double* _ = this, __ = target) {
                //fix to prevent gc from moving them during the operation.
                AsDoubleSpan
                    .CopyTo(target.AsDoubleSpan);
            }
        }

        /// <summary>
        ///     Copies content of this array to <paramref name="target"/>.
        /// </summary>
        /// <param name="target">The target array, must be matching <see cref="LinearLength"/> with this.</param>
        /// <param name="bytes">The number of bytes to copy.</param>
        public virtual void CopyTo(void* target, int bytes) {
            fixed (double* src = this) 
                Unsafe.CopyBlock(target, src, (uint) Math.Min(LinearLength * sizeof(double), bytes));
        }

        /// <summary>
        ///     Copies content of this array to <paramref name="target"/>.
        /// </summary>
        /// <param name="target">The target array, must be matching <see cref="LinearLength"/> with this.</param>
        public virtual void CopyTo(void* target) {
            fixed (double* src = this) 
                Unsafe.CopyBlock(target, src, (uint) LinearLength * sizeof(double));
        }

        /// <summary>
        ///     Fills the contents of this <see cref="DoubleArray"/> with the given value.
        /// </summary>
        public void Fill(double value) {
            int length = LinearLength;

            if (length == 0)
                return;

            unchecked {
                fixed (double* address = this) {
                    // Simple loop unrolling
                    int i = 0;
                    for (; i < (length & ~15); i += 16) {
                        address[i + 0] = value;
                        address[i + 1] = value;
                        address[i + 2] = value;
                        address[i + 3] = value;
                        address[i + 4] = value;
                        address[i + 5] = value;
                        address[i + 6] = value;
                        address[i + 7] = value;
                        address[i + 8] = value;
                        address[i + 9] = value;
                        address[i + 10] = value;
                        address[i + 11] = value;
                        address[i + 12] = value;
                        address[i + 13] = value;
                        address[i + 14] = value;
                        address[i + 15] = value;
                    }

                    for (; i < (length & ~7); i += 8) {
                        address[i + 0] = value;
                        address[i + 1] = value;
                        address[i + 2] = value;
                        address[i + 3] = value;
                        address[i + 4] = value;
                        address[i + 5] = value;
                        address[i + 6] = value;
                        address[i + 7] = value;
                    }

                    if (i < (length & ~3)) {
                        address[i + 0] = value;
                        address[i + 1] = value;
                        address[i + 2] = value;
                        address[i + 3] = value;
                        i += 4;
                    }

                    for (; i < length; i++) {
                        address[i] = value;
                    }
                }
            }
        }

        /// <summary>
        ///     Fills the contents of given property with the given value.
        /// </summary>
        public void Fill(int property, double value) {
            int length = LinearLength - (Properties - property - 1);

            if (length == 0)
                return;

            unchecked {
                fixed (double* address = this) {
                    // Simple loop unrolling
                    for (int i = length - 1; i >= property; i -= Properties)
                        address[i] = value;
                }
            }
        }
    }
}