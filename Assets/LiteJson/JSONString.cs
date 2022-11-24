namespace LiteJson
{
    using System.Text;

    public sealed class JSONString : JSONNode
    {
        public override JSONNodeType Type => JSONNodeType.String;

        // Constructors

        public JSONString(string value) => Value = value;

        // Methods

        public override Enumerator GetEnumerator() => new Enumerator();
        internal override void WriteToStringBuilder(StringBuilder builder, int indent, int indentInc, JSONTextMode mode) => builder.Append('\"').Append(Value).Append('\"');
    }
}