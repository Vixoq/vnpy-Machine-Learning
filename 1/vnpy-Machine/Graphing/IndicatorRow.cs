using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using FinanceSharp.Delegates;

namespace FinanceSharp.Graphing {
    /// <summary>
    ///     Provides a wrapper for <see cref="Concat"/> to create a row of indicators initialized by an index (argument in <see cref="IndicatorFactoryHandler"/>).
    /// </summary>
    /// <remarks>A way to use the  index (argument in <see cref="IndicatorFactoryHandler"/>) to create an indicator is to use it as period setting. Use dictionaries or arrays to gain access to more complex info to initialize the array. Get creative.</remarks>
    public static class IndicatorRow {
        private static Concat BindToConcatenatingList(Concat concat) {
            var bindTo = concat.Concatenating;
            var len = bindTo.Length;
            concat.UpdateHandler = ConcatUpdateHandler;

            void ConcatUpdateHandler(long time, DoubleArray input) {
                for (int i = 0; i < len; i++) 
                    bindTo[i].Update(time, input);
            }

            return concat;
        }

        /// <summary>
        ///     Generates indicators for every <paramref name="indices"/> by calling <paramref name="indicatorFactory"/>.
        /// </summary>
        /// <param name="indices">The index to use to call <paramref name="indicatorFactory"/>.</param>
        /// <param name="indicatorFactory">Factory that uses the passed index to initialize an Indicator.</param>
        /// <returns>All indicators created.</returns>
        public static IUpdatable[] Factory(IEnumerable<int> indices, IndicatorFactoryHandler indicatorFactory) {
            return indices.Select(i => indicatorFactory(i)).ToArray();
        }

        /// <summary>
        ///     Generates <paramref name="size"/> indicators by calling <paramref name="indicatorFactory"/>.
        /// </summary>
        /// <param name="size">How many indicators to initialize.</param>
        /// <param name="indicatorFactory">Factory that uses the passed index to initialize an Indicator.</param>
        /// <returns>All indicators created.</returns>
        public static IUpdatable[] Factory(int size, IndicatorFactoryHandler indicatorFactory) {
            return Enumerable.Range(0, size).Select(i => indicatorFactory(i)).ToArray();
        }

        /// <summary>
        ///     Concatenates when all <see cref="IUpdatable"/> were updated atleast once.
        /// </summary>
        /// <param name="name">Name of the cruncher for debugging purposes.</param>
        /// <param name="properties">
        ///     How many properties all of the <paramref name="updatables"/> emit.
        ///     this can be less than their minimal properties.
        ///     e.g. if <paramref name="updatables"/> emit <see cref="BarValue"/> (4 properties), selecting 1 will take only <see cref="BarValue.Close"/>.
        /// </param>
        /// <returns>A new cruncher configured.</returns>
        public static Concat OnAllUpdatedOnce(int size, IndicatorFactoryHandler factory, int properties = 1, string name = null) {
            return BindToConcatenatingList(Concat.OnAllUpdatedOnce(Factory(size, factory), properties, name));
        }

        /// <summary>
        ///     Concatenates when any of <see cref="IUpdatable"/> are updated for every n <paramref name="interval"/>.
        /// </summary>
        /// <param name="name">Name of the cruncher for debugging purposes.</param>
        /// <param name="interval">The interval for how many fires must any of <paramref name="updatables"/> trigger <see cref="IUpdatable.Updated"/> in order to trigger IndicatorRow's update event.</param>
        /// <param name="properties">
        ///     How many properties all of the <paramref name="updatables"/> emit.
        ///     this can be less than their minimal properties.
        ///     e.g. if <paramref name="updatables"/> emit <see cref="BarValue"/> (4 properties), selecting 1 will take only <see cref="BarValue.Close"/>.
        /// </param>
        /// <param name="size">How many indicators will be initialized via <paramref name="factory"/>.</param>
        /// <param name="factory">A factory to initialize new indicators.</param>
        /// <returns>A new cruncher configured.</returns>
        public static Concat OnEveryUpdate(int size, IndicatorFactoryHandler factory, int interval = 1, int properties = 1, string name = null) {
            return BindToConcatenatingList(Concat.OnEveryUpdate(Factory(size, factory), interval, properties, name));
        }

        /// <summary>
        ///     Concatenates <paramref name="updatables"/> whenever <paramref name="crunchTrigger"/> is updated for n <paramref name="interval"/> times.
        /// </summary>
        /// <param name="name">Name of the cruncher for debugging purposes.</param>
        /// <param name="crunchTrigger">The <see cref="IUpdatable"/> to observe for fires of <see cref="IUpdatable.Updated"/>.</param>
        /// <param name="interval">The interval for how many fires must <paramref name="crunchTrigger"/> trigger <see cref="IUpdatable.Updated"/> in order to trigger IndicatorRow's update event.</param>
        /// <param name="properties">
        ///     How many properties all of the <paramref name="updatables"/> emit.
        ///     this can be less than their minimal properties.
        ///     e.g. if <paramref name="updatables"/> emit <see cref="BarValue"/> (4 properties), selecting 1 will take only <see cref="BarValue.Close"/>.
        /// </param>
        /// <param name="size">How many indicators will be initialized via <paramref name="factory"/>.</param>
        /// <param name="factory">A factory to initialize new indicators.</param>
        /// <param name="triggerMustBeReady">Does <paramref name="crunchTrigger"/> must be ready to trigger IndicatorRow's update event?</param>
        /// <returns>A new cruncher configured.</returns>
        public static Concat OnSpecificUpdate(int size, IndicatorFactoryHandler factory, IUpdatable crunchTrigger, int interval = 1, int properties = 1, string name = null, bool triggerMustBeReady = true) {
            return BindToConcatenatingList(Concat.OnSpecificUpdate(Factory(size, factory), crunchTrigger, interval, properties, name, triggerMustBeReady));
        }

        /// <summary>
        ///     Concatenates <paramref name="updatables"/> whenever <paramref name="crunchTriggers"/> is updated for n <paramref name="interval"/> times.
        /// </summary>
        /// <param name="name">Name of the cruncher for debugging purposes.</param>
        /// <param name="crunchTriggers">The <see cref="IUpdatable"/>s to observe for fires of <see cref="IUpdatable.Updated"/>.</param>
        /// <param name="interval">The interval for how many fires must <paramref name="crunchTriggers"/> trigger <see cref="IUpdatable.Updated"/> in order to trigger IndicatorRow's update event.</param>
        /// <param name="properties">
        ///     How many properties all of the <paramref name="updatables"/> emit.
        ///     this can be less than their minimal properties.
        ///     e.g. if <paramref name="updatables"/> emit <see cref="BarValue"/> (4 properties), selecting 1 will take only <see cref="BarValue.Close"/>.
        /// </param>
        /// <param name="size">How many indicators will be initialized via <paramref name="factory"/>.</param>
        /// <param name="factory">A factory to initialize new indicators.</param>
        /// <param name="triggerMustBeReady">Does <paramref name="crunchTriggers"/> must be ready to trigger IndicatorRow's update event? By default </param>
        /// <returns>A new cruncher configured.</returns>
        public static Concat OnSpecificUpdate(int size, IndicatorFactoryHandler factory, IUpdatable[] crunchTriggers, int interval = 1, int properties = 1, string name = null, bool[] triggerMustBeReady = null) {
            return BindToConcatenatingList(Concat.OnSpecificUpdate(Factory(size, factory), crunchTriggers, interval, properties, name, triggerMustBeReady));
        }

        /// <summary>
        ///     Concatenates when all <see cref="IUpdatable"/> were updated atleast once.
        /// </summary>
        /// <param name="name">Name of the cruncher for debugging purposes.</param>
        /// <param name="properties">
        ///     How many properties all of the <paramref name="updatables"/> emit.
        ///     this can be less than their minimal properties.
        ///     e.g. if <paramref name="updatables"/> emit <see cref="BarValue"/> (4 properties), selecting 1 will take only <see cref="BarValue.Close"/>.
        /// </param>
        /// <param name="factory">A factory to initialize new indicators using <paramref name="indices"/>.</param>
        /// <param name="indices">The index to use to call <paramref name="factory"/>.</param>
        /// <returns>A new cruncher configured.</returns>
        public static Concat OnAllUpdatedOnce(IEnumerable<int> indices, IndicatorFactoryHandler factory, int properties = 1, string name = null) {
            return BindToConcatenatingList(Concat.OnAllUpdatedOnce(Factory(indices, factory), properties, name));
        }

        /// <summary>
        ///     Concatenates when any of <see cref="IUpdatable"/> are updated for every n <paramref name="interval"/>.
        /// </summary>
        /// <param name="name">Name of the cruncher for debugging purposes.</param>
        /// <param name="interval">The interval for how many fires must any of <paramref name="updatables"/> trigger <see cref="IUpdatable.Updated"/> in order to trigger IndicatorRow's update event.</param>
        /// <param name="properties">
        ///     How many properties all of the <paramref name="updatables"/> emit.
        ///     this can be less than their minimal properties.
        ///     e.g. if <paramref name="updatables"/> emit <see cref="BarValue"/> (4 properties), selecting 1 will take only <see cref="BarValue.Close"/>.
        /// </param>
        /// <param name="factory">A factory to initialize new indicators using <paramref name="indices"/>.</param>
        /// <param name="indices">The index to use to call <paramref name="factory"/>.</param>
        /// <returns>A new cruncher configured.</returns>
        public static Concat OnEveryUpdate(IEnumerable<int> indices, IndicatorFactoryHandler factory, int interval = 1, int properties = 1, string name = null) {
            return BindToConcatenatingList(Concat.OnEveryUpdate(Factory(indices, factory), interval, properties, name));
        }

        /// <summary>
        ///     Concatenates <paramref name="updatables"/> whenever <paramref name="crunchTrigger"/> is updated for n <paramref name="interval"/> times.
        /// </summary>
        /// <param name="name">Name of the cruncher for debugging purposes.</param>
        /// <param name="crunchTrigger">The <see cref="IUpdatable"/> to observe for fires of <see cref="IUpdatable.Updated"/>.</param>
        /// <param name="interval">The interval for how many fires must <paramref name="crunchTrigger"/> trigger <see cref="IUpdatable.Updated"/> in order to trigger IndicatorRow's update event.</param>
        /// <param name="properties">
        ///     How many properties all of the <paramref name="updatables"/> emit.
        ///     this can be less than their minimal properties.
        ///     e.g. if <paramref name="updatables"/> emit <see cref="BarValue"/> (4 properties), selecting 1 will take only <see cref="BarValue.Close"/>.
        /// </param>
        /// <param name="indices">The index to use to call <paramref name="factory"/>.</param>
        /// <param name="factory">A factory to initialize new indicators using <paramref name="indices"/>.</param>
        /// <param name="triggerMustBeReady">Does <paramref name="crunchTrigger"/> must be ready to trigger IndicatorRow's update event?</param>
        /// <returns>A new cruncher configured.</returns>
        public static Concat OnSpecificUpdate(IEnumerable<int> indices, IndicatorFactoryHandler factory, IUpdatable crunchTrigger, int interval = 1, int properties = 1, string name = null, bool triggerMustBeReady = true) {
            return BindToConcatenatingList(Concat.OnSpecificUpdate(Factory(indices, factory), crunchTrigger, interval, properties, name, triggerMustBeReady));
        }

        /// <summary>
        ///     Concatenates <paramref name="updatables"/> whenever <paramref name="crunchTriggers"/> is updated for n <paramref name="interval"/> times.
        /// </summary>
        /// <param name="name">Name of the cruncher for debugging purposes.</param>
        /// <param name="crunchTriggers">The <see cref="IUpdatable"/>s to observe for fires of <see cref="IUpdatable.Updated"/>.</param>
        /// <param name="interval">The interval for how many fires must <paramref name="crunchTriggers"/> trigger <see cref="IUpdatable.Updated"/> in order to trigger IndicatorRow's update event.</param>
        /// <param name="properties">
        ///     How many properties all of the <paramref name="updatables"/> emit.
        ///     this can be less than their minimal properties.
        ///     e.g. if <paramref name="updatables"/> emit <see cref="BarValue"/> (4 properties), selecting 1 will take only <see cref="BarValue.Close"/>.
        /// </param>
        /// <param name="indices">The index to use to call <paramref name="factory"/>.</param>
        /// <param name="factory">A factory to initialize new indicators using <paramref name="indices"/>.</param>
        /// <param name="triggerMustBeReady">Does <paramref name="crunchTriggers"/> must be ready to trigger IndicatorRow's update event? By default </param>
        /// <returns>A new cruncher configured.</returns>
        public static Concat OnSpecificUpdate(IEnumerable<int> indices, IndicatorFactoryHandler factory, IUpdatable[] crunchTriggers, int interval = 1, int properties = 1, string name = null, bool[] triggerMustBeReady = null) {
            return BindToConcatenatingList(Concat.OnSpecificUpdate(Factory(indices, factory), crunchTriggers, interval, properties, name, triggerMustBeReady));
        }
    }
}