/*
 * All Rights reserved to Ebby Technologies LTD @ Eli Belash, 2020.
 * Original code by QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace FinanceSharp {
    /// <summary>
    ///     Provides staticly cached variables about <typeparamref name="TStruct"/>.
    /// </summary>
    /// <typeparam name="TStruct">The type to provide info about.</typeparam>
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    public static class StructInfo<TStruct> where TStruct : struct {
        /// <summary>
        ///     Number of public fields of type <see cref="double"/> in given <typeparamref name="TStruct"/>.
        /// </summary>
        public static readonly int DoubleFieldsCount;
        /// <summary>
        ///     Number of fields of type <see cref="double"/> in given <typeparamref name="TStruct"/>.
        /// </summary>
        public static readonly int FieldsCount;

        static StructInfo() {
            var observedFields = BindingFlags.Public | BindingFlags.Instance;
            DoubleFieldsCount = typeof(TStruct).GetFields(observedFields).Count(f => f.FieldType == typeof(double));
            FieldsCount = typeof(TStruct).GetFields(observedFields).Length;
        }
    }

    /// <summary>
    ///     Provides staticly cached variables about <typeparamref name="TDataStruct"/>.
    /// </summary>
    /// <typeparam name="TDataStruct">The type to provide info about.</typeparam>
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    public static class DataStructInfo<TDataStruct> where TDataStruct : struct, DataStruct {
        public static int DoubleFieldsCount = StructInfo<TDataStruct>.DoubleFieldsCount;
        public static int FieldsCount = StructInfo<TDataStruct>.FieldsCount;
        /// <summary>
        ///     Number of Properties in type <typeparamref name="TDataStruct"/>.
        ///     The value is cached staticly.
        /// </summary>
        public static readonly int Properties;

        static DataStructInfo() {
            Properties = new TDataStruct().Properties;
        }
    }
}