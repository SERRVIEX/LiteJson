namespace LiteJson
{
    using System.Collections;
    using System.Collections.Generic;

    public abstract partial class JSONNode
    {
        public IEnumerable<KeyValuePair<string, JSONNode>> Linq => new LinqEnumerator(this);
        public KeyEnumerator Keys => new KeyEnumerator(GetEnumerator());
        public ValueEnumerator Values => new ValueEnumerator(GetEnumerator());

        // Methods
        public abstract Enumerator GetEnumerator();
 
        // Other

        public struct Enumerator
        {
            private enum Type { None, Array, Object }
            private Type _type;

            private Dictionary<string, JSONNode>.Enumerator _objects;
            private List<JSONNode>.Enumerator _array;
            public bool IsValid => _type != Type.None;

            public KeyValuePair<string, JSONNode> Current
            {
                get
                {
                    if (_type == Type.Array)
                        return new KeyValuePair<string, JSONNode>(string.Empty, _array.Current);
                    else if (_type == Type.Object)
                        return _objects.Current;

                    return new KeyValuePair<string, JSONNode>(string.Empty, null);
                }
            }

            // Constructors

            public Enumerator(List<JSONNode>.Enumerator aArrayEnum)
            {
                _type = Type.Array;
                _objects = default(Dictionary<string, JSONNode>.Enumerator);
                _array = aArrayEnum;
            }

            public Enumerator(Dictionary<string, JSONNode>.Enumerator aDictEnum)
            {
                _type = Type.Object;
                _objects = aDictEnum;
                _array = default(List<JSONNode>.Enumerator);
            }

            // Methods
           
            public bool MoveNext()
            {
                if (_type == Type.Array)
                    return _array.MoveNext();
                else if (_type == Type.Object)
                    return _objects.MoveNext();

                return false;
            }
        }

        public struct ValueEnumerator
        {
            private Enumerator _enumerator;
            public JSONNode Current => _enumerator.Current.Value;

            // Constructors

            public ValueEnumerator(List<JSONNode>.Enumerator arrayEnum) : this(new Enumerator(arrayEnum)) { }
            public ValueEnumerator(Dictionary<string, JSONNode>.Enumerator dictEnum) : this(new Enumerator(dictEnum)) { }
            public ValueEnumerator(Enumerator enumerator) { _enumerator = enumerator; }

            // Methods

            public bool MoveNext() => _enumerator.MoveNext();
            public ValueEnumerator GetEnumerator() => this;
        }

        public struct KeyEnumerator
        {
            private Enumerator _enumerator;
            public string Current => _enumerator.Current.Key;

            // Constructors

            public KeyEnumerator(List<JSONNode>.Enumerator arrayEnum) : this(new Enumerator(arrayEnum)) { }
            public KeyEnumerator(Dictionary<string, JSONNode>.Enumerator dictEnum) : this(new Enumerator(dictEnum)) { }
            public KeyEnumerator(Enumerator enumerator) { _enumerator = enumerator; }

            // Methods

            public bool MoveNext() => _enumerator.MoveNext();
            public KeyEnumerator GetEnumerator() => this;
        }

        public class LinqEnumerator : IEnumerator<KeyValuePair<string, JSONNode>>, IEnumerable<KeyValuePair<string, JSONNode>>
        {
            private JSONNode _node;
            private Enumerator _enumerator;

            public KeyValuePair<string, JSONNode> Current => _enumerator.Current;
            object IEnumerator.Current => _enumerator.Current;

            public virtual IEnumerable<JSONNode> Children
            {
                get
                {
                    yield break;
                }
            }


            // Constructors

            internal LinqEnumerator(JSONNode node)
            {
                _node = node;
                if (_node != null)
                    _enumerator = _node.GetEnumerator();
            }

            // Methods

            public bool MoveNext() { return _enumerator.MoveNext(); }

            public void Dispose()
            {
                _node = null;
                _enumerator = new Enumerator();
            }

            public IEnumerator<KeyValuePair<string, JSONNode>> GetEnumerator() => new LinqEnumerator(_node);

            public void Reset()
            {
                if (_node != null)
                    _enumerator = _node.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new LinqEnumerator(_node);
            }
        }
    }
}