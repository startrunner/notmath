using System.Collections;
using System.Collections.Generic;

namespace Mathematica.Models
{
    public class MathDocumentCollection : IList<MathDocument>
    {
        private readonly IList<MathDocument> _listImplementation;

        public MathDocumentCollection()
        {
            _listImplementation = new List<MathDocument>();
        }

        public IEnumerator<MathDocument> GetEnumerator()
        {
            return _listImplementation.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _listImplementation).GetEnumerator();
        }

        public void Add(MathDocument item)
        {
            _listImplementation.Add(item);
        }

        public void Clear()
        {
            _listImplementation.Clear();
        }

        public bool Contains(MathDocument item)
        {
            return _listImplementation.Contains(item);
        }

        public void CopyTo(MathDocument[] array, int arrayIndex)
        {
            _listImplementation.CopyTo(array, arrayIndex);
        }

        public bool Remove(MathDocument item)
        {
            return _listImplementation.Remove(item);
        }

        public int Count => _listImplementation.Count;

        public bool IsReadOnly => _listImplementation.IsReadOnly;

        public int IndexOf(MathDocument item)
        {
            return _listImplementation.IndexOf(item);
        }

        public void Insert(int index, MathDocument item)
        {
            _listImplementation.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _listImplementation.RemoveAt(index);
        }

        public MathDocument this[int index]
        {
            get => _listImplementation[index];
            set => _listImplementation[index] = value;
        }
    }
}