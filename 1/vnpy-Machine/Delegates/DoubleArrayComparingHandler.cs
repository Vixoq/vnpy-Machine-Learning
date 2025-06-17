namespace FinanceSharp.Delegates {
    /// <summary>
    ///     Compares <paramref name="current"/> against <paramref name="input"/> returning the new <see cref="IUpdatable.Current"/> and <see cref="IUpdatable.CurrentTime"/> values.
    /// </summary>
    /// <param name="current">The <see cref="IUpdatable.Current"/> from the handling indicator.</param>
    /// <param name="input">The new input to compare against.</param>
    /// <returns>The new <see cref="IUpdatable.Current"/> and <see cref="IUpdatable.CurrentTime"/> to set.</returns>

    public delegate (long time, DoubleArray Array) DoubleArrayComparingHandler((long time, DoubleArray Array) current, (long Time, DoubleArray Array) input);
}