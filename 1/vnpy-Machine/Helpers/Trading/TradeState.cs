using System;

namespace FinanceSharp.Trading {
    [Serializable]
    public enum TradeState {
        NewDay,
        Idling,
        Longing,
        Shorting,
        /// Not necessary liquidate, can also be take profit or stop loss.
        Liquidate,
        EndOfDay,
        ForcedIdling
    }
}