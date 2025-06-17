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
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FinanceSharp {
    /// <summary>
    ///     Serves a similar purpose to <see cref="RollingWindow{T}"/> but the data is rolled inside <see cref="Current"/> (a <see cref="DoubleArray"/>).
    /// </summary>
    [DebuggerDisplay("{" + nameof(ToDetailedString) + "()}")]
    public class ArrayRollingWindow : IIndicator {
        protected DoubleArray _mostRecentlyRemoved;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public ArrayRollingWindow(int outputCount, int properties, string name = null) {
            OutputCount = outputCount;
            InputProperties = Properties = properties;
            Current = new DoubleArray2DManaged(OutputCount, Properties);
            Name = name ?? $"WINDOW({OutputCount}, {Properties})";
        }

        /// <summary>
        ///     Gets the most recently removed item from the window. This is the
        ///     piece of data that just 'fell off' as a result of the most recent
        ///     add. If no items have been removed, this will throw an exception.
        /// </summary>
        /// <exception cref="InvalidOperationException">When unable to return MostRecentlyRemoved item because this indicator was not updated yet.</exception>
        public DoubleArray MostRecentlyRemoved {
            get => _mostRecentlyRemoved ?? throw new InvalidOperationException("Unable to return MostRecentlyRemoved item because this indicator was not updated enough yet. Expression Samples > OutputCount must be true.");
            protected set => _mostRecentlyRemoved = value;
        }

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
        /// <param name="time">The timestamp represented in milliseconds-epoch-1970.</param>
        /// <param name="input">The value to use to update this updatable</param>
        /// <returns>True if this updatable is ready, false otherwise</returns>
        public unsafe void Update(long time, DoubleArray input) {
            CurrentTime = time;
            if (Properties > 1) {
                var cnt = (int) Math.Min(OutputCount, Samples);
                fixed (double* current = Current, input_ptr = input) {
                    if (Samples >= OutputCount) {
                        _mostRecentlyRemoved = new DoubleArray2DManaged(1, Properties);
                        fixed (double* recentlyRemoved = _mostRecentlyRemoved)
                            Unsafe.CopyBlock(recentlyRemoved, current + Properties * (OutputCount - 1), (uint) (sizeof(double) * Properties));

                        //Shift All items once
                        Unsafe.CopyBlock(current + Properties, current, (uint) (sizeof(double) * (cnt - 1) * Properties));
                    } else {
                        //Shift All items collected by now once
                        if (cnt > 0)
                            Unsafe.CopyBlock(current + Properties, current, (uint) (sizeof(double) * (cnt) * Properties));
                    }

                    //Copy input
                    Unsafe.CopyBlock(current, input_ptr, (uint) (sizeof(double) * Math.Min(Properties, input.Properties)));
                }
            } else {
                //Properties == 1
                var cnt = (int) Math.Min(OutputCount, Samples);
                fixed (double* current = Current, input_ptr = input) {
                    if (Samples >= OutputCount) {
                        _mostRecentlyRemoved = new DoubleArrayScalar(Current[OutputCount - 1, 0]);
                        Unsafe.CopyBlock(current + Properties, current, (uint) (sizeof(double) * (cnt - 1) * Properties));
                    } else {
                        if (cnt > 0)
                            Unsafe.CopyBlock(current + Properties, current, (uint) (sizeof(double) * (cnt) * Properties));
                    }

                    *current = *input_ptr;
                }
            }

            Samples++;
            OnUpdated(time, Current);
        }

        /// <summary>
        /// 	 Resets this updatable to its initial state
        /// </summary>
        public void Reset() {
            Current.Fill(0d);
            CurrentTime = 0;
            Samples = 0;
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
        public DoubleArray Current { get; protected set; }

        /// <summary>
        /// 	 Gets the current time of <see cref="IUpdatable.Current"/>.
        /// </summary>
        public long CurrentTime { get; protected set; }

        /// <summary>
        ///     The number of properties of input argument of <see cref="IUpdatable.Update"/> must have.
        /// </summary>
        public int InputProperties { get; protected set; }

        /// <summary>
        ///     The number of items <see cref="IUpdatable.Current"/> will have.
        /// </summary>
        public int OutputCount { get; protected set; }

        /// <summary>
        ///     The number of properties <see cref="IUpdatable.Current"/> will have.
        /// </summary>
        public int Properties { get; protected internal set; }

        /// <summary>
        /// 	 Gets a name for this indicator
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// 	 Gets the number of samples processed by this indicator
        /// </summary>
        public long Samples { get; protected set; }

        /// <summary>
        /// 	 Required period, in data points (number of updates), for the indicator to be ready and fully initialized.
        /// </summary>
        int IIndicator.WarmUpPeriod => 0;

        /// <summary>
        /// 	 Event invocator for the Updated event
        /// </summary>
        /// <param name="consolidated">This is the new piece of data produced by this indicator</param>
        protected virtual void OnUpdated(long time, DoubleArray consolidated) {
            Updated?.Invoke(time, consolidated);
        }

        /// <summary>
        /// 	 Provides a more detailed string of this indicator in the form of {Name} - {Value}
        /// </summary>
        /// <returns>A detailed string of this indicator's current state</returns>
        public string ToDetailedString() {
            return $"{Name} - ({OutputCount}, {Properties}) - {Current}";
        }
    }
}