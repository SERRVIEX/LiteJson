namespace LiteJson
{
    using System.Globalization;

    using UnityEngine;

    public abstract partial class JSONNode
    {
        public JSONObject AsObject()
        {
            if (this is not JSONObject)
                Debug.LogError($"Can't be converted to object.");

            return this as JSONObject;
        }

        public JSONArray AsArray()
        {
            if (this is not JSONArray)
                Debug.LogError($"Can't be converted to array.");

            return this as JSONArray;
        }

        public string AsString() => Value;

        public virtual bool AsBool()
        {
            if (!bool.TryParse(Value, out bool result))
                Debug.LogError($"Value '{Value}' of key '{Key}' could not be parsed as bool.");

            return result;
        }

        public virtual int AsInt()
        {
            if (!int.TryParse(Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int result))
                Debug.LogError($"Value '{Value}' of key '{Key}' could not be parsed as int.");

            return result;
        }

        public virtual float AsFloat()
        {
            if (!float.TryParse(Value, NumberStyles.Float, CultureInfo.InvariantCulture, out float result))
                Debug.LogError($"Value '{Value}' of key '{Key}' could not be parsed as float.");

            return result;
        }

        public virtual double AsDouble()
        {
            if (!double.TryParse(Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double result))
                Debug.LogError($"Value '{Value}' of key '{Key}' could not be parsed as double.");

            return result;
        }

        public virtual long AsLong()
        {
            if (!long.TryParse(Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out long result))
                Debug.LogError($"Value '{Value}' of key '{Key}' could not be parsed as long.");

            return result;
        }
    }
}