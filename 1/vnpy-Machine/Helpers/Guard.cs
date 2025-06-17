using System;
using System.Diagnostics;

namespace FinanceSharp.Helpers {
    internal static class Guard {
        /// <summary>
        ///     Asserts for DEBUG runs..
        /// </summary>
        [Conditional("DEBUG"), DebuggerHidden]
        public static void AssertTrue(bool condition) {
            if (!condition)
                throw new Exception();
        }

        /// <summary>
        ///     Asserts for DEBUG runs..
        /// </summary>
        [Conditional("DEBUG"), DebuggerHidden]
        public static void AssertTrue(bool condition, string reason) {
            if (!condition)
                throw new Exception(reason);
        }
    }
}