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
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using FinanceSharp.Delegates;
using FinanceSharp.Graphing;
using FinanceSharp.Helpers;
using MathNet.Numerics.Providers.LinearAlgebra.OpenBlas;
using static FinanceSharp.Constants;


namespace FinanceSharp.Indicators {
    /// <summary>
    /// 	 Provides extension methods for Indicator
    /// </summary>
    public static class IndicatorExtensions {
        /// <summary>
        /// 	 Configures the second indicator to receive automatic updates from the first by attaching an event handler
        /// 	 to first.Updated
        /// </summary>
        /// <param name="second">The indicator that receives data from the first</param>
        /// <param name="first">The indicator that sends data via <see cref="IUpdatable.Updated"/> even to the second</param>
        /// <param name="waitForFirstToReady">True to only send updates to the second if first.IsReady returns true, false to alway send updates to second</param>
        /// <returns>The reference to the second indicator to allow for method chaining</returns>
        public static T Of<T>(this T second, IUpdatable first, bool waitForFirstToReady = true)
            where T : IUpdatable {
            if (waitForFirstToReady) {
                first.Updated += (time, consolidated) => {
                    // only send the data along if we're ready
                    if (first.IsReady) {
                        second.Update(time, consolidated);
                    }
                };
            } else {
                first.Updated += (time, consolidated)
                    => second.Update(time, consolidated);
            }

            first.Resetted += sender => second.Reset();

            return second;
        }

        /// <summary>
        /// 	 Configures the second indicator to receive automatic updates from the first by attaching an event handler
        /// 	 to first.Updated
        /// </summary>
        /// <param name="second">The indicator that receives data from the first</param>
        /// <param name="first">The indicator that sends data via <see cref="IUpdatable.Updated"/> even to the second</param>
        /// <param name="waitForFirstToReady">True to only send updates to the second if first.IsReady returns true, false to alway send updates to second</param>
        /// <returns>The reference to the second indicator to allow for method chaining</returns>
        public static T Then<T>(this IUpdatable first, T second, bool waitForFirstToReady = true)
            where T : IUpdatable {
            if (waitForFirstToReady) {
                first.Updated += (time, consolidated) => {
                    // only send the data along if we're ready
                    if (first.IsReady) {
                        second.Update(time, consolidated);
                    }
                };
            } else {
                first.Updated += (time, consolidated)
                    => second.Update(time, consolidated);
            }

            first.Resetted += sender => second.Reset();

            return second;
        }

        private static string ResolveName(IUpdatable first, string argumentedName) {
            if (!string.IsNullOrEmpty(argumentedName))
                return argumentedName;
            if (first is IIndicator ind) {
                argumentedName = $"function({ind.Name})";
            } else {
                argumentedName = $"function({first.GetType().Name})";
            }

            return argumentedName;
        }

        /// <summary>
        ///     Performs a math <paramref name="op"/> over <see cref="DoubleArray.Value"/> or value from <paramref name="selector"/>. The result is stored in a <see cref="Identity"/>.
        /// </summary>
        /// <param name="first">The indicator that sends data via <see cref="IUpdatable.Updated"/> even to the math <paramref name="op"/></param>
        /// <param name="op">The operation to perform on <see cref="DoubleArray.Value"/>.</param>
        /// <param name="selector">A selector to choose what <see cref="double"/> to pass to math <paramref name="op"/>. By default <see cref="DoubleArray.Value"/> is used.</param>
        /// <param name="waitForFirstToReady">First must be ready in order to push the updates forward.</param>
        /// <param name="name">Name of the new returned <see cref="Identity"/> representing the <paramref name="op"/>.</param>
        public static Identity Function(this IUpdatable first, UnaryFunctionHandler op, SelectorFunctionHandler selector = null, bool waitForFirstToReady = true, string name = null) {
            if (op == null) throw new ArgumentNullException(nameof(op));

            var idn = new Identity(ResolveName(first, name));


            if (selector == null) {
                if (waitForFirstToReady) {
                    first.Updated += (time, updated) => {
                        if (first.IsReady)
                            idn.Update(time, op(updated.Value));
                    };
                } else
                    first.Updated += (time, updated) => idn.Update(time, op(updated.Value));
            } else {
                if (waitForFirstToReady) {
                    first.Updated += (time, updated) => {
                        if (first.IsReady)
                            idn.Update(time, new DoubleArrayScalar(op(selector(updated))));
                    };
                } else
                    first.Updated += (time, updated) => idn.Update(time, new DoubleArrayScalar(op(selector(updated))));
            }

            first.Resetted += sender => idn.Reset();

            return idn;
        }

        /// <summary>
        ///     Performs a math <paramref name="op"/> over <see cref="DoubleArray"/> or the value from <paramref name="selector"/>. The result is stored in a <see cref="Identity"/>.
        /// </summary>
        /// <param name="first">The indicator that sends data via <see cref="IUpdatable.Updated"/> even to the math <paramref name="op"/></param>
        /// <param name="op">The operation to perform on the <see cref="DoubleArray"/> passed from <paramref name="first"/>.</param>
        /// <param name="selector">A selector to choose what <see cref="DoubleArray"/> to pass to math <paramref name="op"/>. By default, the unchanged <see cref="DoubleArray"/> is used.</param>
        /// <param name="waitForFirstToReady">First must be ready in order to push the updates forward.</param>
        /// <param name="name">Name of the new returned <see cref="Identity"/> representing the <paramref name="op"/>.</param>
        public static Identity Function(this IUpdatable first, UnaryArrayFunctionHandler op, ArraySelectorFunctionHandler selector = null, bool waitForFirstToReady = true, string name = null) {
            if (op == null) throw new ArgumentNullException(nameof(op));

            var idn = new Identity(ResolveName(first, name));
            if (selector == null) {
                if (waitForFirstToReady) {
                    first.Updated += (time, updated) => {
                        if (first.IsReady)
                            idn.Update(time, op(updated));
                    };
                } else
                    first.Updated += (time, updated) => idn.Update(time, op(updated));
            } else {
                if (waitForFirstToReady) {
                    first.Updated += (time, updated) => {
                        if (first.IsReady)
                            idn.Update(time, op(selector(updated)));
                    };
                } else
                    first.Updated += (time, updated) => idn.Update(time, op(selector(updated)));
            }

            first.Resetted += sender => idn.Reset();

            return idn;
        }

        /// <summary>
        ///     Selects a <see cref="DoubleArray"/> to a <see cref="double"/>. The result is stored in a <see cref="Identity"/>.
        /// </summary>
        /// <param name="input">The indicator that sends data via <see cref="IUpdatable.Updated"/></param>
        /// <param name="selector">A selector to choose a <see cref="double"/>. </param>
        /// <param name="waitForFirstToReady">Input must be ready in order to push the updates forward.</param>
        /// <param name="name">Name of the new returned <see cref="Identity"/>.</param>
        public static Identity Select(this IUpdatable input, SelectorFunctionHandler selector, bool waitForFirstToReady = true, string name = null) {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            var idn = new Identity(ResolveName(input, name), setReady: true);

            if (input.OutputCount > 1) {
                unsafe {
                    if (waitForFirstToReady) {
                        input.Updated += (time, updated) => {
                            if (input.IsReady) {
                                var ret = new DoubleArray2DManaged(updated.Count, 1);
                                fixed (double* dst = ret, src = updated)
                                    for (int i = 0; i < updated.Count; i++)
                                        dst[i] = selector(src[i * updated.Properties]);

                                idn.Update(time, ret);
                            }
                        };
                    } else {
                        input.Updated += (time, updated) => {
                            var ret = new DoubleArray2DManaged(updated.Count, 1);
                            fixed (double* dst = ret, src = updated)
                                for (int i = 0; i < updated.Count; i++)
                                    dst[i] = selector(src[i * updated.Properties]);

                            idn.Update(time, ret);
                        };
                    }

                    input.Resetted += sender => idn.Reset();
                }
            } else {
                if (waitForFirstToReady) {
                    input.Updated += (time, updated) => {
                        if (input.IsReady)
                            idn.Update(time, new DoubleArrayScalar(selector(updated)));
                    };
                } else {
                    input.Updated += (time, updated) => idn.Update(time, new DoubleArrayScalar(selector(updated)));
                }

                input.Resetted += sender => idn.Reset();
            }

            return idn;
        }

        /// <summary>
        ///     Selects a <see cref="DoubleArray"/> to another <see cref="DoubleArray"/>. The result is stored in a <see cref="Identity"/>.
        /// </summary>
        /// <param name="input">The indicator that sends data via <see cref="IUpdatable.Updated"/></param>
        /// <param name="selector">A selector to choose what <see cref="DoubleArray"/>.</param>
        /// <param name="waitForFirstToReady">Input must be ready in order to push the updates forward.</param>
        /// <param name="name">Name of the new returned <see cref="Identity"/>.</param>
        public static Identity Select(this IUpdatable input, ArraySelectorFunctionHandler selector, int outputCount = 1, int properties = 1, bool waitForFirstToReady = true, string name = null) {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            outputCount = outputCount > 0 ? outputCount : input.OutputCount;
            var idn = new Identity(ResolveName(input, name), outputCount, properties);

            if (outputCount > 1) {
                unsafe {
                    if (waitForFirstToReady) {
                        input.Updated += (time, updated) => {
                            if (input.IsReady) {
                                var ret = new DoubleArray2DManaged(outputCount, properties);
                                fixed (double* dst = ret, selectSrc = updated)
                                    for (int i = 0; i < updated.Count; i++) {
                                        var result = selector(selectSrc[i * updated.Properties]);
                                        Guard.AssertTrue(result.Count == outputCount, "Passed outputCount must be matching to the updated array.");
                                        Guard.AssertTrue(properties == result.Properties, "Properties must match the properties argument passed.");
                                        fixed (double* src = result)
                                            Unsafe.CopyBlock(dst + i, src, (uint) (result.LinearLength * sizeof(double)));
                                    }

                                idn.Update(time, ret);
                            }
                        };
                    } else {
                        input.Updated += (time, updated) => {
                            var ret = new DoubleArray2DManaged(updated.Count, properties);
                            fixed (double* dst = ret, selectSrc = updated)
                                for (int i = 0; i < outputCount; i++) {
                                    var result = selector(selectSrc[i * updated.Properties]);
                                    Guard.AssertTrue(result.Count == outputCount, "Passed outputCount must be matching to the updated array.");
                                    Guard.AssertTrue(properties == result.Properties, "Properties must match the properties argument passed.");
                                    fixed (double* src = result)
                                        Unsafe.CopyBlock(dst, src, (uint) (result.LinearLength * sizeof(double)));
                                }

                            idn.Update(time, ret);
                        };
                    }

                    input.Resetted += sender => idn.Reset();
                }
            }

            if (waitForFirstToReady) {
                input.Updated += (time, updated) => {
                    if (input.IsReady)
                        idn.Update(time, selector(updated));
                };
            } else
                input.Updated += (time, updated) => idn.Update(time, selector(updated));

            input.Resetted += sender => idn.Reset();

            return idn;
        }

        /// <summary>
        /// 	 Will collect updates from <paramref name="first"/> into the returned <see cref="List{T}"/>.
        /// </summary>
        /// <param name="first">The indicator that sends data via <see cref="IUpdatable.Updated"/> even to the second</param>
        /// <param name="waitForFirstToReady">True to only send updates to the second if first.IsReady returns true, false to alway send updates to second</param>
        /// <param name="resetListOnIndicatorReset">Should the returned <see cref="List{T}"/> be cleared when <paramref name="first"/> is Resetted.</param>
        public static List<DoubleArray> ThenToList(this IUpdatable first, bool waitForFirstToReady = true, bool resetListOnIndicatorReset = false) {
            var ret = new List<DoubleArray>();
            if (waitForFirstToReady) {
                first.Updated += (time, consolidated) => {
                    // only send the data along if we're ready
                    if (first.IsReady) {
                        ret.Add(consolidated);
                    }
                };
            } else {
                first.Updated += (time, consolidated)
                    => ret.Add(consolidated);
            }

            if (resetListOnIndicatorReset)
                first.Resetted += sender => ret.Clear();

            return ret;
        }


        /// <summary>
        /// 	 Creates a new CompositeIndicator such that the result will be average of a first indicator weighted by a second one
        /// </summary>
        /// <param name="value">Indicator that will be averaged</param>
        /// <param name="weight">Indicator that provides the average weights</param>
        /// <param name="period">Average period</param>
        /// <param name="method">What method to composite the two indicators.</param>
        /// <returns>Indicator that results of the average of first by weights given by second</returns>
        public static CompositeIndicator WeightedBy<TWeight>(this IUpdatable value, TWeight weight, int period, CompositionMethod method = CompositionMethod.OnBothUpdated)
            where TWeight : IUpdatable {
            var x = new WindowIdentity(period);
            var y = new WindowIdentity(period);
            var numerator = new Sum("Sum_xy", period);
            var denominator = new Sum("Sum_y", period);

            value.Updated += (time, consolidated) => {
                x.Update(time, consolidated);
                if (x.Samples == y.Samples) {
                    numerator.Update(time, consolidated * y.Current[0, C]);
                }
            };

            weight.Updated += (time, consolidated) => {
                y.Update(time, consolidated);
                if (x.Samples == y.Samples) {
                    numerator.Update(time, consolidated * x.Current[0, C]);
                }

                denominator.Update(time, consolidated);
            };

            value.Resetted += sender => weight.Reset();

            return numerator.Over((IUpdatable) denominator, method);
        }

        /// <summary>
        /// 	 Creates a new CompositeIndicator such that the result will be the sum of the left and the constant
        /// </summary>
        /// <remarks>
        /// 	 value = left + constant
        /// </remarks>
        /// <param name="left">The left indicator</param>
        /// <param name="constant">The addend</param>
        /// <returns>The sum of the left and right indicators</returns>
        public static CompositeIndicator Plus(this IUpdatable left, double constant) {
            var constantIndicator = new ConstantIndicator(constant.ToString(CultureInfo.InvariantCulture), constant);
            return left.Plus((IUpdatable) constantIndicator);
        }

        /// <summary>
        /// 	 Creates a new CompositeIndicator such that the result will be the sum of the left and right
        /// </summary>
        /// <remarks>
        /// 	 value = left + right
        /// </remarks>
        /// <param name="left">The left indicator</param>
        /// <param name="right">The right indicator</param>
        /// <param name="method">What method to composite the two indicators.</param>
        /// <returns>The sum of the left and right indicators</returns>
        public static CompositeIndicator Plus(this IUpdatable left, IUpdatable right, CompositionMethod method = CompositionMethod.OnBothUpdated) {
            if (AreDoubleScalar(left, right))
                return new CompositeIndicator(left, right, (l, r) => l.Current.Value + r.Current.Value, method);

            return new CompositeIndicator(left, right, (l, r) => l.Current + r.Current, method);
        }

        /// <summary>
        /// 	 Creates a new CompositeIndicator such that the result will be the sum of the left and right
        /// </summary>
        /// <remarks>
        /// 	 value = left + right
        /// </remarks>
        /// <param name="left">The left indicator</param>
        /// <param name="right">The right indicator</param>
        /// <param name="method">What method to composite the two indicators.</param>
        /// <returns>The sum of the left and right indicators</returns>
        public static CompositeIndicator Plus(this IndicatorBase left, IndicatorBase right, CompositionMethod method = CompositionMethod.OnBothUpdated) {
            if (AreDoubleScalar(left, right))
                return new CompositeIndicator(left, right, (l, r) => l.Current.Value + r.Current.Value, method);

            return new CompositeIndicator(left, right, (l, r) => l.Current + r.Current, method);
        }

        /// <summary>
        /// 	 Creates a new CompositeIndicator such that the result will be the sum of the left and right
        /// </summary>
        /// <remarks>
        /// 	 value = left + right
        /// </remarks>
        /// <param name="left">The left indicator</param>
        /// <param name="right">The right indicator</param>
        /// <param name="name">The name of this indicator</param>
        /// <param name="method">What method to composite the two indicators.</param>
        /// <returns>The sum of the left and right indicators</returns>
        public static CompositeIndicator Plus(this IUpdatable left, IUpdatable right, string name, CompositionMethod method = CompositionMethod.OnBothUpdated) {
            if (AreDoubleScalar(left, right))
                return new CompositeIndicator(left, right, (l, r) => l.Current.Value + r.Current.Value, method);

            return new CompositeIndicator(name, left, right, (l, r) => l.Current + r.Current, method);
        }

        /// <summary>
        /// 	 Creates a new CompositeIndicator such that the result will be the difference of the left and constant
        /// </summary>
        /// <remarks>
        /// 	 value = left - constant
        /// </remarks>
        /// <param name="left">The left indicator</param>
        /// <param name="constant">The subtrahend</param>
        /// <returns>The difference of the left and right indicators</returns>
        public static CompositeIndicator Minus(this IUpdatable left, double constant) {
            var constantIndicator = new ConstantIndicator(constant.ToString(CultureInfo.InvariantCulture), constant);
            return left.Minus((IUpdatable) constantIndicator);
        }

        /// <summary>
        /// 	 Creates a new CompositeIndicator such that the result will be the difference of the left and right
        /// </summary>
        /// <remarks>
        /// 	 value = left - right
        /// </remarks>
        /// <param name="left">The left indicator</param>
        /// <param name="right">The right indicator</param>
        /// <param name="method">What method to composite the two indicators.</param>
        /// <returns>The difference of the left and right indicators</returns>
        public static CompositeIndicator Minus(this IUpdatable left, IUpdatable right, CompositionMethod method = CompositionMethod.OnBothUpdated) {
            if (AreDoubleScalar(left, right))
                return new CompositeIndicator(left, right, (l, r) => l.Current.Value - r.Current.Value, method);

            return new CompositeIndicator(left, right, (l, r) => l.Current - r.Current, method);
        }

        /// <summary>
        /// 	 Creates a new CompositeIndicator such that the result will be the difference of the left and right
        /// </summary>
        /// <remarks>
        /// 	 value = left - right
        /// </remarks>
        /// <param name="left">The left indicator</param>
        /// <param name="right">The right indicator</param>
        /// <param name="method">What method to composite the two indicators.</param>
        /// <returns>The difference of the left and right indicators</returns>
        public static CompositeIndicator Minus(this IndicatorBase left, IndicatorBase right, CompositionMethod method = CompositionMethod.OnBothUpdated) {
            if (AreDoubleScalar(left, right))
                return new CompositeIndicator(left, right, (l, r) => l.Current.Value - r.Current.Value, method);

            return new CompositeIndicator(left, right, (l, r) => l.Current - r.Current, method);
        }

        /// <summary>
        /// 	 Creates a new CompositeIndicator such that the result will be the difference of the left and right
        /// </summary>
        /// <remarks>
        /// 	 value = left - right
        /// </remarks>
        /// <param name="left">The left indicator</param>
        /// <param name="right">The right indicator</param>
        /// <param name="name">The name of this indicator</param>
        /// <param name="method">What method to composite the two indicators.</param>
        /// <returns>The difference of the left and right indicators</returns>
        public static CompositeIndicator Minus(this IUpdatable left, IUpdatable right, string name, CompositionMethod method = CompositionMethod.OnBothUpdated) {
            if (AreDoubleScalar(left, right))
                return new CompositeIndicator(left, right, (l, r) => l.Current.Value - r.Current.Value, method);
            return new CompositeIndicator(name, left, right, (l, r) => l.Current - r.Current, method);
        }

        /// <summary>
        /// 	 Creates a new CompositeIndicator such that the result will be the ratio of the left to the constant
        /// </summary>
        /// <remarks>
        /// 	 value = left/constant
        /// </remarks>
        /// <param name="left">The left indicator</param>
        /// <param name="constant">The constant value denominator</param>
        /// <returns>The ratio of the left to the right indicator</returns>
        public static CompositeIndicator Over(this IUpdatable left, double constant) {
            var constantIndicator = new ConstantIndicator(constant.ToString(CultureInfo.InvariantCulture), constant);
            return left.Over((IUpdatable) constantIndicator);
        }

        /// <summary>
        /// 	 Creates a new CompositeIndicator such that the result will be the ratio of the left to the right
        /// </summary>
        /// <remarks>
        /// 	 value = left/right
        /// </remarks>
        /// <param name="left">The left indicator</param>
        /// <param name="right">The right indicator</param>
        /// <param name="method">What method to composite the two indicators.</param>
        /// <returns>The ratio of the left to the right indicator</returns>
        public static CompositeIndicator Over(this IUpdatable left, IUpdatable right, CompositionMethod method = CompositionMethod.OnBothUpdated) {
            if (AreDoubleScalar(left, right))
                return new CompositeIndicator(left, right, (l, r) => r.Current == Constants.Zero ? new IndicatorResult(Constants.Zero, IndicatorStatus.MathError) : new IndicatorResult(l.Current.Value / r.Current.Value), method);
            return new CompositeIndicator(left, right, (l, r) => r.Current == Constants.Zero ? new IndicatorResult(Constants.Zero, IndicatorStatus.MathError) : new IndicatorResult(l.Current / r.Current), method);
        }

        /// <summary>
        /// 	 Creates a new CompositeIndicator such that the result will be the ratio of the left to the right
        /// </summary>
        /// <remarks>
        /// 	 value = left/right
        /// </remarks>
        /// <param name="left">The left indicator</param>
        /// <param name="right">The right indicator</param>
        /// <param name="method">What method to composite the two indicators.</param>
        /// <returns>The ratio of the left to the right indicator</returns>
        public static CompositeIndicator Over(this IndicatorBase left, IndicatorBase right, CompositionMethod method = CompositionMethod.OnBothUpdated) {
            if (AreDoubleScalar(left, right))
                return new CompositeIndicator(left, right, (l, r) => r.Current == Constants.Zero ? new IndicatorResult(Constants.Zero, IndicatorStatus.MathError) : new IndicatorResult(l.Current.Value / r.Current.Value), method);

            return new CompositeIndicator(left, right, (l, r) => r.Current == Constants.Zero ? new IndicatorResult(Constants.Zero, IndicatorStatus.MathError) : new IndicatorResult(l.Current / r.Current), method);
        }

        /// <summary>
        /// 	 Creates a new CompositeIndicator such that the result will be the ratio of the left to the right
        /// </summary>
        /// <remarks>
        /// 	 value = left/right
        /// </remarks>
        /// <param name="left">The left indicator</param>
        /// <param name="right">The right indicator</param>
        /// <param name="name">The name of this indicator</param>
        /// <param name="method">What method to composite the two indicators.</param>
        /// <returns>The ratio of the left to the right indicator</returns>
        public static CompositeIndicator Over(this IUpdatable left, IUpdatable right, string name, CompositionMethod method = CompositionMethod.OnBothUpdated) {
            if (AreDoubleScalar(left, right))
                return new CompositeIndicator(left, right, (l, r) => r.Current == Constants.Zero ? new IndicatorResult(Constants.Zero, IndicatorStatus.MathError) : new IndicatorResult(l.Current.Value / r.Current.Value), method);

            return new CompositeIndicator(name, left, right, (l, r) => r.Current == Constants.Zero ? new IndicatorResult(Constants.Zero, IndicatorStatus.MathError) : new IndicatorResult(l.Current / r.Current), method);
        }

        /// <summary>
        /// 	 Creates a new CompositeIndicator such that the result will be the product of the left and the constant
        /// </summary>
        /// <remarks>
        /// 	 value = left*constant
        /// </remarks>
        /// <param name="left">The left indicator</param>
        /// <param name="constant">The constant value to multiple by</param>
        /// <returns>The product of the left to the right indicators</returns>
        public static CompositeIndicator Times(this IUpdatable left, double constant) {
            var constantIndicator = new ConstantIndicator(constant.ToString(CultureInfo.InvariantCulture), constant);
            return left.Times((IUpdatable) constantIndicator);
        }

        /// <summary>
        /// 	 Creates a new CompositeIndicator such that the result will be the product of the left to the right
        /// </summary>
        /// <remarks>
        /// 	 value = left*right
        /// </remarks>
        /// <param name="left">The left indicator</param>
        /// <param name="right">The right indicator</param>
        /// <param name="method">What method to composite the two indicators.</param>
        /// <returns>The product of the left to the right indicators</returns>
        public static CompositeIndicator Times(this IUpdatable left, IUpdatable right, CompositionMethod method = CompositionMethod.OnBothUpdated) {
            if (AreDoubleScalar(left, right))
                return new CompositeIndicator(left, right, (l, r) => l.Current.Value * r.Current.Value, method);
            return new CompositeIndicator(left, right, (l, r) => l.Current * r.Current, method);
        }

        /// <summary>
        /// 	 Creates a new CompositeIndicator such that the result will be the product of the left to the right
        /// </summary>
        /// <remarks>
        /// 	 value = left*right
        /// </remarks>
        /// <param name="left">The left indicator</param>
        /// <param name="right">The right indicator</param>
        /// <param name="method">What method to composite the two indicators.</param>
        /// <returns>The product of the left to the right indicators</returns>
        public static CompositeIndicator Times(this IndicatorBase left, IndicatorBase right, CompositionMethod method = CompositionMethod.OnBothUpdated) {
            if (AreDoubleScalar(left, right))
                return new CompositeIndicator(left, right, (l, r) => l.Current.Value * r.Current.Value, method);
            return new CompositeIndicator(left, right, (l, r) => l.Current * r.Current, method);
        }

        /// <summary>
        /// 	 Creates a new CompositeIndicator such that the result will be the product of the left to the right
        /// </summary>
        /// <remarks>
        /// 	 value = left * right
        /// </remarks>
        /// <param name="left">The left indicator</param>
        /// <param name="right">The right indicator</param>
        /// <param name="name">The name of this indicator</param>
        /// <param name="method">What method to composite the two indicators.</param>
        /// <returns>The product of the left to the right indicators</returns>
        public static CompositeIndicator Times(this IUpdatable left, IUpdatable right, string name, CompositionMethod method = CompositionMethod.OnBothUpdated) {
            if (AreDoubleScalar(left, right))
                return new CompositeIndicator(left, right, (l, r) => l.Current.Value * r.Current.Value, method);
            return new CompositeIndicator(name, left, right, (l, r) => (IndicatorResult) (l.Current * r.Current), method);
        }

        /// <summary>
        /// 	 Creates a <see cref="CountSelector"/> for every <paramref name="input"/>'s <see cref="IUpdatable.OutputCount"/> ordered by ascending index.
        /// </summary>
        /// <param name="input">An indicator</param>
        /// <param name="waitForFirstToReady">True to only send updates to the second if input.IsReady returns true, false to alway send updates to second</param>
        /// <returns>an array of <see cref="CountSelector"/> for every <paramref name="input"/>'s <see cref="IUpdatable.OutputCount"/> ordered by ascending index.</returns>
        public static CountSelector[] Explode(this IUpdatable input, bool waitForFirstToReady = true) {
            var ret = new CountSelector[input.OutputCount];
            for (int i = 0; i < input.OutputCount; i++) {
                ret[i] = new CountSelector(i, $"SELECT(index: {i})").Of(input, waitForFirstToReady);
            }

            return ret;
        }

        /// <summary>
        /// 	 Slices <paramref name="input"/> at range from <paramref name="start"/> to <paramref name="stop"/> on the <see cref="IUpdatable.OutputCount"/> axis/dimension.
        /// </summary>
        /// <param name="start">Start of interval. The interval includes this value. The default start value is 0.</param>
        /// <param name="stop">End of interval. The interval does not include this value, except in some cases where step is not an integer and floating point round-off affects the length of out.</param>
        /// <param name="input">The indicator to slice.</param>
        /// <param name="name">The name of this indicator</param>
        /// <param name="waitForFirstToReady">True to only send updates to the second if input.IsReady returns true, false to alway send updates to second</param>
        /// <returns>A sliced <see cref="IIndicator"/></returns>
        public static SliceSelector Slice(this IUpdatable input, int start, int stop, string name = null, bool waitForFirstToReady = true) {
            return new SliceSelector(start, stop, name ?? $"SELECT(start: {start}, stop: {stop})").Of(input, waitForFirstToReady);
        }

        /// <summary>
        /// 	 Slices <paramref name="input"/> at range from <paramref name="start"/> to <paramref name="stop"/> on the <see cref="IUpdatable.OutputCount"/> axis/dimension.
        /// </summary>
        /// <param name="start">Start of interval. The interval includes this value. The default start value is 0.</param>
        /// <param name="stop">End of interval. The interval does not include this value, except in some cases where step is not an integer and floating point round-off affects the length of out.</param>
        /// <param name="input">The indicator to slice.</param>
        /// <param name="name">The name of this indicator</param>
        /// <param name="waitForFirstToReady">True to only send updates to the second if ןמפוא.IsReady returns true, false to alway send updates to second</param>
        /// <returns>A sliced <see cref="IIndicator"/></returns>
        public static CountSelector Slice(this IUpdatable input, int index, string name = null, bool waitForFirstToReady = true) {
            return new CountSelector(index, name ?? $"SELECT(start: {index}, stop: {index + 1})").Of(input, waitForFirstToReady);
        }

        /// <summary>
        /// 	 Slices properties of <paramref name="input"/> at range from <paramref name="start"/> to <paramref name="stop"/> on the <see cref="IUpdatable.OutputCount"/> axis/dimension.
        /// </summary>
        /// <param name="start">Start of interval. The interval includes this value. The default start value is 0.</param>
        /// <param name="stop">End of interval. The interval does not include this value, except in some cases where step is not an integer and floating point round-off affects the length of out.</param>
        /// <param name="input">The indicator to slice.</param>
        /// <param name="name">The name of this indicator</param>
        /// <param name="waitForFirstToReady">True to only send updates to the second if ןמפוא.IsReady returns true, false to alway send updates to second</param>
        /// <returns>A sliced <see cref="IIndicator"/></returns>
        public static PropertySelector SliceProperty(this IUpdatable input, int start, int stop, string name = null, bool waitForFirstToReady = true) {
            return new PropertySelector(start, stop, name ?? $"SELECT(start: {start}, stop: {stop})").Of(input, waitForFirstToReady);
        }

        /// <summary>
        /// 	 Creates a new CompositeIndicator such that the result will be the product of the left to the right
        /// </summary>
        /// <remarks>
        /// 	 value = left*right
        /// </remarks>
        /// <param name="start">Start of interval. The interval includes this value. The default start value is 0.</param>
        /// <param name="stop">End of interval. The interval does not include this value, except in some cases where step is not an integer and floating point round-off affects the length of out.</param>
        /// <param name="input">The left indicator</param>
        /// <param name="name">The name of this indicator</param>
        /// <param name="waitForFirstToReady">True to only send updates to the second if first.IsReady returns true, false to alway send updates to second</param>
        /// <returns>The product of the left to the right indicators</returns>
        public static PropertySelector SliceProperty(this IUpdatable input, int index, string name = null, bool waitForFirstToReady = true) {
            return new PropertySelector(index, name ?? $"SELECT(start: {index}, stop: {index + 1})").Of(input, waitForFirstToReady);
        }


        /// <summary>Creates a new ExponentialMovingAverage indicator with the specified period and smoothingFactor from the left indicator
        /// </summary>
        /// <param name="left">The ExponentialMovingAverage indicator will be created using the data from left</param>
        /// <param name="period">The period of the ExponentialMovingAverage indicators</param>
        /// <param name="smoothingFactor">The percentage of data from the previous value to be carried into the next value</param>
        /// <param name="waitForFirstToReady">True to only send updates to the second if left.IsReady returns true, false to alway send updates</param>
        /// <returns>A reference to the ExponentialMovingAverage indicator to allow for method chaining</returns>
        public static ExponentialMovingAverage EMA(this IUpdatable left, int period, double? smoothingFactor = null, bool waitForFirstToReady = true) {
            double k = smoothingFactor.HasValue ? k = smoothingFactor.Value : ExponentialMovingAverage.SmoothingFactorDefault(period);
            return new ExponentialMovingAverage($"EMA{period}_Of_{left.ExtractName()}", period, k).Of(left, waitForFirstToReady);
        }

        /// <summary>Creates a new Maximum indicator with the specified period from the left indicator
        /// </summary>
        /// <param name="left">The Maximum indicator will be created using the data from left</param>
        /// <param name="period">The period of the Maximum indicator</param>
        /// <param name="waitForFirstToReady">True to only send updates to the second if left.IsReady returns true, false to alway send updates</param>
        /// <returns>A reference to the Maximum indicator to allow for method chaining</returns>
        public static IIndicator MAX(this IUpdatable left, int period, bool waitForFirstToReady = true) {
            return new Maximum($"MAX{period}_Of_{left.ExtractName()}", period).Of(left, waitForFirstToReady);
        }

        /// <summary>Creates a new Minimum indicator with the specified period from the left indicator
        /// </summary>
        /// <param name="left">The Minimum indicator will be created using the data from left</param>
        /// <param name="period">The period of the Minimum indicator</param>
        /// <param name="waitForFirstToReady">True to only send updates to the second if left.IsReady returns true, false to alway send updates</param>
        /// <returns>A reference to the Minimum indicator to allow for method chaining</returns>
        public static IIndicator MIN(this IUpdatable left, int period, bool waitForFirstToReady = true) {
            return (period <= 0 ? (IIndicator) new PeriodlessMinimum($"MIN{period}_Of_{left.ExtractName()}") : new Minimum($"MIN{period}_Of_{left.ExtractName()}", period)).Of(left, waitForFirstToReady);
        }

        /// <summary>Initializes a new instance of the SimpleMovingAverage class with the specified name and period from the left indicator
        /// </summary>
        /// <param name="left">The SimpleMovingAverage indicator will be created using the data from left</param>
        /// <param name="period">The period of the SMA</param>
        /// <param name="waitForFirstToReady">True to only send updates to the second if first.IsReady returns true, false to alway send updates to second</param>
        /// <returns>The reference to the SimpleMovingAverage indicator to allow for method chaining</returns>
        public static SimpleMovingAverage SMA(this IUpdatable left, int period, bool waitForFirstToReady = true) {
            return new SimpleMovingAverage($"SMA{period}_Of_{left.ExtractName()}", period).Of(left, waitForFirstToReady);
        }

        /// <summary>
        /// 	 An indicator that delays its input for a certain period
        /// </summary>
        /// <param name="input">The indicator that sends data via <see cref="IUpdatable.Updated"/> even to the second</param>
        /// <param name="waitForFirstToReady">True to only send updates to the second if input.IsReady returns true, false to alway send updates to second</param>
        /// <param name="period">The period to delay input, must be greater than zero</param>
        /// <param name="name">Name of the returned <see cref="FinanceSharp.Indicators.Delay"/>.</param>
        /// <returns>The reference to the second indicator to allow for method chaining</returns>
        public static Delay Delay(this IUpdatable input, int period, bool waitForFirstToReady = true, string name = null) {
            return new Delay(name ?? $"DELAY({period})", period).Of(input, waitForFirstToReady);
        }

        /// <summary>
        /// 	 An indicator that creates a <see cref="RollingWindow{T}"/> with <paramref name="period"/> for every item it receives via <see cref="IUpdatable.Update"/>.
        /// </summary>
        /// <param name="input">The indicator that sends data via <see cref="IUpdatable.Updated"/> even to the WindowIdentity</param>
        /// <param name="waitForFirstToReady">True to only send updates to the second if input.IsReady returns true, false to alway send updates to second</param>
        /// <param name="period">The period to delay input, must be greater than zero</param>
        /// <param name="name">Name of the returned <see cref="FinanceSharp.Indicators.Delay"/>.</param>
        /// <returns>The reference to the second indicator to allow for method chaining</returns>
        public static WindowIdentity Window(this IUpdatable input, int period, bool waitForFirstToReady = true, string name = null) {
            return new WindowIdentity(name ?? $"DELAY({period})", period).Of(input, waitForFirstToReady);
        }

        /// <summary>
        /// 	 An indicator that creates a <see cref="ArrayRollingWindow"/> with <paramref name="period"/> for every item it receives via <see cref="IUpdatable.Update"/>.
        /// </summary>
        /// <param name="input">The indicator that sends data via <see cref="IUpdatable.Updated"/> even to the ArrayRollingWindow</param>
        /// <param name="waitForFirstToReady">True to only send updates to the second if input.IsReady returns true, false to alway send updates to second</param>
        /// <param name="period">The period to delay input, must be greater than zero</param>
        /// <param name="name">Name of the returned <see cref="FinanceSharp.Indicators.Delay"/>.</param>
        /// <returns>The reference to the second indicator to allow for method chaining</returns>
        public static ArrayRollingWindow ArrayWindow(this IUpdatable input, int period, bool waitForFirstToReady = true, string name = null) {
            return new ArrayRollingWindow(period, input.Properties, name ?? $"DELAY({period})").Of(input, waitForFirstToReady);
        }

        /// <summary>
        ///     Resolves the name of given <paramref name="updatable"/>
        /// </summary>
        public static string ExtractName(this IUpdatable updatable) {
            if (updatable is IIndicator ind) {
                return ind.Name;
            }

            return updatable.GetType().Name;
        }

        private class UpdateCounter {
            public int Interval;
            public int Updates;
        }

        /// <summary>
        ///     Makes the give <paramref name="updatable"/> to trigger <see cref="Debugger.Break"/> when <see cref="IUpdatable.Updated"/> is fired.
        /// </summary>
        /// <param name="interval">On every n updates to break. by default 1.</param>
        public static T BreakOnUpdate<T>(this T updatable, int interval = 1) where T : IUpdatable {
            if (interval < 0) throw new ArgumentOutOfRangeException(nameof(interval));
            if (interval > 1) {
                var cnt = new UpdateCounter {Interval = interval, Updates = 0};
                updatable.Updated += (time, updated) => {
                    if (++cnt.Updates < cnt.Interval)
                        return;
                    _breakOnUpdate(updatable, time, updated);
                    cnt.Updates = 0;
                };

                return updatable;
            }

            updatable.Updated += (time, updated) => _breakOnUpdate(updatable, time, updated);
            return updatable;
        }

        [DebuggerStepThrough]
        private static void _breakOnUpdate(IUpdatable updatable, long time, DoubleArray updated) {
            if (Debugger.IsAttached)
                Debugger.Break();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool AreDoubleScalar(IUpdatable left, IUpdatable right) {
            return left.Properties == 1 && right.Properties == 1 && left.OutputCount == 1 && right.OutputCount == 1;
        }
    }
}