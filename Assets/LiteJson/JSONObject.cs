namespace LiteJson
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class JSONObject : JSONNode
    {
        public override JSONNodeType Type => JSONNodeType.Object;

        private Dictionary<string, JSONNode> _objects = new Dictionary<string, JSONNode>();
        public override int Count => _objects.Count;

        public override JSONNode this[string key]
        {
            get
            {
                if (!_objects.ContainsKey(key))
                {
                    string rootPath = GetRootPath();
                    throw new Exception($"Failed to get {rootPath}[\"{key}\"] because key '{key}' does not exist.");
                }

                return _objects[key];
            }
            set
            {
                if (value == null)
                    value = new JSONNull();
              
                SetKey(value, key);
                SetParent(value, this);

                if (_objects.ContainsKey(key))
                {
                    JSONNode node = _objects[key];
                    RemoveKey(node);
                    RemoveParent(node);

                    _objects[key] = value;
                }
                else
                    _objects.Add(key, value);
            }
        }

        public override JSONNode this[int index]
        {
            get
            {
                if (index < 0 || index >= _objects.Count)
                    return null;
                return _objects.ElementAt(index).Value;
            }
            set
            {
                if (value == null)
                    value = new JSONNull();

                SetKey(value, index.ToString());
                SetParent(value, this);

                if (_objects.ContainsKey(index.ToString()))
                {
                    JSONNode node = _objects[index.ToString()];
                    RemoveKey(node);
                    RemoveParent(node);

                    _objects[index.ToString()] = value;
                }
                else
                    _objects.Add(index.ToString(), value);
            }
        }

        // Methods

        public override bool Contains(string key) => _objects.ContainsKey(key);

        public override void Add(string key, JSONNode value)
        {
            if (value == null)
                value = new JSONNull();

            SetKey(value, key);
            SetParent(value, this);
            _objects.Add(key, value);
        }

        public override void Remove(string key)
        {
            if (_objects.TryGetValue(key, out JSONNode value))
            {
                RemoveKey(value);
                RemoveParent(value);
                _objects.Remove(key);
            }
        }

        public override Enumerator GetEnumerator() => new Enumerator(_objects.GetEnumerator());

        internal override void WriteToStringBuilder(StringBuilder builder, int indent, int indentInc, JSONTextMode mode)
        {
            builder.Append('{');
            bool first = true;
            foreach (var item in _objects)
            {
                if (!first)
                    builder.Append(',');

                first = false;

                if (mode == JSONTextMode.Indent)
                    builder.AppendLine();

                if (mode == JSONTextMode.Indent)
                    builder.Append(' ', indent + indentInc);

                builder.Append('\"').Append(JSON.Escape(item.Key)).Append('\"');

                if (mode == JSONTextMode.Compact)
                    builder.Append(':');
                else
                    builder.Append(" : ");

                item.Value.WriteToStringBuilder(builder, indent + indentInc, indentInc, mode);
            }

            if (mode == JSONTextMode.Indent)
                builder.AppendLine().Append(' ', indent);
            builder.Append('}');
        }

    }
}