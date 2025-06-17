using System;
using System.Runtime.CompilerServices;
using FinanceSharp.Trading;

namespace FinanceSharp.Helpers {
    public static class PriceCalculator {
        /// <param name="style">The trade style</param>
        /// <param name="price">Price of the stock</param>
        /// <param name="quantity">Absolute quantity</param>
        /// <param name="profitPrecentage">Profit in precentages, can be negative if loss planned. examples: 0.05f is 0.05% profit.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double LimitAtProfit(BuyStyle style, double price, int quantity, float profitPrecentage) {
            if (quantity == 0)
                throw new ArgumentOutOfRangeException(nameof(quantity));
            return Math.Round(price * (1 + (style == BuyStyle.Long ? 1 : -1) * ((double) profitPrecentage / 100d)), 2);
        }

        /// <param name="style">The trade style</param>
        /// <param name="price">Price of the stock</param>
        /// <param name="quantity">Absolute quantity</param>
        /// <param name="profitDollar">Profit in dollars, can be negative if loss planned</param>
        /// <returns></returns>
        public static double LimitAtProfit(BuyStyle style, double price, int quantity, double profitDollar) {
            if (quantity == 0)
                throw new ArgumentOutOfRangeException(nameof(quantity));
            quantity = Math.Abs(quantity);

            //161*(1+0.05/100)
            var mod = style == BuyStyle.Long ? 1 : -1;
            var balance = price * quantity;
            var targetbalance = balance + mod * profitDollar;
            var targetPrice = targetbalance / quantity;
            return Math.Round(targetPrice, 2);
        }

        /// <param name="style">The trade style</param>
        /// <param name="price">Price of the stock</param>
        /// <param name="quantity">Absolute quantity</param>
        /// <param name="profitDollar">Profit in dollars, can be negative if loss planned</param>
        /// <returns></returns>
        public static double LimitAtProfit(BuyStyle style, TickValue tick, int quantity, double profitDollar) {
            if (quantity == 0)
                throw new ArgumentOutOfRangeException(nameof(quantity));
            quantity = Math.Abs(quantity);

            //161*(1+0.05/100)
            var mod = style == BuyStyle.Long ? 1 : -1;
            var balance = (style == BuyStyle.Long ? tick.AskPrice : tick.BidPrice) * quantity;
            var targetbalance = balance + mod * profitDollar;
            var targetPrice = targetbalance / quantity;
            return Math.Round(targetPrice, 2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double Profit, int Quantity) CalculateProfit(BuyStyle style, TickValue from, TickValue to, double budget) {
            if (style == BuyStyle.Long) {
                return CalculateProfit(BuyStyle.Long, from.AskPrice, to.BidPrice, budget);
            } else {
                //short
                return CalculateProfit(BuyStyle.Short, from.BidPrice, to.AskPrice, budget);
            }
        }

        /// <param name="quantity">Non absolute quantity of the trade</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double Profit, int Quantity) CalculateProfit(BuyStyle style, TickValue from, TickValue to, int quantity) {
            if (style == BuyStyle.Long) {
                return CalculateProfit(BuyStyle.Long, from.AskPrice, to.BidPrice, quantity);
            } else {
                //short
                return CalculateProfit(BuyStyle.Short, from.BidPrice, to.AskPrice, quantity);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double Profit, int Quantity) CalculateProfit(BuyStyle style, TickValue from, double to, double budget) {
            if (style == BuyStyle.Long) {
                return CalculateProfit(BuyStyle.Long, from.AskPrice, to, budget);
            } else {
                //short
                return CalculateProfit(BuyStyle.Short, from.BidPrice, to, budget);
            }
        }

        /// <param name="quantity">Non absolute quantity of the trade</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double Profit, int Quantity) CalculateProfit(BuyStyle style, TickValue from, double to, int quantity) {
            if (style == BuyStyle.Long) {
                return CalculateProfit(BuyStyle.Long, from.AskPrice, to, quantity);
            } else {
                //short
                return CalculateProfit(BuyStyle.Short, from.BidPrice, to, quantity);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double Profit, int Quantity) CalculateProfit(BuyStyle style, double from, TickValue to, double budget) {
            if (style == BuyStyle.Long) {
                return CalculateProfit(BuyStyle.Long, from, to.BidPrice, budget);
            } else {
                //short
                return CalculateProfit(BuyStyle.Short, from, to.AskPrice, budget);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (float Profit, int Quantity) CalculateProfit(BuyStyle style, TickValue from, float to, float budget) {
            if (style == BuyStyle.Long) {
                return CalculateProfit(BuyStyle.Long, (float) from.AskPrice, to, budget);
            } else {
                //short
                return CalculateProfit(BuyStyle.Short, (float) from.BidPrice, to, budget);
            }
        }

        /// <param name="quantity">Non absolute quantity of the trade</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (float Profit, int Quantity) CalculateProfit(BuyStyle style, TickValue from, float to, int quantity) {
            if (style == BuyStyle.Long) {
                return CalculateProfit(BuyStyle.Long, (float) from.AskPrice, to, quantity);
            } else {
                //short
                return CalculateProfit(BuyStyle.Short, (float) from.BidPrice, to, quantity);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (float Profit, int Quantity) CalculateProfit(BuyStyle style, float from, TickValue to, float budget) {
            if (style == BuyStyle.Long) {
                return CalculateProfit(BuyStyle.Long, from, (float) to.BidPrice, budget);
            } else {
                //short
                return CalculateProfit(BuyStyle.Short, from, (float) to.AskPrice, budget);
            }
        }

        /// <param name="quantity">Non absolute quantity of the trade</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double Profit, int Quantity) CalculateProfit(BuyStyle style, double from, TickValue to, int quantity) {
            if (style == BuyStyle.Long) {
                return CalculateProfit(BuyStyle.Long, from, to.BidPrice, quantity);
            } else {
                //short
                return CalculateProfit(BuyStyle.Short, from, to.AskPrice, quantity);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double Profit, int Quantity) CalculateProfit(BuyStyle style, double from, double to, double budget) {
            return CalculateProfit(style, from, to, CalculateQuantity(style, from, budget));
        }

        /// <param name="quantity">Non absolute quantity of the trade</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double Profit, int Quantity) CalculateProfit(BuyStyle style, double from, double to, int quantity) {
            return (quantity * to - quantity * @from, quantity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (float Profit, int Quantity) CalculateProfit(BuyStyle style, float from, float to, double budget) {
            return CalculateProfit(style, from, to, CalculateQuantity(style, from, budget));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (float Profit, int Quantity) CalculateProfit(BuyStyle style, float from, float to, float budget) {
            return CalculateProfit(style, from, to, CalculateQuantity(style, from, budget));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (float Profit, int Quantity) CalculateProfit(BuyStyle style, float from, float to, int quantity) {
            return (quantity * to - quantity * @from, quantity);
        }

        #region Trade

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trade CalculateTrade(BuyStyle style, long fromTime, TickValue from, long toTime, TickValue to, double budget) {
            if (style == BuyStyle.Long) {
                return CalculateTrade(BuyStyle.Long, from.AskPrice, to.BidPrice, fromTime, toTime, budget);
            } else {
                //short
                return CalculateTrade(BuyStyle.Short, from.BidPrice, to.AskPrice, fromTime, toTime, budget);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trade CalculateTrade(BuyStyle style, double from, double to, long timefrom, long timeto, double budget) {
            return CalculateTrade(style, from, to, timefrom, timeto, CalculateQuantity(style, from, budget));
        }

        /// <param name="quantity">Non absolute quantity of the trade</param>
        public static Trade CalculateTrade(BuyStyle style, double from, double to, long timefrom, long timeto, int quantity) {
            if (quantity == 0) {
                return new Trade() {
                    Profit = 0,
                    Quantity = 0,
                    From = Convert.ToSingle(from),
                    To = Convert.ToSingle(to),
                    EntryTime = timefrom,
                    ExitTime = timeto,
                    TradeState = style == BuyStyle.Long ? TradeState.Longing : TradeState.Shorting
                };
            }

            return new Trade() {
                Profit = Convert.ToSingle(quantity * to - quantity * @from),
                From = Convert.ToSingle(from),
                To = Convert.ToSingle(to),
                EntryTime = timefrom,
                ExitTime = timeto,
                Quantity = quantity,
                TradeState = style == BuyStyle.Long ? TradeState.Longing : TradeState.Shorting
            };
        }

        #endregion


        #region Quantity

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CalculateQuantity(BuyStyle style, double from, double budget) {
            return (style == BuyStyle.Long ? 1 : -1) * (from == 0 ? 0 : Convert.ToInt32(Math.Floor(budget / from)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CalculateQuantity(BuyStyle style, float from, float budget) {
            return (style == BuyStyle.Long ? 1 : -1) * (Math.Abs(@from) < 0.00001f ? 0 : Convert.ToInt32(Math.Floor(budget / from)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CalculateQuantity(BuyStyle style, float from, double budget) {
            return (style == BuyStyle.Long ? 1 : -1) * (Math.Abs(@from) < 0.00001f ? 0 : Convert.ToInt32(Math.Floor(budget / (double) @from)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CalculateQuantity(BuyStyle style, TickValue from, double budget) {
            if (style == BuyStyle.Long) {
                return CalculateQuantity(BuyStyle.Long, from.AskPrice, budget);
            } else {
                //short
                return CalculateQuantity(BuyStyle.Short, from.BidPrice, budget);
            }
        }

        #endregion

        #region TickValue

        /// <summary>
        ///     Extracts the right price (bid or ask) for entering the market based on given purchase <see cref="style"/>.
        /// </summary>
        /// <param name="tick">The tick to extract price from</param>
        /// <param name="style">The style the trade will be done in.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double EntryPrice(BuyStyle style, TickValue tick) {
            if (style == BuyStyle.Long) {
                return tick.AskPrice;
            } else {
                return tick.BidPrice;
            }
        }

        /// <summary>
        ///     Extracts the right price (bid or ask) for entering the market based on given purchase <see cref="style"/>.
        /// </summary>
        /// <param name="tick">The tick to extract price from</param>
        /// <param name="style">The style the trade will be done in.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ExitPrice(BuyStyle style, TickValue tick) {
            if (style == BuyStyle.Long) {
                return tick.BidPrice;
            } else {
                return tick.AskPrice;
            }
        }


        /// <summary>
        ///     Extracts the right price (bid or ask) for entering the market based on given purchase <see cref="style"/>.
        /// </summary>
        /// <param name="tick">The tick to extract price from</param>
        /// <param name="style">The style the trade will be done in.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double EntryPrice(this TickValue tick, BuyStyle style) {
            if (style == BuyStyle.Long) {
                return tick.AskPrice;
            } else {
                return tick.BidPrice;
            }
        }

        /// <summary>
        ///     Extracts the right price (bid or ask) for entering the market based on given purchase <see cref="style"/>.
        /// </summary>
        /// <param name="tick">The tick to extract price from</param>
        /// <param name="style">The style the trade will be done in.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ExitPrice(this TickValue tick, BuyStyle style) {
            if (style == BuyStyle.Long) {
                return tick.BidPrice;
            } else {
                return tick.AskPrice;
            }
        }

        #endregion
    }
}