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
using System.Runtime.InteropServices;
using System.Threading;

namespace FinanceSharp {
    /// <summary>
    ///     A thread 
    /// </summary>
    public class DisposerThread : IDisposable {
        protected BusyBlockingCollection<object> queue = new BusyBlockingCollection<object>();
        protected Thread thread;
        protected CancellationTokenSource source;
        public static DisposerThread Instance;
        protected static BusyBlockingCollection<object> InstanceQueue;

        static DisposerThread() {
            Instance = new DisposerThread();
            InstanceQueue = Instance.queue;
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        protected DisposerThread() {
            source = new CancellationTokenSource();
            thread = new Thread(Consumer);
            thread.Name = "DisposerThread";
            thread.Start();
        }

        private void Consumer() {
            var token = source.Token;
            foreach (var o in queue.GetConsumingEnumerable(token)) {
                switch (o) {
                    case IntPtr ptr:
                        Marshal.FreeHGlobal(ptr);
                        break;
                    case GCHandle handle:
                        handle.Free();
                        break;
                    case null:
                        break;
                    case Action act:
                        act();
                        break;
                }
            }
        }

        public void Cancel() {
            source.Cancel();
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose() {
            queue?.Dispose();
            source.Cancel();
        }

        public static void Enqueue(IntPtr ptr) {
            InstanceQueue.Add(ptr);
        }

        public static void Enqueue(GCHandle ptr) {
            try {
                if (ptr.IsAllocated)
                    InstanceQueue.Add(ptr);
            } catch (InvalidOperationException) {
                //swallow
            }
        }

        public static void Enqueue(Action work) {
            InstanceQueue.Add(work);
        }

        public static unsafe void Enqueue(void* ptr) {
            InstanceQueue.Add(new IntPtr(ptr));
        }
    }
}