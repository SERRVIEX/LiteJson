namespace LiteJson
{
    using System.Collections.Generic;

    using UnityEngine;

    public abstract partial class JSONNode
    {
        public static implicit operator JSONNode(string value) => value == null ? new JSONNull() : new JSONString(value);
        public static implicit operator string(JSONNode value) => value == null ? new JSONNull() : value.Value;

        public static implicit operator JSONNode(bool value) => new JSONBool(value);
        public static implicit operator bool(JSONNode value) => value == null ? false : value.AsBool();

        public static implicit operator JSONNode(int value) => new JSONNumber(value);
        public static implicit operator int(JSONNode value) => value == null ? 0 : value.AsInt();

        public static implicit operator JSONNode(float value) => new JSONNumber(value);
        public static implicit operator float(JSONNode value) => value == null ? 0f : value.AsFloat();

        public static implicit operator JSONNode(double value) => new JSONNumber(value);
        public static implicit operator double(JSONNode value) => value == null ? 0 : value.AsDouble();

        public static implicit operator JSONNode(long value) => new JSONNumber(value);
        public static implicit operator long(JSONNode value) => value == null ? 0L : value.AsLong();

        public static implicit operator JSONNode(KeyValuePair<string, JSONNode> keyValue) => keyValue.Value;

        public static implicit operator JSONNode(Vector2 value)
        {
            JSONArray node = new JSONArray();
            node.Add(value.x);
            node.Add(value.y);
            return node;
        }

        public static implicit operator Vector2(JSONNode value)
        {
            Vector2 vector = new Vector2(value[0], value[1]);
            return vector;
        }

        public static implicit operator JSONNode(Vector2Int value)
        {
            JSONArray node = new JSONArray();
            node.Add(value.x);
            node.Add(value.y);
            return node;
        }

        public static implicit operator Vector2Int(JSONNode value)
        {
            Vector2Int vector = new Vector2Int(value[0], value[1]);
            return vector;
        }

        public static implicit operator JSONNode(Vector3 value)
        {
            JSONArray node = new JSONArray();
            node.Add(value.x);
            node.Add(value.y);
            node.Add(value.z);
            return node;
        }

        public static implicit operator Vector3(JSONNode value)
        {
            Vector3 vector = new Vector3(value[0], value[1], value[2]);
            return vector;
        }

        public static implicit operator JSONNode(Vector3Int value)
        {
            JSONArray node = new JSONArray();
            node.Add(value.x);
            node.Add(value.y);
            node.Add(value.z);
            return node;
        }

        public static implicit operator Vector3Int(JSONNode value)
        {
            Vector3Int vector = new Vector3Int(value[0], value[1], value[2]);
            return vector;
        }

        public static implicit operator JSONNode(Vector4 value)
        {
            JSONArray node = new JSONArray();
            node.Add(value.x);
            node.Add(value.y);
            node.Add(value.z);
            node.Add(value.w);
            return node;
        }

        public static implicit operator Vector4(JSONNode value)
        {
            Vector4 vector = new Vector4(value[0], value[1], value[2], value[3]);
            return vector;
        }

        public static implicit operator JSONNode(Quaternion value)
        {
            JSONArray node = new JSONArray();
            node.Add(value.x);
            node.Add(value.y);
            node.Add(value.z);
            node.Add(value.w);
            return node;
        }

        public static implicit operator Quaternion(JSONNode value)
        {
            Quaternion quaternion = new Quaternion();
            quaternion.x = value[0];
            quaternion.y = value[1];
            quaternion.z = value[2];
            quaternion.w = value[3];
            return quaternion;
        }

        public static implicit operator JSONNode(Color value)
        {
            JSONArray node = new JSONArray();
            node.Add(value.r);
            node.Add(value.g);
            node.Add(value.b);
            node.Add(value.a);
            return node;
        }

        public static implicit operator Color(JSONNode value)
        {
            Color vector = new Color(value[0], value[1], value[2], value[3]);
            return vector;
        }

        public static implicit operator JSONNode(Color32 value)
        {
            JSONArray node = new JSONArray();
            node.Add(value.r);
            node.Add(value.g);
            node.Add(value.b);
            node.Add(value.a);
            return node;
        }

        public static implicit operator Color32(JSONNode value)
        {
            Color32 vector = new Color32((byte)value[0], (byte)value[1], (byte)value[2], (byte)value[3]);
            return vector;
        }

        public static bool operator ==(JSONNode a, object b)
        {
            bool aIsNull = a is null;
            bool bIsNull = b is null;

            if (aIsNull && bIsNull)
                return true;

            if (aIsNull != bIsNull)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(JSONNode a, object b) => !(a == b);
    }
}