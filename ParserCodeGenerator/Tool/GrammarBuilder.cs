using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ParserCodeGenerator.Tool
{
    public class GrammarBuilder
    {
        public class Token
        {
            public int Index { get; set; }
            public int Length { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }

            public Token(int index, int length, string name, string value)
            {
                Index = index;
                Length = length;
                Name = name;
                Value = value;
            }
        }

        public delegate void DelegateProgress(double progress);
        Dictionary<string, string> tokens;

        public Exception LastError { get; set; }

        public event DelegateProgress ProgressDelegate;


        public GrammarBuilder()
        {
            tokens = new Dictionary<string, string>();
            LastError = null;
        }

        public List<Token> Generate(string text)
        {
            Dictionary<string, List<Match>> prepare = new Dictionary<string, List<Match>>();
            foreach (KeyValuePair<string, string> pair in tokens)
            {
                var t = Regex.Matches(text, pair.Value);
                foreach (Match m in t)
                {
                    if (m.Success)
                    {
                        if (!prepare.ContainsKey(pair.Key))
                            prepare[pair.Key] = new List<Match>();
                        prepare[pair.Key].Add(m);
                    }
                }
            }

            List<Token> result = new List<Token>();
            int index = 0;
            bool success = false;
            while (true)
            {
                if (index >= text.Length)
                    break;
                success = false;
                foreach (KeyValuePair<string, List<Match>> pair in prepare)
                {
                    List<Match> list = pair.Value;
                    for (int i = 0; i < list.Count; ++i)
                    {
                        Match m = list[i];
                        if (m.Index == index)
                        {
                            if (m.Length == 0)
                                continue;
                            index += m.Length;
                            result.Add(new Token(m.Index, m.Length, pair.Key, m.Value));
                            if (ProgressDelegate != null)
                                ProgressDelegate((double)index / (double)text.Length);
                            success = true;
                            list.RemoveAt(i);
                            break;
                        }
                    }
                    if (success)
                        break;
                }

                if (success)
                    continue;


                if (ProgressDelegate != null)
                    ProgressDelegate((double)index / (double)text.Length);
                result.Add(new Token(index, 1, "UNDEFINED", text[index].ToString()));
                ++index;
            }

            if (ProgressDelegate != null)
                ProgressDelegate(1.0);
            return result;
        }

        bool Check(Match m, List<Token> list)
        {
            for (int i = 0; i < list.Count; ++i)
            {
                if (list[i].Index <= m.Index &&
                    list[i].Index + list[i].Length > m.Index)
                    return false;
            }

            return true;
        }

        public List<Token> Generate2(string text)
        {
            List<Token> result = new List<Token>();

            string temp = text;
            int count = 0;
            int index = 0;
            foreach (KeyValuePair<string, string> pair in this.tokens)
            {
                Regex r = new Regex(pair.Value);
                MatchCollection mc = r.Matches(temp, index);

                foreach (Match m in mc)
                {
                    if (m.Success)
                    {
                        if (m.Length == 0)
                            continue;

                        if (!Check(m, result))
                            continue;
                        if (index == m.Index)
                            index += m.Length;
                        Token ttt = new Token(m.Index, m.Length, pair.Key, m.Value);
                        result.Add(ttt);
                    }
                }
                ++count;
                if (ProgressDelegate != null)
                    ProgressDelegate((double)count / (double)(this.tokens.Count + 1));
            }
            result.Sort(delegate (Token a, Token b)
            {
                return a.Index - b.Index;
            });

            index = 0;
            for (int i = 1; i < result.Count; ++i)
            {
                Token a = result[i - 1];
                Token b = result[i];

                index = a.Index + a.Length;

                if (index < b.Index)
                    result.Insert(i, new Token(index, 1, "UNDEFINED", text[index].ToString()));
            }
            if (ProgressDelegate != null)
                ProgressDelegate(1.0);

            return result;
        }

        public bool AddRegExToken(string regEx, string identifier)
        {
            if (tokens.ContainsKey(identifier.Trim()))
                return false;

            identifier = identifier.Trim();
            if (CheckValidity(regEx) == false)
            {
                return false;
            }
            tokens.Add(identifier, regEx);

            return true;
        }

        private bool CheckValidity(string regEx)
        {
            try
            {
                Regex r = new Regex(regEx);

                r.Match("");
            }
            catch (Exception ex)
            {
                LastError = ex;
                return false;
            }

            return true;
        }


        public void Clear()
        {
            tokens.Clear();
        }
    }
}
