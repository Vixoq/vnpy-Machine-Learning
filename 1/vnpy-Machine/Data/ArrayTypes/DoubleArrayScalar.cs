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
    public class DoubleArrayScalar : DoubleArray {
        protected double value;

        public override int Count {
            get => 1;
            protected internal set => throw new NotSupportedException();
        }

        public override int Properties {
            get => 1;
            protected internal set => throw new NotSupportedException();
        }

        /// <param name="value">A scalar value.</param>
        public DoubleArrayScalar(double value) {
            this.value = value;
        }

        public DoubleArrayScalar() {
            this.value = 0d;
        }

        public override void ForEach(ReferenceForFunctionHandler function) {
            function(ref value);
        }

        public override unsafe Span<double> AsDoubleSpan => new Span<double>(Unsafe.AsPointer(ref value), 1);

        public override ref double GetPinnableReference() {
            return ref value;
        }

        public override ref double GetPinnableReference(int index) {
            AssertTrue(index == 0, "Index out of range.");
            return ref value;
        }

        protected internal override bool? IsEqualExactlyTo(DoubleArray other) {
            var o = other[0, 0];
            return o == value || double.IsNaN(o) == double.IsNaN(value);
        }

        protected internal override int ComputeHashCode() {
            return value.GetHashCode();
        }

        public override double this[int property] {
            get {
                AssertTrue(property == 0, "Property index is out of range.");
                return this.value;
            }
            set {
                AssertTrue(property == 0, "Property index is out of range.");
                this.value = value;
            }
        }

        public override double this[int index, int property] {
            get {
                AssertTrue(index < Count, "Index is out of range.");
                AssertTrue(property == 0, "Property index is out of range.");
                return this.value;
            }
            set {
                AssertTrue(index < Count, "Index is out of range.");
                AssertTrue(property == 0, "Property index is out of range.");
                this.value = value;
            }
        }

        public override TStruct Get<TStruct>(int index) {
            return new TStruct() {Value = value};
        }

        public override double GetLinear(int offset) {
            AssertTrue(offset == 0);
            return value;
        }

        /// <summary>
        ///     Writes to this DoubleArray linearly regardless to shape.
        /// </summary>
        /// <param name="offset">Absolute offset to set <paramref name="value"/> at.</param>
        /// <param name="value">The value to write</param>
        public override void SetLinear(int offset, double value) {
            AssertTrue(offset == 0);
            this.value = value;
        }


        public override DoubleArray Reshape(int count, int properties, bool copy = true) {
            if (count == 1 && properties == 1) {
                if (copy)
                    return Clone();
                else
                    return this;
            }

            throw new ReshapeException($"Unable to reshape ({Count}, {Properties}) to ({count}, {properties})");
        }

        public override DoubleArray Clone() {
            return new DoubleArrayScalar(value);
        }

        #region Blunt Overloads

        public override double Value {
            get => this.value;
            set => this.value = value;
        }

        public override double Close {
            get => this.value;
            set => this.value = value;
        }

        #endregion
    }
}