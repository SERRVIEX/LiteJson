namespace LiteJson
{
    using System;
    using System.Text;
    using System.Collections.Generic;

    public abstract partial class JSONNode
    {
        public static bool AllowLineComments;

        public virtual JSONNodeType Type { get; }

        public JSONNode Parent { get; protected set; }
        public virtual int Count => 0;

        public string Key
        {
            get
            {
                if (Parent == null)
                    return "root";

                if (Parent.Type == JSONNodeType.Array)
                    return Parent.AsArray().IndexOf(this).ToString();

                return _key;
            }
        }

        private string _key;

        public string Value { get; protected set; }

        /// <summary>
        /// Is intended to get an item from the array.
        /// </summary>
        public virtual JSONNode this[int index]
        {
            get
            {
                string rootPath = GetRootPath();
                throw new Exception($"Failed to get {rootPath}[{index}] because this is intended only for JSONArray.");
            }
            set
            {
                string rootPath = GetRootPath();
                throw new Exception($"Failed to set {rootPath}[{index}] because this is intended only for JSONArray.");
            }
        }

        // Is intended to get an item from the object.
        public virtual JSONNode this[string key]
        {
            get
            {
                string rootPath = GetRootPath();
                throw new Exception($"Failed to get {rootPath}[\"{key}\"] because this is intended only for JSONObject.");
            }
            set
            {
                string rootPath = GetRootPath();
                throw new Exception($"Failed to set {rootPath}[\"{key}\"] because this is intended only for JSONObject.");
            }
        }

        // Methods

        /// <summary>
        /// Lazy creator intendede for object type.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="value"></param>
        public virtual void Create(string path, JSONNode value) => throw new Exception($"That function isn't implemented for '{Type}' type.");

        /// <summary>
        /// Intended for object type.
        /// </summary>
        public virtual bool Contains(string key) => throw new Exception($"That function isn't implemented for '{Type}' type.");

        /// <summary>
        /// Intended for array type.
        /// </summary>
        public virtual bool Contains(JSONNode value) => throw new Exception($"That function isn't implemented for '{Type}' type.");

        /// <summary>
        /// Intended for array type.
        /// </summary>
        public virtual int IndexOf(JSONNode value) => throw new Exception($"That function isn't implemented for '{Type}' type.");

        /// <summary>
        /// Intended for object type.
        /// </summary>
        public virtual void Add(string key, JSONNode value) => throw new Exception($"That method isn't implemented for '{Type}' type.");

        /// <summary>
        /// Intended for array type.
        /// </summary>
        public virtual void Add(JSONNode value) => throw new Exception($"That method isn't implemented for '{Type}' type.");

        /// <summary>
        /// Intended for array type.
        /// </summary>
        public virtual void Insert(int index, JSONNode value) => throw new Exception($"That method isn't implemented for '{Type}' type.");

        /// <summary>
        /// Intended for object type.
        /// </summary>
        public virtual void Remove(string key) => throw new Exception($"That method isn't implemented for '{Type}' type.");

        /// <summary>
        /// Intended for array type.
        /// </summary>
        public virtual void Remove(JSONNode value) => throw new Exception($"That method isn't implemented for '{Type}' type.");

        /// <summary>
        /// Intended for array type.
        /// </summary>
        public virtual void RemoveAt(int index) => throw new Exception($"That method isn't implemented for '{Type}' type.");

        /// <summary>
        /// Return the path to the root.
        /// </summary>
        protected string GetRootPath()
        {
            List<(string key, JSONNode value)> nodes = new List<(string key, JSONNode value)>();
            return GetRootPath(this, nodes);
        }

        protected string GetRootPath(JSONNode node, List<(string key, JSONNode value)> nodes)
        {
            nodes.Add((node.Key, node));

            if (node.Parent == null)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("JSON");

                for (int i = nodes.Count - 1; i >= 0; i--)
                {
                    var item = nodes[i];
                    if (item.value.Parent != null)
                    {
                        if (item.value.Parent.Type == JSONNodeType.Object)
                            builder.Append($"[\"{item.key}\"]");

                        else if (item.value.Parent.Type == JSONNodeType.Array)
                            builder.Append($"[{item.key}]");
                    }
                }

                return builder.ToString();
            }

            return GetRootPath(node.Parent, nodes);
        }

        /// <summary>
        /// Set the key if the parent node is an object type.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="key"></param>
        protected void SetKey(JSONNode target, string key)
        {
            if (target == null)
                throw new Exception("The key cannot be set because the target is null.");

            target._key = key;
        }

        /// <summary>
        /// Remove the key when removed from the parent node of type object.
        /// </summary>
        /// <param name="target"></param>
        protected void RemoveKey(JSONNode target)
        {
            if (target == null)
                throw new Exception("The key cannot be remove because the target is null.");

            target._key = string.Empty;
        }

        protected void SetParent(JSONNode target, JSONNode parent)
        {
            if (target == null)
                throw new Exception("The parent cannot be set because the target is null.");

            if (parent == null)
                throw new Exception("The parent cannot be set because the parent is null.");

            target.Parent = parent;
        }

        protected void RemoveParent(JSONNode target)
        {
            if (target == null)
                throw new Exception("The parent cannot be deleted because the target is null.");

            target.Parent = null;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            WriteToStringBuilder(builder, 0, 0, JSONTextMode.Compact);
            return builder.ToString();
        }

        public virtual string ToString(int indent)
        {
            StringBuilder builder = new StringBuilder();
            WriteToStringBuilder(builder, 0, indent, JSONTextMode.Indent);
            return builder.ToString();
        }

        internal abstract void WriteToStringBuilder(StringBuilder builder, int indent, int indentInc, JSONTextMode mode);

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            return AsString() == obj.ToString();
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}