using System;

namespace FinanceSharp.Trading {
    [Serializable]
    public struct Trade : IEquatable<Trade> {
        public double From;
        public double To;
        public TradeState TradeState;
        public double Profit;
        public string Extra;
        public int Quantity;
        public long EntryTime;
        public long ExitTime;


        public void AppendProfit(double profit) {
            Profit += profit;
        }

        /// <inheritdoc />
        public override string ToString() {
            return $"{nameof(From)}: {From}, {nameof(To)}: {To}, {nameof(TradeState)}: {TradeState}, {nameof(Profit)}: {Profit}, {nameof(Extra)}: {Extra}, {nameof(Quantity)}: {Quantity}";
        }

        public bool Equals(Trade other) {
            return From.Equals(other.From) && To.Equals(other.To) && TradeState == other.TradeState && Profit.Equals(other.Profit) && string.Equals(Extra, other.Extra) && Quantity == other.Quantity && EntryTime.Equals(other.EntryTime) && ExitTime.Equals(other.ExitTime);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Trade other && Equals(other);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = From.GetHashCode();
                hashCode = (hashCode * 397) ^ To.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) TradeState;
                hashCode = (hashCode * 397) ^ Profit.GetHashCode();
                hashCode = (hashCode * 397) ^ Quantity;
                hashCode = (hashCode * 397) ^ EntryTime.GetHashCode();
                hashCode = (hashCode * 397) ^ ExitTime.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Trade left, Trade right) {
            return left.Equals(right);
        }

        public static bool operator !=(Trade left, Trade right) {
            return !left.Equals(right);
        }
    }
}