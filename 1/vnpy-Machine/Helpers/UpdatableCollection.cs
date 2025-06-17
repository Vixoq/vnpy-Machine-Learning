using System;
using System.Collections.Generic;
using System.Linq;

namespace FinanceSharp.Helpers {
    /// <summary>
    ///     A <see cref="List{T}"/> that supports + operator to add items.
    /// </summary>
    public class UpdatableCollection : List<IUpdatable> {
        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.List`1"></see> class that is empty and has the default initial capacity.</summary>
        public UpdatableCollection() { }

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.List`1"></see> class that contains elements copied from the specified collection and has sufficient capacity to accommodate the number of elements copied.</summary>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="collection">collection</paramref> is null.</exception>
        public UpdatableCollection(IEnumerable<IUpdatable> collection) : base(collection) { }

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.List`1"></see> class that is empty and has the specified initial capacity.</summary>
        /// <param name="capacity">The number of elements that the new list can initially store.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="capacity">capacity</paramref> is less than 0.</exception>
        public UpdatableCollection(int capacity) : base(capacity) { }

        public static IUpdatable operator +(UpdatableCollection list, IUpdatable newItem) {
            if (newItem == null) throw new ArgumentNullException(nameof(newItem));
            list.Add(newItem);
            return newItem;
        }

        public static IEnumerable<IUpdatable> operator +(UpdatableCollection list, IEnumerable<IUpdatable> newItems) {
            if (newItems == null) throw new ArgumentNullException(nameof(newItems));
            foreach (var item in newItems) {
                if (item == null) throw new ArgumentNullException(nameof(item));
                list.Add(item);
            }

            return list;
        }

        public T Add<T>(T updatable) where T : IUpdatable {
            base.Add(updatable);
            return updatable;
        }

        public T[] AddRange<T>(T[] updatable) where T : IUpdatable {
            base.AddRange(updatable.Cast<IUpdatable>());
            return updatable;
        }
    }
}