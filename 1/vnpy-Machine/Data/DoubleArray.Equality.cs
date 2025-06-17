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

namespace FinanceSharp {
    public abstract unsafe partial class DoubleArray : IEquatable<DoubleArray> {
        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(DoubleArray other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Count == other.Count && Properties == other.Properties && (IsEqualExactlyTo(other) ?? CompareValues(other));
        }

        /// <summary>
        ///     A less performant version of <see cref="IsEqualExactlyTo"/> that can handle all types of <see cref="DoubleArray"/>.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        protected virtual bool CompareValues(DoubleArray other) {
            bool AreNotEqual(double lhs, double rhs) => lhs != rhs && double.IsNaN(lhs) != double.IsNaN(rhs);

            for (int i = 0; i < Count; i++) {
                for (int j = 0; j < Properties; j++) {
                    if (AreNotEqual(this[i, j], other[i, j]))
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        ///     A comparison method used when <see cref="Equals(DoubleArray)"/> is called from <see cref="IEquatable{T}"/> to compare same types of <see cref="DoubleArray"/>.
        /// </summary>
        /// <param name="other">An DoubleArray to compare to this.</param>
        /// <returns>Are <see cref="this"/> equals to <see cref="other"/>. Null returned when test was not performed.</returns>
        protected internal abstract bool? IsEqualExactlyTo(DoubleArray other);

        /// <summary>
        ///     Additional hashcode for inherieted classes.
        /// </summary>
        /// <returns></returns>
        protected internal abstract int ComputeHashCode();

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj) {
            return ReferenceEquals(this, obj) || obj is DoubleArray other && Equals(other);
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() {
            unchecked {
                return (Count * 397) ^ (Properties * 396) ^ ComputeHashCode();
            }
        }

        /// <summary>Returns a value that indicates whether the values of two <see cref="T:FinanceSharp.DoubleArray" /> objects are equal.</summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>true if the <paramref name="left" /> and <paramref name="right" /> parameters have the same value; otherwise, false.</returns>
        public static bool operator ==(DoubleArray left, DoubleArray right) {
            return Equals(left, right);
        }

        /// <summary>Returns a value that indicates whether two <see cref="T:FinanceSharp.DoubleArray" /> objects have different values.</summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>true if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
        public static bool operator !=(DoubleArray left, DoubleArray right) {
            return !Equals(left, right);
        }


        /// <summary>Returns a value that indicates whether the values of two <see cref="T:FinanceSharp.DoubleArray" /> objects are equal.</summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>true if the <paramref name="left" /> and <paramref name="right" /> parameters have the same value; otherwise, false.</returns>
        public static bool operator ==(DoubleArray left, double right) {
            return left != null && left.Properties == 1 && left.Count == 1 && Equals(left.Value, right);
        }

        /// <summary>Returns a value that indicates whether two <see cref="T:FinanceSharp.DoubleArray" /> objects have different values.</summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>true if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
        public static bool operator !=(DoubleArray left, double right) {
            return !(left == right);
        }

        /// <summary>Returns a value that indicates whether the values of two <see cref="T:FinanceSharp.DoubleArray" /> objects are equal.</summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>true if the <paramref name="left" /> and <paramref name="right" /> parameters have the same value; otherwise, false.</returns>
        public static bool operator ==(double left, DoubleArray right) {
            return right != null && right.Properties == 1 && right.Count == 1 && Equals(left, right.Value);
        }

        /// <summary>Returns a value that indicates whether two <see cref="T:FinanceSharp.DoubleArray" /> objects have different values.</summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>true if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
        public static bool operator !=(double left, DoubleArray right) {
            return !(left == right);
        }
    }
}