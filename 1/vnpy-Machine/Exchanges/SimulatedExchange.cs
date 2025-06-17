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

namespace FinanceSharp.Simulator {
    public interface ISimulatedExchange {
        //TODO: this is incomplete, Discard or implement.
        TimeSpan TimeStep { get; }

        /// <summary>
        ///     Every timestep this <see cref="IUpdatable"/> gets updated with a <see cref="TickValue"/>
        /// </summary>
        IUpdatable Source { get; }

        /// <summary>
        ///     Step one timestep.
        /// </summary>
        void Step();

        void Reset();
    }

    public class SimulatedExchange : ISimulatedExchange {
        public TimeSpan TimeStep { get; } = TimeSpan.FromSeconds(1);

        /// <summary>
        ///     Every timestep this <see cref="IUpdatable"/> gets updated with a <see cref="TickValue"/>
        /// </summary>
        public IUpdatable Source { get; }

        /// <summary>
        ///     Step one timestep.
        /// </summary>
        public void Step() { }

        public void Reset() { }
    }
}