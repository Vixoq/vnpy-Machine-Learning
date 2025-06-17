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

// ReSharper disable once CheckNamespace

namespace FinanceSharp {
    public static class Constants {
        public static readonly DoubleArray Empty = new DoubleArrayScalar(0);
        public const double Zero = 0;

        public const double One = 1;
        public const bool True = true;
        public const bool False = false;
        public const double ZeroEpsilon = 1e-6;

        /// Close 
        public const int C = 0;

        /// High
        public const int H = 1;

        /// Low
        public const int L = 2;

        /// Open
        public const int O = 3;

        /// Volume
        public const int V = 4;

        /// Open
        public const int OpenIdx = O;

        /// High
        public const int HighIdx = H;

        /// Low
        public const int LowIdx = L;

        /// Close 
        public const int CloseIdx = C;

        /// Volume
        public const int VolumeIdx = V;

        public const int BidPrice = 1;
        public const int AskPrice = 2;
        public const int BidSize = 3;
        public const int AskSize = 5;

        static Constants() { }
    }
}