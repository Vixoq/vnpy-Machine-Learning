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
using FinanceSharp.Exceptions;

namespace FinanceSharp.Graphing {
    public partial class Concat {
        /// <summary>
        /// 	 Event handler that fires after this updatable is updated.
        /// </summary>
        public event UpdatedHandler Updated;

        /// <summary>
        ///     Event handler that fires after this updatable is reset.
        /// </summary>
        public event ResettedHandler Resetted;

        /// <summary>
        /// 	 Updates the state of this updatable with the given value and returns true
        /// 	 if this updatable is ready, false otherwise
        /// </summary>
        /// <param name="time"></param>
        /// <param name="input">The value to use to update this updatable</param>
        /// <returns>True if this updatable is ready, false otherwise</returns>
        public void Update(long time, DoubleArray input) {
            if (UpdateHandler == null)
                throw new IndicatorNotUpdatableDirectlyException("Concat can't be updated directy. It rather binds to IUpdatables during construction.");
            UpdateHandler(time, input);
        }

        /// <summary>
        /// 	 Resets this updatable to its initial state
        /// </summary>
        public void Reset() {
            Samples = 0;
            CurrentTime = 0;
            if (signalCounter != null)
                Array.Clear(signalCounter, 0, signalCounter.Length);
            counter = length;
            unsafe {
                fixed (double* _ = workingTarget)
                    workingTarget.Fill(0d);
            }

            Resetted?.Invoke(this);
        }

        /// <summary>
        /// 	 Gets a flag indicating when this updatable is ready and fully initialized
        /// </summary>
        public bool IsReady => Samples > 0;

        /// <summary>
        /// 	 Gets the current state of this updatable. If the state has not been updated
        /// 	 then the value will be null.
        /// </summary>
        public DoubleArray Current => workingTarget;

        /// <summary>
        /// 	 Gets the current time of <see cref="IUpdatable.Current"/>.
        /// </summary>
        public long CurrentTime { get; protected set; }

        /// <summary>
        ///     The number of properties of input argument of <see cref="IUpdatable.Update"/> must have.
        /// </summary>
        int IUpdatable.InputProperties => -1;

        /// <summary>
        ///     The number of items <see cref="IUpdatable.Current"/> will have.
        /// </summary>
        public int OutputCount => outputCount;

        /// <summary>
        ///     The number of properties <see cref="IUpdatable.Current"/> will have.
        /// </summary>
        public int Properties { get; set; }

        /// <summary>
        /// 	 Gets a name for this indicator
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 	 Gets the number of samples processed by this indicator
        /// </summary>
        public long Samples { get; protected set; }

        /// <summary>
        /// 	 Required period, in data points (number of updates), for the indicator to be ready and fully initialized.
        /// </summary>
        public int WarmUpPeriod => 0;
    }
}