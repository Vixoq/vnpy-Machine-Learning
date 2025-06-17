using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using FinanceSharp.Delegates;
using FinanceSharp.Indicators;

namespace FinanceSharp.Graphing {
    public class kek {
        public void kekek() {
            ExponentialMovingAverage Selector(int i) => new ExponentialMovingAverage($"EMA{i + 2}", i + 2);
            var concat = Concat.OnAllUpdatedOnce(Enumerable.Range(0, 3).Select(Selector));
            Concat.ForEvery.OnAllUpdatedOnce(concat, Selector);
        }
    }

    /// <summary>
    ///     Provides a wrapper for <see cref="Concat"/> to create a row of indicators initialized by an index (argument in <see cref="IndicatorFactoryHandler"/>).
    /// </summary>
    /// <remarks>A way to use the  index (argument in <see cref="IndicatorFactoryHandler"/>) to create an indicator is to use it as period setting. Use dictionaries or arrays to gain access to more complex info to initialize the array. Get creative.</remarks>
    public partial class Concat {
        /// <summary>
        ///     Provides an API for processing attaching <see cref="IUpdatable"/> that are created from a
        ///     factory (method similar to <see cref="Factory(int,FinanceSharp.Delegates.IndicatorFactoryHandler)"/>) and are fed with every item in the <see cref="DoubleArray"/> passed from <see cref="IUpdatable.Update"/>.
        /// </summary>
        public static class ForEvery {
            private static Concat BindToConcatenatingList(IUpdatable source, Concat concat) {
                //var bindTo = concat.Concatenating;
                //var len = bindTo.Length;
                //concat.UpdateHandler = ConcatUpdateHandler;

                //bool ConcatUpdateHandler(long time, DoubleArray input) {
                //    for (int i = 0; i < len; i++)
                //        bindTo[i].Update(time, input.SliceIndex(i));

                //    return concat.IsReady;
                //}

                return concat;
            }

            /// <summary>
            ///     Generates <paramref name="size"/> indicators by calling <paramref name="indicatorFactory"/>.
            /// </summary>
            /// <param name="source">The indicators that emits <see cref="IUpdatable.OutputCount"/> &gt; 1. and for every <see cref="IUpdatable.OutputCount"/> </param>
            /// <param name="indicatorFactory">Factory that uses the passed index to initialize an Indicator.</param>
            /// <returns>All indicators created.</returns>
            public static IUpdatable[] Factory(IUpdatable source, IndicatorFactoryHandler indicatorFactory, bool waitForSourceReady) {
                return Factory(source, (input, index) => indicatorFactory(index).Of(input, waitForSourceReady));
            }

            /// <summary>
            ///     Generates <paramref name="size"/> indicators by calling <paramref name="indicatorFactory"/>.
            /// </summary>
            /// <param name="source">The indicators that emits <see cref="IUpdatable.OutputCount"/> &gt; 1. and for every <see cref="IUpdatable.OutputCount"/> </param>
            /// <param name="indicatorFactory">Factory that uses the passed index to initialize an Indicator.</param>
            /// <returns>All indicators created.</returns>
            public static IUpdatable[] Factory(IUpdatable source, BindingIndicatorFactoryHandler indicatorFactory) {
                return source.Explode().Select((selector, i) => indicatorFactory(selector, i)).ToArray();
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
            public static Concat OnAllUpdatedOnce(IUpdatable source, IndicatorFactoryHandler factory, int properties = 1, string name = null, bool waitForSourceReady = true) {
                IUpdatable[] sources = Factory(source, factory, waitForSourceReady);
                return BindToConcatenatingList(source, Concat.OnAllUpdatedOnce(sources, properties, name));
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
            public static Concat OnAllUpdatedOnce(IUpdatable source, BindingIndicatorFactoryHandler factory, int properties = 1, string name = null, bool waitForSourceReady = true) {
                IUpdatable[] sources = Factory(source, factory);
                return BindToConcatenatingList(source, Concat.OnAllUpdatedOnce(sources, properties, name));
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
            public static Concat OnEveryUpdate(IUpdatable source, IndicatorFactoryHandler factory, int interval = 1, int properties = 1, string name = null, bool waitForSourceReady = true) {
                IUpdatable[] sources = Factory(source, factory, waitForSourceReady);
                return BindToConcatenatingList(source, Concat.OnEveryUpdate(sources, interval, properties, name));
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
            public static Concat OnEveryUpdate(IUpdatable source, BindingIndicatorFactoryHandler factory, int interval = 1, int properties = 1, string name = null, bool waitForSourceReady = true) {
                IUpdatable[] sources = Factory(source, factory);
                return BindToConcatenatingList(source, Concat.OnEveryUpdate(sources, interval, properties, name));
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
            public static Concat OnSpecificUpdate(IUpdatable source, IndicatorFactoryHandler factory, IUpdatable crunchTrigger, int interval = 1, int properties = 1, string name = null, bool triggerMustBeReady = true, bool waitForSourceReady = true) {
                IUpdatable[] sources = Factory(source, factory, waitForSourceReady);
                return BindToConcatenatingList(source, Concat.OnSpecificUpdate(sources, crunchTrigger, interval, properties, name, triggerMustBeReady));
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
            public static Concat OnSpecificUpdate(IUpdatable source, BindingIndicatorFactoryHandler factory, IUpdatable crunchTrigger, int interval = 1, int properties = 1, string name = null, bool triggerMustBeReady = true, bool waitForSourceReady = true) {
                IUpdatable[] sources = Factory(source, factory);
                return BindToConcatenatingList(source, Concat.OnSpecificUpdate(sources, crunchTrigger, interval, properties, name, triggerMustBeReady));
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
            public static Concat OnSpecificUpdate(IUpdatable source, IndicatorFactoryHandler factory, IUpdatable[] crunchTriggers, int interval = 1, int properties = 1, string name = null, bool[] triggerMustBeReady = null, bool waitForSourceReady = true) {
                IUpdatable[] sources = Factory(source, factory, waitForSourceReady);
                return BindToConcatenatingList(source, Concat.OnSpecificUpdate(sources, crunchTriggers, interval, properties, name, triggerMustBeReady));
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
            public static Concat OnSpecificUpdate(IUpdatable source, BindingIndicatorFactoryHandler factory, IUpdatable[] crunchTriggers, int interval = 1, int properties = 1, string name = null, bool[] triggerMustBeReady = null, bool waitForSourceReady = true) {
                IUpdatable[] sources = Factory(source, factory);
                return BindToConcatenatingList(source, Concat.OnSpecificUpdate(sources, crunchTriggers, interval, properties, name, triggerMustBeReady));
            }
        }
    }
}