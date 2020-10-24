namespace Excel.Core
{
    using System;
    using System.Runtime.CompilerServices;

    public class FormatReader
    {
        private const char escapeChar = '\\';

        public bool IsDateFormatString()
        {
            char[] anyOf = new char[] { 'y', 'm', 'd', 's', 'h', 'Y', 'M', 'D', 'S', 'H' };
            if (this.FormatString.IndexOfAny(anyOf) >= 0)
            {
                foreach (char ch in anyOf)
                {
                    for (int i = this.FormatString.IndexOf(ch); i > -1; i = this.FormatString.IndexOf(ch, i + 1))
                    {
                        if ((!this.IsSurroundedByBracket(ch, i) && !this.IsPrecededByBackSlash(ch, i)) && !this.IsSurroundedByQuotes(ch, i))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool IsPrecededByBackSlash(char dateChar, int pos)
        {
            if (pos == 0)
            {
                return false;
            }
            char ch = this.FormatString[pos - 1];
            return (ch.CompareTo('\\') == 0);
        }

        private bool IsSurroundedByBracket(char dateChar, int pos)
        {
            if (pos == (this.FormatString.Length - 1))
            {
                return false;
            }
            int num = this.NumberOfUnescapedOccurances('[', this.FormatString.Substring(0, pos));
            int num2 = this.NumberOfUnescapedOccurances(']', this.FormatString.Substring(0, pos));
            num -= num2;
            int num3 = this.NumberOfUnescapedOccurances('[', this.FormatString.Substring(pos + 1));
            int num4 = this.NumberOfUnescapedOccurances(']', this.FormatString.Substring(pos + 1)) - num3;
            return (((num % 2) == 1) && ((num4 % 2) == 1));
        }

        private bool IsSurroundedByQuotes(char dateChar, int pos)
        {
            if (pos == (this.FormatString.Length - 1))
            {
                return false;
            }
            int num = this.NumberOfUnescapedOccurances('"', this.FormatString.Substring(pos + 1));
            int num2 = this.NumberOfUnescapedOccurances('"', this.FormatString.Substring(0, pos));
            return (((num % 2) == 1) && ((num2 % 2) == 1));
        }

        private int NumberOfUnescapedOccurances(char value, string src)
        {
            int num = 0;
            char ch = '\0';
            foreach (char ch2 in src)
            {
                if ((ch2 == value) && ((ch == '\0') || (ch.CompareTo('\\') != 0)))
                {
                    num++;
                    ch = ch2;
                }
            }
            return num;
        }

        public string FormatString { get; set; }
    }
}

