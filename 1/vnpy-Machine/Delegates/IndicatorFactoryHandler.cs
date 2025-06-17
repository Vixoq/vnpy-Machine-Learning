namespace FinanceSharp.Delegates {
    /// <summary>
    ///     Used to initialize a new indicator based on given <paramref name="index"/>.
    /// </summary>
    /// <param name="index">The index in the factory iteration.</param>
    public delegate IUpdatable IndicatorFactoryHandler(int index);

    /// <summary>
    ///     Used to initialize a new indicator based on given <paramref name="index"/> and must bind it to <paramref name="input"/>.
    /// </summary>
    /// <param name="index">The index in the factory iteration.</param>
    public delegate IUpdatable BindingIndicatorFactoryHandler(IUpdatable input, int index);
}