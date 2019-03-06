using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Mathematica
{
    public class MathElementCollection : IList<MathElement>, INotifyCollectionChanged
    {
        private readonly IList<MathElement> _listImplementation;

        public MathElementCollection()
        {
            _listImplementation = new List<MathElement>();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this,e);
        }
        public IEnumerator<MathElement> GetEnumerator()
        {
            return _listImplementation.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _listImplementation).GetEnumerator();
        }

        public void Add(MathElement item)
        {
            _listImplementation.Add(item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs
                (NotifyCollectionChangedAction.Add, item));
        }

        public void Clear()
        {
            _listImplementation.Clear();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs
                (NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(MathElement item)
        {
            return _listImplementation.Contains(item);
        }

        public void CopyTo(MathElement[] array, int arrayIndex)
        {
            _listImplementation.CopyTo(array, arrayIndex);
        }

        public bool Remove(MathElement item)
        {
            if (_listImplementation.Remove(item))
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs
                    (NotifyCollectionChangedAction.Remove, item));
                return true;
            }

            return false;
        }

        public int Count => _listImplementation.Count;

        public bool IsReadOnly => _listImplementation.IsReadOnly;

        public int IndexOf(MathElement item)
        {
            return _listImplementation.IndexOf(item);
        }

        public void Insert(int index, MathElement item)
        {
            _listImplementation.Insert(index, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs
                (NotifyCollectionChangedAction.Add, item, index));
        }

        public void RemoveAt(int index)
        {
            var item = _listImplementation[index];
            _listImplementation.RemoveAt(index);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
        }

        public MathElement this[int index]
        {
            get => _listImplementation[index];
            set => _listImplementation[index] = value;
        }
    }
}