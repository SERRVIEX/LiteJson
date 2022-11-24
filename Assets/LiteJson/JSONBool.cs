namespace LiteJson
{
    using System.Text;

    public sealed class JSONBool : JSONNode
    {
        public override JSONType Type => JSONType.Boolean;
        private bool _value;

        // Constructors

        public JSONBool(bool value)
        {
            Value = value.ToString();
            _value = value;
        }

        // Methods

        public override bool AsBool() => _value;
        public override Enumerator GetEnumerator() => new Enumerator();
        internal override void WriteToStringBuilder(StringBuilder builder, int indent, int indentInc, JSONTextMode mode) => builder.Append(Value);
    }
}