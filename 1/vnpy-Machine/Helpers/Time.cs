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
using System.Globalization;

// ReSharper disable once CheckNamespace
namespace FinanceSharp {
    /// <summary>
    ///     Conversion methods for epoch time and other time formats.
    /// </summary>
    public static partial class Time {
        public static readonly DateTime EpochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        ///     Provides UTC time now in milliseconds-epoch time.
        /// </summary>
        public static long Now => (long) DateTime.UtcNow.Subtract(EpochStart).TotalMilliseconds;

        /// <summary>
        ///     Provides local time now in milliseconds-epoch time.
        /// </summary>
        public static long LocalNow => (long) new DateTime(DateTime.Now.Ticks, DateTimeKind.Utc).Subtract(EpochStart).TotalMilliseconds;

        /// <summary>
        ///     Converts <see cref="TimeSpan"/> to milliseconds-epoch time represented by <see cref="long"/>.
        /// </summary>
        public static long ToEpochTime(this TimeSpan ts) {
            return (long) ts.TotalMilliseconds;
        }

        /// <summary>
        ///     Converts <see cref="DateTime"/> to milliseconds-epoch time represented by <see cref="long"/>.
        /// </summary>
        public static long ToEpochTime(this DateTime ts) {
            return (long) new DateTime(ts.Ticks, DateTimeKind.Utc).Subtract(EpochStart).TotalMilliseconds;
        }

        /// <summary>
        ///     Converts <see cref="DateTime"/> to milliseconds-epoch time represented by <see cref="long"/>.
        /// </summary>
        public static long ToEpochTime(this DateTimeOffset ts) {
            return (long) new DateTime(ts.Ticks, DateTimeKind.Utc).Subtract(EpochStart).TotalMilliseconds;
        }

        /// <summary>
        ///     Converts milliseconds-epoch time to <see cref="DateTime"/> represented by <see cref="long"/>.
        /// </summary>
        public static DateTime ToDateTime(this long epochMilliseconds) {
            return EpochStart.AddMilliseconds(epochMilliseconds);
        }

        /// <summary>
        /// Parse a standard YY MM DD date into a DateTime. Attempt common date formats
        /// </summary>
        /// <param name="dateToParse">String date time to parse</param>
        /// <returns>Date time</returns>
        internal static DateTime ParseDate(string dateToParse) {
            try {
                //First try the exact options:
                DateTime date;
                if (DateTime.TryParseExact(dateToParse, DateFormat.SixCharacter, CultureInfo.InvariantCulture, DateTimeStyles.None, out date)) {
                    return date;
                }

                if (DateTime.TryParseExact(dateToParse, DateFormat.EightCharacter, CultureInfo.InvariantCulture, DateTimeStyles.None, out date)) {
                    return date;
                }

                if (DateTime.TryParseExact(dateToParse.Substring(0, Math.Min(19, dateToParse.Length)), DateFormat.JsonFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date)) {
                    return date;
                }

                if (DateTime.TryParseExact(dateToParse, DateFormat.US, CultureInfo.InvariantCulture, DateTimeStyles.None, out date)) {
                    return date;
                }

                if (DateTime.TryParse(dateToParse, out date)) {
                    return date;
                }
            } catch (Exception err) {
                Log.Error(err);
            }

            return DateTime.Now;
        }
    }
}