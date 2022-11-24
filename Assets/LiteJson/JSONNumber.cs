namespace LiteJson
{
    using System.Text;

    public sealed class JSONNumber : JSONNode
    {
        public override JSONNodeType Type => JSONNodeType.Number;
        private double _value;

        // Constructors

        public JSONNumber(double value)
        {
            Value = value.ToString();
            _value = value;
        }

        // Methods

        public override int AsInt() => (int)_value;
        public override float AsFloat() => (float)_value;
        public override double AsDouble() => _value;
        public override Enumerator GetEnumerator() => new Enumerator();
        internal override void WriteToStringBuilder(StringBuilder builder, int indent, int indentInc, JSONTextMode mode) => builder.Append(Value);

        // Operators

        public static implicit operator JSONNumber(int value) => new JSONNumber(value);
        public static implicit operator int(JSONNumber value) => value == null ? 0 : value.AsInt();

        public static implicit operator JSONNumber(float value) => new JSONNumber(value);
        public static implicit operator float(JSONNumber value) => value == null ? 0 : value.AsFloat();

        public static implicit operator JSONNumber(double value) => new JSONNumber(value);
        public static implicit operator double(JSONNumber value) => value == null ? 0 : value.AsDouble();
    }
}