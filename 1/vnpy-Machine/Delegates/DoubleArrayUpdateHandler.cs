namespace FinanceSharp.Delegates {
    /// <summary>
    /// 	 Handler for a function that updates the state of this updatable with the given value and returns true
    /// 	 if this updatable is ready, false otherwise
    /// </summary>
    /// <param name="time">The timestamp represented in milliseconds-epoch-1970.</param>
    /// <param name="input">The value to use to update this updatable</param>
    /// <returns>True if this updatable is ready, false otherwise</returns>
    public delegate void DoubleArrayUpdateHandler(long time, DoubleArray input);
}