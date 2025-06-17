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

using System.Diagnostics;

namespace FinanceSharp {
    /// <summary>
    ///     Represents an object that can be updated.
    /// </summary>
    /// <remarks>Serves as a graphing component.</remarks>
    public interface IUpdatable
    {
        /// <summary>
        /// 	 Event handler that fires after this updatable is updated.
        /// </summary>
        event UpdatedHandler Updated;

        /// <summary>
        ///     Event handler that fires after this updatable is reset.
        /// </summary>
        event ResettedHandler Resetted;

        /// <summary>
        /// 	 Updates the state of this updatable with the given value and returns true
        /// 	 if this updatable is ready, false otherwise
        /// </summary>
        /// <param name="time">The timestamp represented in milliseconds-epoch-1970.</param>
        /// <param name="input">The value to use to update this updatable</param>
        /// <returns>True if this updatable is ready, false otherwise</returns>
        void Update(long time, DoubleArray input);

        /// <summary>
        /// 	 Resets this updatable to its initial state
        /// </summary>
        void Reset();

        /// <summary>
        /// 	 Gets a flag indicating when this updatable is ready and fully initialized
        /// </summary>
        bool IsReady { get; }

        /// <summary>
        /// 	 Gets the current state of this updatable. If the state has not been updated
        /// 	 then the value will be null.
        /// </summary>
        DoubleArray Current { get; }

        /// <summary>
        /// 	 Gets the current time of <see cref="Current"/>.
        /// </summary>
        long CurrentTime { get; }

        /// <summary>
        ///     The number of properties of input argument of <see cref="Update"/> must have.
        /// </summary>
        int InputProperties { get; }

        /// <summary>
        ///     The number of items <see cref="Current"/> will have.
        /// </summary>
        int OutputCount { get; }

        /// <summary>
        ///     The number of properties <see cref="Current"/> will have.
        /// </summary>
        int Properties { get; }
    }

    /// <summary>
    /// 	 Represents an indicator that can receive data updates and emit events when the value of
    /// 	 the indicator has changed.
    /// </summary>
    public interface IIndicator : IUpdatable {
        /// <summary>
        /// 	 Gets a name for this indicator
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 	 Gets the number of samples processed by this indicator
        /// </summary>
        long Samples { get; }

        /// <summary>
        /// 	 Required period, in data points (number of updates), for the indicator to be ready and fully initialized.
        /// </summary>
        int WarmUpPeriod { get; }
    }
}