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

namespace FinanceSharp {
    /// <summary>
    ///     Ensures that the given T structures are in fact unmanaged during compile time
    /// </summary>
    internal class UnmanagedEnsurer {
#pragma warning disable 169
        private MustBeUnmanaged<BarValue> _c;
        private MustBeUnmanaged<TradeBarValue> _d;
        private MustBeUnmanaged<IndicatorValue> _e;
#pragma warning restore 169

        // ReSharper disable once ClassNeverInstantiated.Local
        private class MustBeUnmanaged<T> where T : unmanaged { }
    }
}