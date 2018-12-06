using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserCodeGenerator.Tool
{
    public class MyStringBuilder
    {
        StringBuilder sb = new StringBuilder();
        public MyStringBuilder()
        {
        }

        public MyStringBuilder AppendTab()
        {
            sb.Append("\t");
            return this;
        }
        public MyStringBuilder AppendTab(int tabCount)
        {
            for (int i = 0; i < tabCount; ++i)
                sb.Append("\t");
            return this;
        }
        public MyStringBuilder AppendLine()
        {
            sb.AppendLine();
            return this;
        }
        public MyStringBuilder AppendLine(string value)
        {
            sb.AppendLine(value);
            return this;
        }
        public MyStringBuilder Append(string value)
        {
            sb.Append(value);
            return this;
        }

        public MyStringBuilder AppendFormat(string format, params object[] args)
        {
            sb.AppendFormat(format, args);
            return this;
        }

        public int Length
        {
            get => sb.Length;
            set => sb.Length = value;
        }

        public override string ToString()
        {
            return sb.ToString();
        }

    }
}
