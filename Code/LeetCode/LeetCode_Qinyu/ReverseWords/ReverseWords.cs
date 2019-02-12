using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode_Qinyu.ReverseWords
{
    class ReverseWords_557
    {
        public void Test()
        {
            string test = "Let's take LeetCode contest";

            Console.Write($"{ReverseWords(test)}");

            //Console.ReadLine();
        }

        public string ReverseWords(string s)
        {
            StringBuilder sb = new StringBuilder();
            string[] words = s.Split(' ');
            for(int i = 0; i < words.Length; i++)
            {
                sb.Append(words[i] = Reverse(words[i]));
                sb.Append(' ');
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
        string Reverse(string str)
        {
            StringBuilder sb = new StringBuilder(str);
            for (int i = 0, j = str.Length - 1; i < j; i++, j--)
            {
                char temp = sb[i];
                sb[i] = sb[j];
                sb[j] = temp;
            }
            return sb.ToString();
        }
    }
}
