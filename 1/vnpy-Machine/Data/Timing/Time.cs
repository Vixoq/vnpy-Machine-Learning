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
using static FinanceSharp.StringExtensions;

namespace FinanceSharp {
    /// <summary>
    /// 	 Time helper class collection for working with trading dates
    /// </summary>
    public static partial class Time {
        /// <summary>
        /// 	 Provides a value far enough in the future the current computer hardware will have decayed :)
        /// </summary>
        /// <value>
        /// 	 new DateTime(2050, 12, 31)
        /// </value>
        public static readonly DateTime EndOfTime = new DateTime(2050, 12, 31);

        /// <summary>
        /// 	 Provides a time span based on <see cref="EndOfTime"/>
        /// </summary>
        public static TimeSpan EndOfTimeTimeSpan = new TimeSpan(EndOfTime.Ticks);

        /// <summary>
        /// 	 Provides a value far enough in the past that can be used as a lower bound on dates
        /// </summary>
        /// <value>
        /// 	 DateTime.FromOADate(0)
        /// </value>
        public static readonly DateTime BeginningOfTime = DateTime.FromOADate(0);

        /// <summary>
        /// 	 Provides a value large enough that we won't hit the limit, while small enough
        /// 	 we can still do math against it without checking everywhere for <see cref="TimeSpan.MaxValue"/>
        /// </summary>
        public static readonly TimeSpan MaxTimeSpan = TimeSpan.FromDays(1000 * 365);

        /// <summary>
        /// 	 One Year TimeSpan Period Constant
        /// </summary>
        /// <remarks>365 days</remarks>
        public static readonly TimeSpan OneYear = TimeSpan.FromDays(365);

        /// <summary>
        /// 	 One Day TimeSpan Period Constant
        /// </summary>
        public static readonly TimeSpan OneDay = TimeSpan.FromDays(1);

        /// <summary>
        /// 	 One Hour TimeSpan Period Constant
        /// </summary>
        public static readonly TimeSpan OneHour = TimeSpan.FromHours(1);

        /// <summary>
        /// 	 One Minute TimeSpan Period Constant
        /// </summary>
        public static readonly TimeSpan OneMinute = TimeSpan.FromMinutes(1);

        /// <summary>
        /// 	 One Second TimeSpan Period Constant
        /// </summary>
        public static readonly TimeSpan OneSecond = TimeSpan.FromSeconds(1);

        /// <summary>
        /// 	 One Millisecond TimeSpan Period Constant
        /// </summary>
        public static readonly TimeSpan OneMillisecond = TimeSpan.FromMilliseconds(1);

        /// <summary>
        /// 	 Live charting is sensitive to timezone so need to convert the local system time to a UTC and display in browser as UTC.
        /// </summary>
        public struct DateTimeWithZone {
            private readonly DateTime utcDateTime;
            private readonly TimeZoneInfo timeZone;

            /// <summary>
            /// 	 Initializes a new instance of the <see cref="DateTimeWithZone"/> struct.
            /// </summary>
            /// <param name="dateTime">Date time.</param>
            /// <param name="timeZone">Time zone.</param>
            public DateTimeWithZone(DateTime dateTime, TimeZoneInfo timeZone) {
                utcDateTime = TimeZoneInfo.ConvertTimeToUtc(dateTime, timeZone);
                this.timeZone = timeZone;
            }

            /// <summary>
            /// 	 Gets the universal time.
            /// </summary>
            /// <value>The universal time.</value>
            public DateTime UniversalTime {
                get { return utcDateTime; }
            }

            /// <summary>
            /// 	 Gets the time zone.
            /// </summary>
            /// <value>The time zone.</value>
            public TimeZoneInfo TimeZone {
                get { return timeZone; }
            }

            /// <summary>
            /// 	 Gets the local time.
            /// </summary>
            /// <value>The local time.</value>
            public DateTime LocalTime {
                get { return TimeZoneInfo.ConvertTime(utcDateTime, timeZone); }
            }
        }

        private static readonly DateTime EpochTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        /// <summary>
        /// 	 Create a C# DateTime from a UnixTimestamp
        /// </summary>
        /// <param name="unixTimeStamp">Double unix timestamp (Time since Midnight Jan 1 1970)</param>
        /// <returns>C# date timeobject</returns>
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp) {
            DateTime time;
            try {
                var ticks = unixTimeStamp * TimeSpan.TicksPerSecond;
                time = EpochTime.AddTicks((long) ticks);
            } catch (Exception err) {
                Log.Error(err, Invariant($"UnixTimeStamp: {unixTimeStamp}"));
                time = DateTime.Now;
            }

            return time;
        }

        /// <summary>
        /// 	 Create a C# DateTime from a UnixTimestamp
        /// </summary>
        /// <param name="unixTimeStamp">Double unix timestamp (Time since Midnight Jan 1 1970) in milliseconds</param>
        /// <returns>C# date timeobject</returns>
        public static DateTime UnixMillisecondTimeStampToDateTime(double unixTimeStamp) {
            DateTime time;
            try {
                var ticks = unixTimeStamp * TimeSpan.TicksPerMillisecond;
                time = EpochTime.AddTicks((long) ticks);
            } catch (Exception err) {
                Log.Error(err, Invariant($"UnixTimeStamp: {unixTimeStamp}"));
                time = DateTime.Now;
            }

            return time;
        }

        /// <summary>
        /// 	 Convert a Datetime to Unix Timestamp
        /// </summary>
        /// <param name="time">C# datetime object</param>
        /// <returns>Double unix timestamp</returns>
        public static double DateTimeToUnixTimeStamp(DateTime time) {
            double timestamp = 0;
            try {
                timestamp = (time - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds;
            } catch (Exception err) {
                Log.Error(err, Invariant($"{time:o}"));
            }

            return timestamp;
        }

        /// <summary>
        /// 	 Get the current time as a unix timestamp
        /// </summary>
        /// <returns>Double value of the unix as UTC timestamp</returns>
        public static double TimeStamp() {
            return DateTimeToUnixTimeStamp(DateTime.UtcNow);
        }

        /// <summary>
        /// 	 Returns the timespan with the larger value
        /// </summary>
        public static TimeSpan Max(TimeSpan one, TimeSpan two) {
            return TimeSpan.FromTicks(Math.Max(one.Ticks, two.Ticks));
        }

        /// <summary>
        /// 	 Returns the timespan with the smaller value
        /// </summary>
        public static TimeSpan Min(TimeSpan one, TimeSpan two) {
            return TimeSpan.FromTicks(Math.Min(one.Ticks, two.Ticks));
        }

        /// <summary>
        /// 	 Returns the larger of two date times
        /// </summary>
        public static DateTime Max(DateTime one, DateTime two) {
            return one > two ? one : two;
        }

        /// <summary>
        /// 	 Returns the smaller of two date times
        /// </summary>
        public static DateTime Min(DateTime one, DateTime two) {
            return one < two ? one : two;
        }

        /// <summary>
        /// 	 Multiplies the specified interval by the multiplier
        /// </summary>
        /// <param name="interval">The interval to be multiplied, such as TimeSpan.FromSeconds(1)</param>
        /// <param name="multiplier">The number of times to multiply the interval</param>
        /// <returns>The multiplied interval, such as 1s*5 = 5s</returns>
        public static TimeSpan Multiply(this TimeSpan interval, double multiplier) {
            return TimeSpan.FromTicks((long) (interval.Ticks * multiplier));
        }

        /// <summary>
        /// 	 Normalizes the current time within the specified period
        /// 	 time = start => 0
        /// 	 time = start + period => 1
        /// </summary>
        /// <param name="start">The start time of the range</param>
        /// <param name="current">The current time we seek to normalize</param>
        /// <param name="period">The time span of the range</param>
        /// <returns>The normalized time</returns>
        public static double NormalizeInstantWithinRange(DateTime start, DateTime current, TimeSpan period) {
            // normalization of a point time only has a value at that specific point
            if (period == TimeSpan.Zero) {
                return start == current ? 1 : 0;
            }

            var delta = (current - start).TotalSeconds;
            return delta / period.TotalSeconds;
        }

        /// <summary>
        /// 	 Normalizes the step size as a percentage of the period.
        /// </summary>
        /// <param name="period">The period to normalize against</param>
        /// <param name="stepSize">The step size to be normaized</param>
        /// <returns>The normalized step size as a percentage of the period</returns>
        public static double NormalizeTimeStep(TimeSpan period, TimeSpan stepSize) {
            // normalization of a time step for an instantaneous period will always be zero
            if (period == TimeSpan.Zero) {
                return 0;
            }

            return stepSize.TotalSeconds / period.TotalSeconds;
        }

        /// <summary>
        /// 	 Gets the absolute value of the specified time span
        /// </summary>
        /// <param name="timeSpan">Time span whose absolute value we seek</param>
        /// <returns>The absolute value of the specified time span</returns>
        public static TimeSpan Abs(this TimeSpan timeSpan) {
            return TimeSpan.FromTicks(Math.Abs(timeSpan.Ticks));
        }
    }
}