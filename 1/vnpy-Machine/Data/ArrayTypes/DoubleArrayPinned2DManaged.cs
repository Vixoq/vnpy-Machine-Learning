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
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace FinanceSharp {
    public unsafe class DoubleArrayPinned2DManaged : DoubleArrayUnmanaged {
        protected internal double[,] values;
        protected internal GCHandle gcHandle;

        /// <param name="values">The structure value that'll be wrapped.</param>
        public DoubleArrayPinned2DManaged(double[,] values) : this(values, GCHandle.Alloc(values, GCHandleType.Pinned)) { }

        /// <param name="values">The structure value that'll be wrapped.</param>
        /// <remarks>This constructor copies given <paramref name="values"/> to a double[Count, Properties]</remarks>
        public DoubleArrayPinned2DManaged(double[][] values) : this(DoubleArray2DManaged.ToMultiDimArray(values)) { }

        public DoubleArrayPinned2DManaged(int count, int properties) : this(new double[count, properties]) { }

        public DoubleArrayPinned2DManaged() : this(new double[1, 1]) { }

        /// <param name="values">The structure value that'll be wrapped.</param>
        /// <param name="handle">The <see cref="GCHandle"/> of <paramref name="values"/>.</param>
        public DoubleArrayPinned2DManaged(double[,] values, GCHandle handle) : base((double*) handle.AddrOfPinnedObject(), values.GetLength(0), values.GetLength(1), false, () => _disposer(handle)) {
            if (!handle.IsAllocated)
                throw new ArgumentNullException(nameof(handle));

            this.values = values;
            this.gcHandle = handle;
        }

        private static void _disposer(GCHandle handle) {
            DisposerThread.Enqueue(handle);
        }

        /// <summary>
        ///     Returns a reference to the unmanaged array stored internally.
        /// </summary>
        public double[,] InternalArray => values;

        protected internal override bool? IsEqualExactlyTo(DoubleArray other) {
            if (other is DoubleArray2DManaged o) {
                if (o.values.Equals(values)) {
                    var othervals = o.values;
                    for (int i = 0; i < Count; i++) {
                        for (int j = 0; j < Properties; j++) {
                            if (othervals[i, j] != values[i, j])
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

        public override DoubleArray Clone() {
            return new DoubleArrayPinned2DManaged((double[,]) values.Clone());
        }

        public override double[,] To2DArray() {
            return (double[,]) values.Clone();
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