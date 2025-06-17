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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using FinanceSharp.Delegates;
using FinanceSharp.Exceptions;

namespace FinanceSharp {
    public unsafe class DoubleArrayStruct<TStruct> : DoubleArray where TStruct : unmanaged, DataStruct {
        protected internal TStruct[] values;

        public override int Count { get; protected internal set; }

        public override int Properties {
            get => DataStructInfo<TStruct>.Properties;
            protected internal set => throw new NotSupportedException();
        }

        static DoubleArrayStruct() {
            //verify staticly that this struct indeed has only made from doubles
            if (DataStructInfo<TStruct>.DoubleFieldsCount != DataStructInfo<TStruct>.FieldsCount)
                throw new InvalidDoubleArrayStructException();
        }

        /// <param name="struct">The structure value that'll be wrapped.</param>
        public DoubleArrayStruct(params TStruct[] @struct) {
            values = @struct;
            // ReSharper disable once VirtualMemberCallInConstructor
            Count = @struct.Length;
        }

        public DoubleArrayStruct() {
            values = new TStruct[] {new TStruct()};
            // ReSharper disable once VirtualMemberCallInConstructor
            Count = 1;
        }

        public override void ForEach(ReferenceForFunctionHandler function) {
            ref var s = ref values;
            fixed (TStruct* struct_ptr = values) {
                var ptr = (double*) struct_ptr;
                var cnt = Properties * Count;
                for (int i = 0; i < cnt; i++) {
                    function(ref ptr[i]);
                }
            }
        }

        public override double this[int property] {
            get {
                AssertTrue(IsScalar, "Scalar only overload was called but the array is not scalar.");
                AssertTrue(property >= 0 && property < Properties, "Property index is out of range.");
                fixed (TStruct* ptr = values) {
                    return ((double*) ptr)[property];
                }
            }
            set {
                AssertTrue(IsScalar, "Scalar only overload was called but the array is not scalar.");
                AssertTrue(property >= 0 && property < Properties, "Property index is out of range.");
                fixed (TStruct* ptr = values) {
                    ((double*) ptr)[property] = value;
                }
            }
        }

        public override double this[int index, int property] {
            get {
                AssertTrue(index < Count, "Index is out of range.");
                AssertTrue(property >= 0 && property < Properties, "Property index is out of range.");
                return ((double*) Unsafe.AsPointer(ref values[index]))[property];
            }
            set {
                AssertTrue(index < Count, "Index is out of range.");
                AssertTrue(property >= 0 && property < Properties, "Property index is out of range.");
                ((double*) Unsafe.AsPointer(ref values[index]))[property] = value;
            }
        }

        public override TDestStruct Get<TDestStruct>(int index) {
            AssertTrue(index >= 0 && index < Count, "Index is out of range.");
            if (DataStructInfo<TDestStruct>.Properties > Properties)
                return base.Get<TDestStruct>(index);
            return *(TDestStruct*) Unsafe.AsPointer(ref values[index]);
        }

        public override double GetLinear(int offset) {
            AssertTrue(offset >= 0 && offset < LinearLength, "Offset is out of range.");
            fixed (TStruct* ptr = values) {
                return ((double*) ptr)[offset];
            }
        }

        public override void SetLinear(int offset, double value) {
            AssertTrue(offset >= 0 && offset < LinearLength, "Offset is out of range.");
            fixed (TStruct* ptr = values) {
                ((double*) ptr)[offset] = value;
            }
        }

        public virtual TStruct Get(int index) {
            AssertTrue(index >= 0 && index < Count, "Index is out of range.");
            return values[index];
        }

        protected internal override bool? IsEqualExactlyTo(DoubleArray other) {
            if (other is DoubleArrayStruct<TStruct> o) {
                fixed (TStruct* lhs = this.values, rhs = o.values) {
                    double* lhs_d = (double*) lhs, rhs_d = (double*) rhs;
                    var cnt = LinearLength;
                    for (int i = 0; i < cnt; i++)
                        if (lhs_d[i] != rhs_d[i] && double.IsNaN(lhs_d[i]) != double.IsNaN(rhs_d[i]))
                            return false;

                    return true;
                }
            }

            return null;
        }

        protected internal override int ComputeHashCode() {
            return values.GetHashCode();
        }

        public override Span<double> AsDoubleSpan => new Span<double>(Unsafe.AsPointer(ref values[0]), LinearLength);

        public override ref double GetPinnableReference() {
            return ref Unsafe.As<TStruct, double>(ref values[0]);
        }

        public override ref double GetPinnableReference(int index) {
            return ref Unsafe.As<TStruct, double>(ref values[index]);
        }

        /// <inheritdoc />
        /// <exception cref="ReshapeException">When copy is False. because TStruct is a representation of Properties dimension. call with copy: true</exception>
        /// <returns>A <see cref="DoubleArray2DManaged"/> reshaped.</returns>
        public override DoubleArray Reshape(int count, int properties, bool copy = true) {
            if (!copy)
                throw new ReshapeException("DoubleArrayStruct<TStruct> can't be reshaped without copying because TStruct is a representation of Properties dimension. call with copy: true");

            if (LinearLength != (count * properties))
                throw new ReshapeException($"Unable to reshape ({Count}, {Properties}) to ({count}, {properties})");

            var data = new double[count, properties];
            fixed (TStruct* src = values) {
                fixed (double* dst = data) {
                    Unsafe.CopyBlock(dst, src, (uint) (sizeof(double) * LinearLength));
                }
            }

            return new DoubleArray2DManaged(data);
        }

        public override DoubleArray Clone() {
            return new DoubleArrayStruct<TStruct>((TStruct[]) values.Clone());
        }
    }
}