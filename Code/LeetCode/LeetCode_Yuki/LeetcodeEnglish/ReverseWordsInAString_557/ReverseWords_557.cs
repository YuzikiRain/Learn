using System;
using System.Text;

namespace LeetCode_Yuki.ReverseWordsInAString_557
{
    class ReverseWords_557
    {
        public void Test()
        {
            string test = "Let's take LeetCode contest";
            Console.Write($"{ReverseWords(test)}");
        }

        public string ReverseWords(string s)
        {
            StringBuilder sb = new StringBuilder(s);
            sb.Append(' ');

            int index = 0;
            for (int i = 0; i < sb.Length; i++)
            {
                if (sb[i].Equals(' '))
                {
                    ReverseStringBetween(sb, index, i - 1);
                    index = i + 1;
                }
            }

            return sb.Remove(sb.Length-1, 1).ToString();
        }

        public void ReverseStringBetween(StringBuilder sb, int from, int to)
        {
            char temp;
            for (int i = from, j = to; i < j; i++, j--)
            {
                temp = sb[i];
                sb[i] = sb[j];
                sb[j] = temp;
            }
        }
    }
}
