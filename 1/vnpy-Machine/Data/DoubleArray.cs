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
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using FinanceSharp.Exceptions;

namespace FinanceSharp {
    /// <summary>
    ///     A block of memory of doubles represented in two dimensions.
    /// </summary>
    /// <remarks>
    ///     First dimension of this array is <see cref="Count"/> and 2nd dimension is <see cref="Properties"/>,
    ///     which for a OHLC trade bar would be (n, 4) where n is the number of trade bars in the memory block.
    /// </remarks>
    public abstract unsafe partial class DoubleArray : ICloneable, IDisposable {
        /// The number of items in this array, each having n <see cref="Properties"/>.
        public abstract int Count { get; protected internal set; }

        /// The count of properties for every 
        public abstract int Properties { get; protected internal set; }

        /// <param name="count">The number of items in this array.</param>
        /// <param name="properties">How many properties typed double are for every <see cref="count"/></param>
        protected DoubleArray(int count, int properties) {
            Count = count;
            Properties = properties;
        }

        /// <param name="values"></param>
        /// <param name="properties">How many properties typed double are for every <see cref="count"/></param>
        protected DoubleArray(double[] values, int properties = 1) : this(values.Length / properties, properties) {
            AssertTrue(values.Length % properties == 0, "It has to be evenly divided by the properties count.");
        }

        protected DoubleArray() { }

        /// <summary>
        ///     Clones current DoubleArray.
        /// </summary>
        /// <returns>A new copy of this.</returns>
        public abstract DoubleArray Clone();

        /// <summary>
        ///     Converts this <see cref="DoubleArray"/> to a linear <see cref="double"/>.
        /// </summary>
        /// <returns></returns>
        public virtual double[] ToArray() {
            fixed (double* _ = this) //fix to prevent gc from moving them during the operation.
                return AsDoubleSpan.ToArray();
        }

        /// <summary>
        ///     Converts current array to an Array of type <typeparamref name="TDestStruct"/>.
        /// </summary>
        /// <typeparam name="TDestStruct"></typeparam>
        /// <param name="method">ArrayConversionMethod,</param>
        /// <param name="propertiesPerItem">Used for ArrayConversionMethod.Cast: describes how many doubles to copy to and per TDestStruct. If -1 is specified then sizeof(TDestStruct)/sizeof(double) will be used.<br></br></param>
        public virtual unsafe TDestStruct[] ToArray<TDestStruct>(ArrayConversionMethod method, int propertiesPerItem = -1) where TDestStruct : unmanaged, DataStruct {
            if (typeof(TDestStruct) == typeof(double))
                fixed (double* _ = this)
                    return (TDestStruct[]) (object) AsDoubleSpan.ToArray();
            var totalBytes = this.SizeOf;
            if (method == ArrayConversionMethod.Reinterpret) {
                if (totalBytes % sizeof(TDestStruct) != 0)
                    throw new ArgumentException($"Can't reinterpret to TDestStruct because: totalBytes ({totalBytes}) % sizeof(TDestStruct) ({sizeof(TDestStruct)}) != 0 ({totalBytes % sizeof(TDestStruct)})");
                fixed (double* _ = this)
                    return MemoryMarshal.Cast<double, TDestStruct>(AsDoubleSpan).ToArray();
            } else if (method == ArrayConversionMethod.Cast) {
                if (propertiesPerItem == -1)
                    propertiesPerItem = sizeof(TDestStruct) / sizeof(double);
                if (totalBytes % (propertiesPerItem * sizeof(double)) != 0)
                    throw new ArgumentException($"Can't reinterpret to TDestStruct because: totalBytes ({totalBytes}) % (propertiesPerItem * sizeof(double)) (({propertiesPerItem} * sizeof(double))) != 0 ({totalBytes % sizeof(TDestStruct)})");
                var ret = new TDestStruct[(totalBytes) / (propertiesPerItem * sizeof(double))];
                TDestStruct tmp = new TDestStruct();
                double* tmpPtr = (double*) &tmp;
                var len = Count * (Properties / propertiesPerItem);
                for (int i = 0, linearI = 0; i < len; i++) {
                    for (int j = 0; j < propertiesPerItem; j++, linearI++) {
                        tmpPtr[j] = this.GetLinear(linearI);
                    }

                    ret[i] = tmp; //copies because tmp is struct.
                }

                return ret;
            } else {
                throw new NotSupportedException(method.ToString());
            }
        }

        /// <summary>
        ///     Copies this array into a multi-dimensional array, double[,].
        /// </summary>
        /// <returns></returns>
        public virtual double[,] To2DArray() {
            var ret = new double[Count, Properties];
            fixed (double* src = this, dst = ret) {
                Unsafe.CopyBlock(src, dst, (uint) (sizeof(double) * ret.Length));
                return ret;
            }
        }

        /// <summary>
        ///     Asserts for DEBUG runs..
        /// </summary>
        [Conditional("DEBUG"), DebuggerHidden]
        protected void AssertTrue(bool condition) {
            if (!condition)
                throw new Exception();
        }

        /// <summary>
        ///     Asserts for DEBUG runs..
        /// </summary>
        [Conditional("DEBUG"), DebuggerHidden]
        protected void AssertTrue(bool condition, string reason) {
            if (!condition)
                throw new Exception(reason);
        }

        object ICloneable.Clone() {
            return Clone();
        }

        public override string ToString() {
            var sb = new StringBuilder();
            if (this.Properties == 1) {
                sb.Append($"[");
                for (int i = 0; i < this.Count; i++) {
                    sb.Append($"{this[i, 0]:0.#####}");
                    if (i != this.Count - 1) {
                        sb.Append($", ");
                    }
                }

                sb.Append($"]");
            } else {
                sb.Append($"[");
                for (int i = 0; i < this.Count; i++) {
                    sb.Append($"[");
                    for (int j = 0; j < this.Properties; j++) {
                        sb.Append($"{this[i, j]:0.#####}");
                        if (j != this.Properties - 1) sb.Append($", ");
                    }

                    sb.Append($"]");

                    if (i != this.Count - 1) sb.Append($", ");
                }

                sb.Append($"]");
            }

            return sb.ToString();
        }
    }
}