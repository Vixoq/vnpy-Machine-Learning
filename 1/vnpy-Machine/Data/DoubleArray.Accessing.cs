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

using System.Runtime.CompilerServices;

namespace FinanceSharp {
    public abstract unsafe partial class DoubleArray {
        public virtual double Value {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this[0];
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => this[0] = value;
        }

        public virtual double Close {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                AssertTrue(Properties >= Constants.CloseIdx + 1);
                return this[Constants.CloseIdx];
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set {
                AssertTrue(Properties >= Constants.CloseIdx + 1);
                this[Constants.CloseIdx] = value;
            }
        }

        public virtual double Open {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                AssertTrue(Properties >= Constants.OpenIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.OpenIdx");
                return this[Constants.OpenIdx];
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set {
                AssertTrue(Properties >= Constants.OpenIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.OpenIdx");
                this[Constants.OpenIdx] = value;
            }
        }

        public virtual double High {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                AssertTrue(Properties >= Constants.HighIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.HighIdx");
                return this[Constants.HighIdx];
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set {
                AssertTrue(Properties >= Constants.HighIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.HighIdx");
                this[Constants.HighIdx] = value;
            }
        }

        public virtual double Low {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                AssertTrue(Properties >= Constants.LowIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.LowIdx");
                return this[Constants.LowIdx];
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set {
                AssertTrue(Properties >= Constants.LowIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.LowIdx");
                this[Constants.LowIdx] = value;
            }
        }

        public virtual double Volume {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                AssertTrue(Properties >= Constants.VolumeIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.VolumeIdx");
                return this[Constants.VolumeIdx];
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set {
                AssertTrue(Properties >= Constants.VolumeIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.VolumeIdx");
                this[Constants.VolumeIdx] = value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual double ValueAt(int index) {
            return this[index * Properties];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual double CloseAt(int index) {
            AssertTrue(index < Count);
            AssertTrue(Properties >= Constants.CloseIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.CloseIdx");
            return this[index * Properties + Constants.CloseIdx];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual double OpenAt(int index) {
            AssertTrue(index < Count);
            AssertTrue(Properties >= Constants.OpenIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.OpenIdx");
            return this[index * Properties + Constants.OpenIdx];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual double HighAt(int index) {
            AssertTrue(index < Count);
            AssertTrue(Properties >= Constants.HighIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.HighIdx");
            return this[index * Properties + Constants.HighIdx];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual double LowAt(int index) {
            AssertTrue(index < Count);
            AssertTrue(Properties >= Constants.LowIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.LowIdx");
            return this[index * Properties + Constants.LowIdx];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual double VolumeAt(int index) {
            AssertTrue(index < Count);
            AssertTrue(Properties >= Constants.VolumeIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.VolumeIdx");
            return this[index * Properties + Constants.VolumeIdx];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual double SetValueAt(int index, double value) {
            AssertTrue(index < Count);
            return this[index * Properties] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual double SetCloseAt(int index, double value) {
            AssertTrue(index < Count);
            AssertTrue(Properties >= Constants.CloseIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.CloseIdx");
            return this[index * Properties + Constants.CloseIdx] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual double SetOpenAt(int index, double value) {
            AssertTrue(index < Count);
            AssertTrue(Properties >= Constants.OpenIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.OpenIdx");
            return this[index * Properties + Constants.OpenIdx] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual double SetHighAt(int index, double value) {
            AssertTrue(index < Count);
            AssertTrue(Properties >= Constants.HighIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.HighIdx");
            return this[index * Properties + Constants.HighIdx] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual double SetLowAt(int index, double value) {
            AssertTrue(index < Count);
            AssertTrue(Properties >= Constants.LowIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.LowIdx");
            return this[index * Properties + Constants.LowIdx] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual double SetVolumeAt(int index, double value) {
            AssertTrue(index < Count);
            AssertTrue(Properties >= Constants.VolumeIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.VolumeIdx");
            return this[index * Properties + Constants.VolumeIdx] = value;
        }

        /// <summary>
        ///     Scalar overload.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public abstract double this[int property] { get; set; }

        public abstract double this[int index, int property] { get; set; }

        /// <summary>
        ///     Casts item at <paramref name="index"/> to <typeparamref name="TDestStruct"/>.
        /// </summary>
        /// <typeparam name="TDestStruct">The destination structure.</typeparam>
        /// <param name="index">The index</param>
        /// <returns></returns>
        public virtual TDestStruct Get<TDestStruct>(int index) where TDestStruct : unmanaged, DataStruct {
            AssertTrue(index >= 0 && index < Count, "Index is out of range.");
            if (Properties > DataStructInfo<TDestStruct>.Properties) {
                var ret = new TDestStruct();
                var dst = (double*) Unsafe.AsPointer(ref ret);
                var len = DataStructInfo<TDestStruct>.Properties;
                for (int i = 0; i < len; i++)
                    dst[i] = this[index, i];

                return ret;
            }

            fixed (double* ptr = this)
                return *(TDestStruct*) (ptr + index * Properties);
        }

        /// <summary>
        ///     Reads from this DoubleArray linearly regardless to shape.
        /// </summary>
        /// <param name="offset">Absolute offset</param>
        /// <returns>The value at given <paramref name="offset"/>.</returns>
        public virtual double GetLinear(int offset) {
            AssertTrue(offset >= 0 && offset < LinearLength, "Offset is out of range.");
            fixed (double* ptr = this)
                return ptr[offset];
        }

        /// <summary>
        ///     Writes to this DoubleArray linearly regardless to shape.
        /// </summary>
        /// <param name="offset">Absolute offset to set <paramref name="value"/> at.</param>
        /// <param name="value">The value to write</param>
        public virtual void SetLinear(int offset, double value) {
            AssertTrue(offset >= 0 && offset < LinearLength, "Offset is out of range.");
            fixed (double* ptr = this)
                ptr[offset] = value;
        }
    }
}