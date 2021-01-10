using System.ComponentModel;
using System.Collections.Specialized;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable enable

namespace MaxLib.Common.Collections
{
    public class ObservableDictionary<Key, Value> : IDictionary<Key, Value>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        readonly Dictionary<Key, Value> dict = new Dictionary<Key, Value>();

        public Value this[Key key]
        {
            get => dict[key];
            set
            {
                dict[key] = value;
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Replace,
                    new KeyValuePair<Key, Value>(key, value)
                ));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
            }
        }

        public ICollection<Key> Keys => dict.Keys;

        public ICollection<Value> Values => dict.Values;

        public int Count => dict.Count;

        public bool IsReadOnly => false;

        public event NotifyCollectionChangedEventHandler? CollectionChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        public void Add(Key key, Value value)
        {
            dict.Add(key, value);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Add,
                new KeyValuePair<Key, Value>(key, value)
            ));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
        }

        void ICollection<KeyValuePair<Key, Value>>.Add(KeyValuePair<Key, Value> item)
            => Add(item.Key, item.Value);

        public void Clear()
        {
            dict.Clear();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Reset
            ));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
        }

        bool ICollection<KeyValuePair<Key, Value>>.Contains(KeyValuePair<Key, Value> item)
            => ((ICollection<KeyValuePair<Key, Value>>)dict).Contains(item);

        public bool ContainsKey(Key key)
            => dict.ContainsKey(key);

        void ICollection<KeyValuePair<Key, Value>>.CopyTo(KeyValuePair<Key, Value>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<Key, Value>>)dict).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<Key, Value>> GetEnumerator()
            => dict.GetEnumerator();

        public bool Remove(Key key)
        {
            if (dict.Remove(key))
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Remove,
                    key
                ));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
                return true;
            }
            else return false;
        }

        bool ICollection<KeyValuePair<Key, Value>>.Remove(KeyValuePair<Key, Value> item)
        {
            return Remove(item.Key);
        }

        public bool TryGetValue(Key key, out Value value)
        {
            return dict.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}