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

namespace FinanceSharp.Consolidators {
    /// <summary>
    /// 	 Represents a type capable of taking BaseData updates and firing events containing new
    /// 'consolidated' data. These types can be used to produce larger bars, or even be used to
    /// 	 transform the data before being sent to another component. The most common usage of these
    /// 	 types is with indicators.
    /// </summary>
    public interface IDataConsolidator : IIndicator, IDisposable {
        /// <summary>
        /// 	 Gets a clone of the data being currently consolidated
        /// </summary>
        DoubleArray WorkingData { get; }

        /// <summary>
        /// 	 Scans this consolidator to see if it should emit a bar due to time passing
        /// </summary>
        /// <param name="currentLocalTime">The current time in the local time zone (same as <see cref="BaseData.Time"/>)</param>
        void Scan(long currentLocalTime);
    }
}