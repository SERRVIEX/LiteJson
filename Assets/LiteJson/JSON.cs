namespace LiteJson
{
    using System;
    using System.Text;
    using System.Globalization;
    using System.Collections.Generic;

    public static class JSON
    {
        /// <summary>
        /// Use Unicode by default.
        /// </summary>
        public static bool ForceASCII = false;
        /// <summary>
        /// Allow "//"-style comments at the end of a line.
        /// </summary>
        public static bool AllowLineComments = true;

        internal static StringBuilder EscapeBuilder
        {
            get
            {
                if (_escapeBuilder == null)
                    _escapeBuilder = new StringBuilder();
                return _escapeBuilder;
            }
        }

        [ThreadStatic]
        private static StringBuilder _escapeBuilder;

        // Methods

        public static JSONNode Parse(string value)
        {
            Stack<JSONNode> stack = new Stack<JSONNode>();
            JSONNode context = null;

            StringBuilder token = new StringBuilder();
            string tokenName = string.Empty;

            bool quoteMode = false;
            bool tokenIsQuoted = false;

            int i = 0;
            while (i < value.Length)
            {
                switch (value[i])
                {
                    case '{':
                        if (quoteMode)
                        {
                            token.Append(value[i]);
                            break;
                        }

                        stack.Push(new JSONObject());

                        if (context != null)
                        {
                            if (context is JSONObject)
                                (context as JSONObject).Add(tokenName, stack.Peek());
                            else if (context is JSONArray)
                                (context as JSONArray).Add(stack.Peek());
                        }

                        tokenName = "";
                        token.Length = 0;
                        context = stack.Peek();
                        break;

                    case '[':
                        if (quoteMode)
                        {
                            token.Append(value[i]);
                            break;
                        }

                        stack.Push(new JSONArray());
                        if (context != null)
                        {
                            if (context is JSONObject)
                                (context as JSONObject).Add(tokenName, stack.Peek());
                            else if (context is JSONArray)
                                (context as JSONArray).Add(stack.Peek());
                        }

                        tokenName = "";
                        token.Length = 0;
                        context = stack.Peek();
                        break;

                    case '}':
                    case ']':
                        if (quoteMode)
                        {
                            token.Append(value[i]);
                            break;
                        }

                        if (stack.Count == 0)
                            throw new Exception("JSON Parse: Too many closing brackets");

                        stack.Pop();
                        if (token.Length > 0 || tokenIsQuoted)
                        {
                            if (context is JSONObject)
                                (context as JSONObject).Add(tokenName, ParseElement(token.ToString(), tokenIsQuoted));
                            else if (context is JSONArray)
                                (context as JSONArray).Add(ParseElement(token.ToString(), tokenIsQuoted));
                        }

                        tokenIsQuoted = false;
                        tokenName = "";
                        token.Length = 0;
                        if (stack.Count > 0)
                            context = stack.Peek();
                        break;

                    case ':':
                        if (quoteMode)
                        {
                            token.Append(value[i]);
                            break;
                        }
                        tokenName = token.ToString();
                        token.Length = 0;
                        tokenIsQuoted = false;
                        break;

                    case '"':
                        quoteMode ^= true;
                        tokenIsQuoted |= quoteMode;
                        break;

                    case ',':
                        if (quoteMode)
                        {
                            token.Append(value[i]);
                            break;
                        }
                        if (token.Length > 0 || tokenIsQuoted)
                        {
                            if (context is JSONObject)
                                (context as JSONObject).Add(tokenName, ParseElement(token.ToString(), tokenIsQuoted));
                            else if (context is JSONArray)
                                (context as JSONArray).Add(ParseElement(token.ToString(), tokenIsQuoted));
                        }
                        tokenIsQuoted = false;
                        tokenName = "";
                        token.Length = 0;
                        tokenIsQuoted = false;
                        break;

                    case '\r':
                    case '\n':
                        break;

                    case ' ':
                    case '\t':
                        if (quoteMode)
                            token.Append(value[i]);
                        break;

                    case '\\':
                        ++i;
                        if (quoteMode)
                        {
                            char C = value[i];
                            switch (C)
                            {
                                case 't':
                                    token.Append('\t');
                                    break;
                                case 'r':
                                    token.Append('\r');
                                    break;
                                case 'n':
                                    token.Append('\n');
                                    break;
                                case 'b':
                                    token.Append('\b');
                                    break;
                                case 'f':
                                    token.Append('\f');
                                    break;
                                case 'u':
                                    {
                                        string s = value.Substring(i + 1, 4);
                                        token.Append((char)int.Parse(s, NumberStyles.AllowHexSpecifier));
                                        i += 4;
                                        break;
                                    }
                                default:
                                    token.Append(C);
                                    break;
                            }
                        }
                        break;
                    case '/':
                        if (JSONNode.AllowLineComments && !quoteMode && i + 1 < value.Length && value[i + 1] == '/')
                        {
                            while (++i < value.Length && value[i] != '\n' && value[i] != '\r') ;
                            break;
                        }
                        token.Append(value[i]);
                        break;
                    case '\uFEFF': // Remove / ignore BOM (Byte Order Mark).
                        break;

                    default:
                        token.Append(value[i]);
                        break;
                }
                ++i;
            }

            if (quoteMode)
                throw new Exception("Json Parse: Quotation marks seems to be messed up.");

            if (context == null)
                return ParseElement(token.ToString(), tokenIsQuoted);
            return context;
        }

        private static JSONNode ParseElement(string token, bool quoted)
        {
            if (quoted)
                return token;

            if (token.Length <= 5)
            {
                string tmp = token.ToLower();

                if (tmp == "false" || tmp == "true")
                    return tmp == "true";

                if (tmp == "null")
                    return new JSONNull();
            }

            if (double.TryParse(token, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                return value;
            else
                return token;

           
        }

        internal static string Escape(string text)
        {
            StringBuilder builder = EscapeBuilder;
            builder.Length = 0;
            if (builder.Capacity < text.Length + text.Length / 10)
                builder.Capacity = text.Length + text.Length / 10;

            foreach (char c in text)
            {
                switch (c)
                {
                    case '\\':
                        builder.Append("\\\\");
                        break;
                    case '\"':
                        builder.Append("\\\"");
                        break;
                    case '\n':
                        builder.Append("\\n");
                        break;
                    case '\r':
                        builder.Append("\\r");
                        break;
                    case '\t':
                        builder.Append("\\t");
                        break;
                    case '\b':
                        builder.Append("\\b");
                        break;
                    case '\f':
                        builder.Append("\\f");
                        break;
                    default:
                        if (c < ' ' || (ForceASCII && c > 127))
                        {
                            ushort val = c;
                            builder.Append("\\u").Append(val.ToString("X4"));
                        }
                        else
                            builder.Append(c);
                        break;
                }
            }

            string result = builder.ToString();
            builder.Length = 0;
            return result;
        }
    }
}