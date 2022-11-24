namespace LiteJson
{
    using System.Text;

    public sealed class JSONNull : JSONNode
    {
        public override JSONNodeType Type => JSONNodeType.Null;

        // Methods

        public override Enumerator GetEnumerator() => new Enumerator();
        internal override void WriteToStringBuilder(StringBuilder builder, int indent, int indentInc, JSONTextMode mode) => builder.Append("null");
    }
}