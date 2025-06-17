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
using FinanceSharp.Indicators;

namespace FinanceSharp {
    public unsafe partial class DoubleArray {
        public static implicit operator DoubleArray(double scalar) => new DoubleArrayScalar(scalar);
        public static explicit operator double(DoubleArray scalar) => scalar.Value;

#if _REGEN
        #region Compute
        %operators = ["add", "sub", "mul", "div", "mod"]
        %operators_sign = ["+", "-", "*", "/", "%"]
        %operators_comparers = [">", "<", ">=", "<="]
        %operators_comparers_names = ["greater", "less", "greater_equal", "less_equal"]

        %possabilities = ["sbyte", "byte", "short", "ushort", "int", "uint", "ulong", "long", "float", "double", "IndicatorBase"]
		
        %foreach operators, operators_sign%
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static DoubleArray operator #2(DoubleArray lhs, DoubleArray rhs) => lhs.Function(rhs, (l, r) => l #2 r);
            %foreach possabilities%
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static DoubleArray operator #2(DoubleArray lhs, #101 rhs) => lhs.Function(d => d #2 rhs, true);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static DoubleArray operator #2(#101 lhs, DoubleArray rhs) => rhs.Function(d => lhs #2 d, true);
            %
        %				

        %foreach operators, operators_sign%
            %foreach ["decimal"]%
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static DoubleArray operator #2(DoubleArray lhs, #101 rhs) => lhs #2 (double) rhs;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static DoubleArray operator #2(#101 lhs, DoubleArray rhs) => (double) lhs #2 rhs;
            %
        %		

        %foreach except(possabilities, "double", "IndicatorBase")%
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static explicit operator DoubleArray(#1 scalar) => new DoubleArrayScalar(scalar);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static explicit operator #1(DoubleArray scalar) => (#1)scalar.Value;
        %
        #endregion
#else

        #region Compute

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator +(DoubleArray lhs, DoubleArray rhs) => lhs.Function(rhs, (l, r) => l + r);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator +(DoubleArray lhs, sbyte rhs) => lhs.Function(d => d + rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator +(sbyte lhs, DoubleArray rhs) => rhs.Function(d => lhs + d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator +(DoubleArray lhs, byte rhs) => lhs.Function(d => d + rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator +(byte lhs, DoubleArray rhs) => rhs.Function(d => lhs + d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator +(DoubleArray lhs, short rhs) => lhs.Function(d => d + rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator +(short lhs, DoubleArray rhs) => rhs.Function(d => lhs + d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator +(DoubleArray lhs, ushort rhs) => lhs.Function(d => d + rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator +(ushort lhs, DoubleArray rhs) => rhs.Function(d => lhs + d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator +(DoubleArray lhs, int rhs) => lhs.Function(d => d + rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator +(int lhs, DoubleArray rhs) => rhs.Function(d => lhs + d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator +(DoubleArray lhs, uint rhs) => lhs.Function(d => d + rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator +(uint lhs, DoubleArray rhs) => rhs.Function(d => lhs + d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator +(DoubleArray lhs, ulong rhs) => lhs.Function(d => d + rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator +(ulong lhs, DoubleArray rhs) => rhs.Function(d => lhs + d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator +(DoubleArray lhs, long rhs) => lhs.Function(d => d + rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator +(long lhs, DoubleArray rhs) => rhs.Function(d => lhs + d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator +(DoubleArray lhs, float rhs) => lhs.Function(d => d + rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator +(float lhs, DoubleArray rhs) => rhs.Function(d => lhs + d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator +(DoubleArray lhs, double rhs) => lhs.Function(d => d + rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator +(double lhs, DoubleArray rhs) => rhs.Function(d => lhs + d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator +(DoubleArray lhs, IndicatorBase rhs) => lhs.Function(d => d + rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator +(IndicatorBase lhs, DoubleArray rhs) => rhs.Function(d => lhs + d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator -(DoubleArray lhs, DoubleArray rhs) => lhs.Function(rhs, (l, r) => l - r);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator -(DoubleArray lhs, sbyte rhs) => lhs.Function(d => d - rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator -(sbyte lhs, DoubleArray rhs) => rhs.Function(d => lhs - d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator -(DoubleArray lhs, byte rhs) => lhs.Function(d => d - rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator -(byte lhs, DoubleArray rhs) => rhs.Function(d => lhs - d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator -(DoubleArray lhs, short rhs) => lhs.Function(d => d - rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator -(short lhs, DoubleArray rhs) => rhs.Function(d => lhs - d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator -(DoubleArray lhs, ushort rhs) => lhs.Function(d => d - rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator -(ushort lhs, DoubleArray rhs) => rhs.Function(d => lhs - d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator -(DoubleArray lhs, int rhs) => lhs.Function(d => d - rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator -(int lhs, DoubleArray rhs) => rhs.Function(d => lhs - d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator -(DoubleArray lhs, uint rhs) => lhs.Function(d => d - rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator -(uint lhs, DoubleArray rhs) => rhs.Function(d => lhs - d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator -(DoubleArray lhs, ulong rhs) => lhs.Function(d => d - rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator -(ulong lhs, DoubleArray rhs) => rhs.Function(d => lhs - d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator -(DoubleArray lhs, long rhs) => lhs.Function(d => d - rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator -(long lhs, DoubleArray rhs) => rhs.Function(d => lhs - d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator -(DoubleArray lhs, float rhs) => lhs.Function(d => d - rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator -(float lhs, DoubleArray rhs) => rhs.Function(d => lhs - d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator -(DoubleArray lhs, double rhs) => lhs.Function(d => d - rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator -(double lhs, DoubleArray rhs) => rhs.Function(d => lhs - d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator -(DoubleArray lhs, IndicatorBase rhs) => lhs.Function(d => d - rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator -(IndicatorBase lhs, DoubleArray rhs) => rhs.Function(d => lhs - d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator *(DoubleArray lhs, DoubleArray rhs) => lhs.Function(rhs, (l, r) => l * r);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator *(DoubleArray lhs, sbyte rhs) => lhs.Function(d => d * rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator *(sbyte lhs, DoubleArray rhs) => rhs.Function(d => lhs * d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator *(DoubleArray lhs, byte rhs) => lhs.Function(d => d * rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator *(byte lhs, DoubleArray rhs) => rhs.Function(d => lhs * d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator *(DoubleArray lhs, short rhs) => lhs.Function(d => d * rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator *(short lhs, DoubleArray rhs) => rhs.Function(d => lhs * d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator *(DoubleArray lhs, ushort rhs) => lhs.Function(d => d * rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator *(ushort lhs, DoubleArray rhs) => rhs.Function(d => lhs * d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator *(DoubleArray lhs, int rhs) => lhs.Function(d => d * rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator *(int lhs, DoubleArray rhs) => rhs.Function(d => lhs * d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator *(DoubleArray lhs, uint rhs) => lhs.Function(d => d * rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator *(uint lhs, DoubleArray rhs) => rhs.Function(d => lhs * d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator *(DoubleArray lhs, ulong rhs) => lhs.Function(d => d * rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator *(ulong lhs, DoubleArray rhs) => rhs.Function(d => lhs * d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator *(DoubleArray lhs, long rhs) => lhs.Function(d => d * rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator *(long lhs, DoubleArray rhs) => rhs.Function(d => lhs * d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator *(DoubleArray lhs, float rhs) => lhs.Function(d => d * rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator *(float lhs, DoubleArray rhs) => rhs.Function(d => lhs * d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator *(DoubleArray lhs, double rhs) => lhs.Function(d => d * rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator *(double lhs, DoubleArray rhs) => rhs.Function(d => lhs * d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator *(DoubleArray lhs, IndicatorBase rhs) => lhs.Function(d => d * rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator *(IndicatorBase lhs, DoubleArray rhs) => rhs.Function(d => lhs * d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator /(DoubleArray lhs, DoubleArray rhs) => lhs.Function(rhs, (l, r) => l / r);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator /(DoubleArray lhs, sbyte rhs) => lhs.Function(d => d / rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator /(sbyte lhs, DoubleArray rhs) => rhs.Function(d => lhs / d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator /(DoubleArray lhs, byte rhs) => lhs.Function(d => d / rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator /(byte lhs, DoubleArray rhs) => rhs.Function(d => lhs / d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator /(DoubleArray lhs, short rhs) => lhs.Function(d => d / rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator /(short lhs, DoubleArray rhs) => rhs.Function(d => lhs / d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator /(DoubleArray lhs, ushort rhs) => lhs.Function(d => d / rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator /(ushort lhs, DoubleArray rhs) => rhs.Function(d => lhs / d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator /(DoubleArray lhs, int rhs) => lhs.Function(d => d / rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator /(int lhs, DoubleArray rhs) => rhs.Function(d => lhs / d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator /(DoubleArray lhs, uint rhs) => lhs.Function(d => d / rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator /(uint lhs, DoubleArray rhs) => rhs.Function(d => lhs / d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator /(DoubleArray lhs, ulong rhs) => lhs.Function(d => d / rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator /(ulong lhs, DoubleArray rhs) => rhs.Function(d => lhs / d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator /(DoubleArray lhs, long rhs) => lhs.Function(d => d / rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator /(long lhs, DoubleArray rhs) => rhs.Function(d => lhs / d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator /(DoubleArray lhs, float rhs) => lhs.Function(d => d / rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator /(float lhs, DoubleArray rhs) => rhs.Function(d => lhs / d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator /(DoubleArray lhs, double rhs) => lhs.Function(d => d / rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator /(double lhs, DoubleArray rhs) => rhs.Function(d => lhs / d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator /(DoubleArray lhs, IndicatorBase rhs) => lhs.Function(d => d / rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator /(IndicatorBase lhs, DoubleArray rhs) => rhs.Function(d => lhs / d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator %(DoubleArray lhs, DoubleArray rhs) => lhs.Function(rhs, (l, r) => l % r);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator %(DoubleArray lhs, sbyte rhs) => lhs.Function(d => d % rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator %(sbyte lhs, DoubleArray rhs) => rhs.Function(d => lhs % d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator %(DoubleArray lhs, byte rhs) => lhs.Function(d => d % rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator %(byte lhs, DoubleArray rhs) => rhs.Function(d => lhs % d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator %(DoubleArray lhs, short rhs) => lhs.Function(d => d % rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator %(short lhs, DoubleArray rhs) => rhs.Function(d => lhs % d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator %(DoubleArray lhs, ushort rhs) => lhs.Function(d => d % rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator %(ushort lhs, DoubleArray rhs) => rhs.Function(d => lhs % d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator %(DoubleArray lhs, int rhs) => lhs.Function(d => d % rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator %(int lhs, DoubleArray rhs) => rhs.Function(d => lhs % d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator %(DoubleArray lhs, uint rhs) => lhs.Function(d => d % rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator %(uint lhs, DoubleArray rhs) => rhs.Function(d => lhs % d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator %(DoubleArray lhs, ulong rhs) => lhs.Function(d => d % rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator %(ulong lhs, DoubleArray rhs) => rhs.Function(d => lhs % d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator %(DoubleArray lhs, long rhs) => lhs.Function(d => d % rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator %(long lhs, DoubleArray rhs) => rhs.Function(d => lhs % d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator %(DoubleArray lhs, float rhs) => lhs.Function(d => d % rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator %(float lhs, DoubleArray rhs) => rhs.Function(d => lhs % d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator %(DoubleArray lhs, double rhs) => lhs.Function(d => d % rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator %(double lhs, DoubleArray rhs) => rhs.Function(d => lhs % d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator %(DoubleArray lhs, IndicatorBase rhs) => lhs.Function(d => d % rhs, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator %(IndicatorBase lhs, DoubleArray rhs) => rhs.Function(d => lhs % d, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator +(DoubleArray lhs, decimal rhs) => lhs + (double) rhs;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator +(decimal lhs, DoubleArray rhs) => (double) lhs + rhs;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator -(DoubleArray lhs, decimal rhs) => lhs - (double) rhs;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator -(decimal lhs, DoubleArray rhs) => (double) lhs - rhs;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator *(DoubleArray lhs, decimal rhs) => lhs * (double) rhs;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator *(decimal lhs, DoubleArray rhs) => (double) lhs * rhs;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator /(DoubleArray lhs, decimal rhs) => lhs / (double) rhs;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator /(decimal lhs, DoubleArray rhs) => (double) lhs / rhs;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator %(DoubleArray lhs, decimal rhs) => lhs % (double) rhs;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleArray operator %(decimal lhs, DoubleArray rhs) => (double) lhs % rhs;

        #endregion

#endif


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator DoubleArray(sbyte scalar) => new DoubleArrayScalar(scalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator sbyte(DoubleArray scalar) => (sbyte) scalar.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator DoubleArray(byte scalar) => new DoubleArrayScalar(scalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator byte(DoubleArray scalar) => (byte) scalar.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator DoubleArray(short scalar) => new DoubleArrayScalar(scalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator short(DoubleArray scalar) => (short) scalar.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator DoubleArray(ushort scalar) => new DoubleArrayScalar(scalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ushort(DoubleArray scalar) => (ushort) scalar.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator DoubleArray(int scalar) => new DoubleArrayScalar(scalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator int(DoubleArray scalar) => (int) scalar.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator DoubleArray(uint scalar) => new DoubleArrayScalar(scalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator uint(DoubleArray scalar) => (uint) scalar.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator DoubleArray(ulong scalar) => new DoubleArrayScalar(scalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ulong(DoubleArray scalar) => (ulong) scalar.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator DoubleArray(long scalar) => new DoubleArrayScalar(scalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator long(DoubleArray scalar) => (long) scalar.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator DoubleArray(float scalar) => new DoubleArrayScalar(scalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator float(DoubleArray scalar) => (float) scalar.Value;
    }
}