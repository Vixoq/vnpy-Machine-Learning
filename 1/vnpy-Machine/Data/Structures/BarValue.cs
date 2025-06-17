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

using System.Runtime.InteropServices;

namespace FinanceSharp {
    /// <summary>
    ///     Represents a CHLO candle struct.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct BarValue : DataStruct {
        public const int Properties = 4;
        public double Close;
        public double High;
        public double Low;
        public double Open;

        public BarValue(double close, double high, double low, double open) {
            Close = close;
            High = high;
            Low = low;
            Open = open;
        }

        public object Clone() {
            return new BarValue(Close, High, Low, Open);
        }

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>The fully qualified type name.</returns>
        public override string ToString() {
            return $"{nameof(Close)}: {Close}, {nameof(High)}: {High}, {nameof(Low)}: {Low}, {nameof(Open)}: {Open}, {nameof(Properties)}: {Properties}";
        }

        #region Explicit Interfaces

        int DataStruct.Properties => Properties;

        double DataStruct.Value {
            get => Close;
            set => Close = value;
        }

        #endregion
    }
}