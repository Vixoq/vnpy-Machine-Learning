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
using FinanceSharp.Delegates;
using FinanceSharp.Exceptions;

namespace FinanceSharp {
    public unsafe class DoubleArrayStructScalar<TStruct> : DoubleArray where TStruct : unmanaged, DataStruct {
        protected internal TStruct value;

        public override int Count {
            get => 1;
            protected internal set => throw new NotSupportedException();
        }

        public override int Properties {
            get => DataStructInfo<TStruct>.Properties;
            protected internal set => throw new NotSupportedException();
        }

        static DoubleArrayStructScalar() {
            //verify staticly that this struct indeed has only made from doubles fields.
            if (DataStructInfo<TStruct>.DoubleFieldsCount != DataStructInfo<TStruct>.Properties
                || DataStructInfo<TStruct>.DoubleFieldsCount != DataStructInfo<TStruct>.FieldsCount)
                throw new InvalidDoubleArrayStructException();
        }

        /// <param name="struct">The structure value that'll be wrapped.</param>
        public DoubleArrayStructScalar(in TStruct @struct) {
            value = @struct;
        }

        public DoubleArrayStructScalar() {
            value = new TStruct();
        }

        public override void ForEach(ReferenceForFunctionHandler function) {
            var cnt = Properties;
            fixed (double* ptr = this)
                for (int i = 0; i < cnt; i++)
                    function(ref ptr[i]);
        }

        public override double this[int property] {
            get {
                AssertTrue(IsScalar, "Scalar only overload was called but the array is not scalar.");
                AssertTrue(property >= 0 && property < Properties, "Property index is out of range.");
                fixed (double* ptr = this)
                    return ptr[property];
            }
            set {
                AssertTrue(IsScalar, "Scalar only overload was called but the array is not scalar.");
                AssertTrue(property >= 0 && property < Properties, "Property index is out of range.");
                fixed (double* ptr = this)
                    ptr[property] = value;
            }
        }

        public override double this[int index, int property] {
            get {
                AssertTrue(IsScalar, "Scalar only overload was called but the array is not scalar.");
                AssertTrue(index == 0, "Index is out of range.");
                AssertTrue(property >= 0 && property < Properties, "Property index is out of range.");
                fixed (double* ptr = this)
                    return ptr[property];
            }
            set {
                AssertTrue(IsScalar, "Scalar only overload was called but the array is not scalar.");
                AssertTrue(index == 0, "Index is out of range.");
                AssertTrue(property >= 0 && property < Properties, "Property index is out of range.");
                fixed (double* ptr = this)
                    ptr[property] = value;
            }
        }

        public override double GetLinear(int offset) {
            AssertTrue(offset >= 0 && offset < LinearLength, "Offset is out of range.");
            fixed (double* ptr = this)
                return ptr[offset];
        }

        // ReSharper disable once ParameterHidesMember
        public override void SetLinear(int offset, double value) {
            AssertTrue(offset >= 0 && offset < LinearLength, "Offset is out of range.");
            fixed (double* ptr = this)
                ptr[offset] = value;
        }

        public virtual TStruct Get(int index) {
            AssertTrue(index >= 0 && index < Properties, "Index is out of range.");
            return value;
        }

        protected internal override bool? IsEqualExactlyTo(DoubleArray other) {
            if (other is DoubleArrayStructScalar<TStruct> o) {
                return o.value.Equals(value);
            }

            return null;
        }

        protected internal override int ComputeHashCode() {
            return value.GetHashCode();
        }

        public override Span<double> AsDoubleSpan => new Span<double>(Unsafe.AsPointer(ref this.value), LinearLength);

        public override ref double GetPinnableReference() {
            return ref Unsafe.As<TStruct, double>(ref value);
        }

        public override ref double GetPinnableReference(int index) {
            AssertTrue(index == 0, "Index is out of range.");
            return ref Unsafe.As<TStruct, double>(ref value);
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
            fixed (TStruct* src = &value) {
                fixed (double* dst = data) {
                    Unsafe.CopyBlock(dst, src, (uint) (sizeof(double) * LinearLength));
                }
            }

            return new DoubleArray2DManaged(data);
        }

        public override DoubleArray Clone() {
            return new DoubleArrayStructScalar<TStruct>(in value);
        }
    }
}