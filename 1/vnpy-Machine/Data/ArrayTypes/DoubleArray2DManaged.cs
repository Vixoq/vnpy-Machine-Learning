/*
 * All Rights reserved to Ebby Technologies LTD @ Eli Belash, 2020.
 * Original code by QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FinanceSharp.Delegates;
using FinanceSharp.Exceptions;

namespace FinanceSharp {
    public unsafe class DoubleArray2DManaged : DoubleArray {
        protected internal double[,] values;

        /// <param name="values">The structure value that'll be wrapped.</param>
        public DoubleArray2DManaged(double[,] values) {
            this.values = values;
            Count = values.GetLength(0);
            Properties = values.GetLength(1);
        }

        /// <param name="values">The structure value that'll be wrapped.</param>
        /// <remarks>This constructor copies given <paramref name="values"/> to a <see cref="double[Count, Properties]"/></remarks>
        public DoubleArray2DManaged(double[][] values) {
            this.values = ToMultiDimArray(values);
            Count = this.values.GetLength(0);
            Properties = this.values.GetLength(1);
        }

        public DoubleArray2DManaged(int count, int properties) {
            this.values = new double[count, properties];
            Count = count;
            Properties = properties;
        }

        public DoubleArray2DManaged() {
            this.values = new double[1, 1];
            Count = 1;
            Properties = 1;
        }

        public override int Count { get; protected internal set; }

        public override int Properties { get; protected internal set; }

        /// <summary>
        ///     Returns a reference to the unmanaged array stored internally.
        /// </summary>
        public ref double[,] InternalArray => ref values;

        public override void ForEach(ReferenceForFunctionHandler function) {
            fixed (double* ptr = values) {
                var len = values.Length;
                for (int i = 0; i < len; i++) {
                    function(ref ptr[i]);
                }
            }
        }

        /// <summary>
        ///     Pins this current <see cref="InternalArray"/> with <see cref="DoubleArrayPinned2DManaged"/>.
        /// </summary>
        /// <param name="copy">Should clone <see cref="InternalArray"/> passed to with <see cref="DoubleArrayPinned2DManaged"/>?</param>
        /// <returns></returns>
        public DoubleArrayPinned2DManaged ToPinned(bool copy) {
            return new DoubleArrayPinned2DManaged(copy ? (double[,]) values.Clone() : values);
        }

        public override double this[int property] {
            get {
                AssertTrue(IsScalar, "Scalar only overload was called but the array is not scalar.");
                AssertTrue(property >= 0 && property < Properties, "Property index is out of range.");
                return values[0, property];
            }
            set {
                AssertTrue(IsScalar, "Scalar only overload was called but the array is not scalar.");
                AssertTrue(property >= 0 && property < Properties, "Property index is out of range.");
                values[0, property] = value;
            }
        }

        public override double this[int index, int property] {
            get {
                AssertTrue(index < Count, "Index is out of range.");
                AssertTrue(property >= 0 && property < Properties, "Property index is out of range.");
                return values[index, property];
            }
            set {
                AssertTrue(index < Count, "Index is out of range.");
                AssertTrue(property >= 0 && property < Properties, "Property index is out of range.");
                values[index, property] = value;
            }
        }

        public override double GetLinear(int offset) {
            AssertTrue(offset >= 0 && offset < LinearLength, "Offset is out of range.");
            fixed (double* ptr = values) {
                return ptr[offset];
            }
        }

        public override void SetLinear(int offset, double value) {
            AssertTrue(offset >= 0 && offset < LinearLength, "Offset is out of range.");
            fixed (double* ptr = values) {
                ptr[offset] = value;
            }
        }

        protected internal override bool? IsEqualExactlyTo(DoubleArray other) {
            if (other is DoubleArray2DManaged o) {
                if (o.values.Equals(values)) {
                    var othervals = o.values;
                    for (int i = 0; i < Count; i++) {
                        for (int j = 0; j < Properties; j++) {
                            if (othervals[i, j] != values[i, j] && double.IsNaN(othervals[i, j]) != double.IsNaN(values[i, j]))
                                return false;
                        }
                    }

                    return true;
                }
            }

            return null;
        }

        protected internal override int ComputeHashCode() {
            return values.GetHashCode();
        }

        public override Span<double> AsDoubleSpan => new Span<double>(Unsafe.AsPointer(ref values[0, 0]), LinearLength);

        public override ref double GetPinnableReference() {
            return ref values[0, 0];
        }

        public override ref double GetPinnableReference(int index) {
            return ref values[index, 0];
        }

        public override double[,] To2DArray() {
            return (double[,]) values.Clone();
        }

        public override DoubleArray Reshape(int count, int properties, bool copy = true) {
            if (LinearLength != (count * properties))
                throw new ReshapeException($"Unable to reshape ({Count}, {Properties}) to ({count}, {properties})");

            var data = new double[count, properties];
            fixed (double* src = values, dst = data)
                Unsafe.CopyBlock(dst, src, (uint) (sizeof(double) * LinearLength));

            if (copy) {
                return new DoubleArray2DManaged(data);
            } else {
                values = data;
                Count = count;
                Properties = properties;
                return this;
            }
        }

        public override DoubleArray Clone() {
            return new DoubleArray2DManaged((double[,]) values.Clone());
        }

        [SuppressMessage("ReSharper", "SuggestVarOrType_SimpleTypes")]
        [SuppressMessage("ReSharper", "SuggestVarOrType_Elsewhere")]
        [MethodImpl((MethodImplOptions) 512)]
        internal static double[,] ToMultiDimArray(double[][] data) {
            unsafe {
                int len1 = data.Length;
                int len2 = data[0].Length;

                var @out = new double[len1, len2];

                int stride1 = len2;
                fixed (double* addr = @out) {
                    if (len1 * len2 > 500_000) {
                        var addr_ = addr;
                        Parallel.For(0, len1, i1 => {
                            double* dst1 = addr_ + i1 * stride1;
                            double[] src1 = data[i1];
                            for (long i2 = 0; i2 < len2; i2++) {
                                dst1[i2] = src1[i2];
                            }
                        });
                    } else {
                        for (long i1 = 0; i1 < len1; i1++) {
                            double* dst1 = addr + i1 * stride1;
                            double[] src1 = data[i1];
                            for (long i2 = 0; i2 < len2; i2++) {
                                dst1[i2] = src1[i2];
                            }
                        }
                    }
                }

                return @out;
            }
        }

        #region Blunt Overloads

        public override double Value {
            get => this.values[0, 0];
            set => this.values[0, 0] = value;
        }

        public override double Close {
            get => this.values[0, 0];
            set => this.values[0, 0] = value;
        }

        #endregion
    }
}