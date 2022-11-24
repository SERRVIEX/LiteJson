namespace LiteJson
{
    using System;
    using System.Text;
    using System.Collections.Generic;

    public sealed class JSONArray : JSONNode
    {
        public override JSONType Type => JSONType.Array;

        private List<JSONNode> _objects = new List<JSONNode>();
        public override int Count => _objects.Count;

        public override JSONNode this[int index]
        {
            get
            {
                if (index < 0 || index >= _objects.Count)
                {
                    string rootPath = GetRootPath();
                    throw new Exception($"Failed to get {rootPath} because index '{index}' is out of range.");
                }

                return _objects[index];
            }
            set
            {
                if (index < 0 || index >= _objects.Count)
                {
                    string rootPath = GetRootPath();
                    throw new Exception($"Failed to set {rootPath} because index '{index}' is out of range.");
                }
                else
                {
                    if (value == null)
                        value = new JSONNull();

                    JSONNode node = _objects[index];
                    RemoveParent(node);

                    SetParent(value, this);
                    _objects[index] = value;
                }
            }
        }

        // Methods

        public override bool Contains(JSONNode value) => _objects.Contains(value);
        public override int IndexOf(JSONNode value) => _objects.IndexOf(value);

        public override void Add(JSONNode value)
        {
            if (value == null)
                value = new JSONNull();

            SetParent(value, this);
            _objects.Add(value);
        }

        public override void Insert(int index, JSONNode value)
        {
            if (value == null)
                value = new JSONNull();

            SetParent(value, this);
            _objects.Insert(index, value);
        }

        public override void Remove(JSONNode value)
        {
            RemoveParent(value);
            _objects.Remove(value);
        }

        public override void RemoveAt(int index)
        {
            if (index < 0 || index >= _objects.Count)
                throw new Exception($"Failed to remove at {index} because index is out of range.");

            JSONNode node = _objects[index];
            RemoveParent(node);
            _objects.RemoveAt(index);
        }

        public override Enumerator GetEnumerator() => new Enumerator(_objects.GetEnumerator());

        internal override void WriteToStringBuilder(StringBuilder builder, int indent, int indentInc, JSONTextMode mode)
        {
            builder.Append('[');
            int count = _objects.Count;
            for (int i = 0; i < count; i++)
            {
                if (i > 0)
                    builder.Append(',');

                if (mode == JSONTextMode.Indent)
                    builder.AppendLine();

                if (mode == JSONTextMode.Indent)
                    builder.Append(' ', indent + indentInc);

                _objects[i].WriteToStringBuilder(builder, indent + indentInc, indentInc, mode);
            }
            if (mode == JSONTextMode.Indent)
                builder.AppendLine().Append(' ', indent);

            builder.Append(']');
        }
    }
}