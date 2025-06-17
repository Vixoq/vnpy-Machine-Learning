﻿/*
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
    /// Shortcut date format strings
    /// </summary>
    internal static class DateFormat {
        /// Year-Month-Date 6 Character Date Representation
        public const string SixCharacter = "yyMMdd";

        /// YYYY-MM-DD Eight Character Date Representation
        public const string EightCharacter = "yyyyMMdd";

        /// Daily and hourly time format
        public const string TwelveCharacter = "yyyyMMdd HH:mm";

        /// JSON Format Date Representation
        public static string JsonFormat = "yyyy-MM-ddTHH:mm:ss";

        /// MySQL Format Date Representation
        public const string DB = "yyyy-MM-dd HH:mm:ss";

        /// QuantConnect UX Date Representation
        public const string UI = "yyyy-MM-dd HH:mm:ss";

        /// en-US format
        public const string US = "M/d/yyyy h:mm:ss tt";

        /// Date format of QC forex data
        public const string Forex = "yyyyMMdd HH:mm:ss.ffff";

        /// YYYYMM Year and Month Character Date Representation (used for futures)
        public const string YearMonth = "yyyyMM";
    }
}