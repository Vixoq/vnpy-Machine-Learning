namespace FinanceSharp.Delegates {
    /// <summary>
    ///     A mathematical function that receives a two values and returns one.
    /// </summary>
    /// <param name="lhs">A value.</param>
    /// <param name="rhs">A value.</param>
    /// <returns>The result of the function.</returns>
    public delegate double BinaryFunctionHandler(double lhs, double rhs);
}