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
    ///     Provides options of how to convert a <see cref="DoubleArray"/> to an NDArray.
    /// </summary>
    public enum ArrayConversionMethod {
        /// <summary>
        ///     Reinterprets the entire array to be as TDestStruct. Similar to casting address pointer. returned array's Length will be total-bytes/sizeof(TDestStruct)
        /// </summary>
        /// <remarks>total_bytes % sizeof(TDestStruct) must be 0.</remarks>
        Reinterpret,

        /// <summary>
        ///     Casts the entire array to TDestStruct by reading propertiesPerItem properties. See remarks.
        /// </summary>
        /// <remarks>
        ///     propertiesPerItem describes how many doubles to copy to and per TDestStruct. <br></br>If -1 is specified then sizeof(TDestStruct)/sizeof(double) will be used.<br></br><br></br>
        ///     If TDestStruct has 2 double fields and propertiesPerItem specified is 1 then all new TDestStruct will have only their first field written to and rest are zero'ed.
        /// </remarks>
        Cast,
    }
}