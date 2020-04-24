using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode_Yuki.ReverseWordsInAString_151
{
    class ReverseWordsInAString_151
    {
        public void Test()
        {
            string s = "the    sky       is blue";
            //string s = "   the sky is blue";
            //string s = "   a   b ";
            //string s = "   a   b  c d   e  ";
            //string s = " ";
            //string s = " 1";

            s = ReverseWords(s);
            Console.WriteLine($"{s}");
        }

        public string ReverseWords(string s)
        {
            if (s.Length.Equals(0))
            {
                return string.Empty;
            }
            else if (s.Equals(' '))
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder( s + ' ');

            int i = 0;
            int head = 0;
            int middle = 0;

            while (i < sb.Length)
            {
                // skip space
                while (i < sb.Length && sb[i].Equals(' '))
                {
                    i++;
                }

                middle = i;

                // move to end of word
                while (i < sb.Length && !sb[i].Equals(' '))
                {
                    i++;
                }

                if (i >= sb.Length) break;

                // only reverse non-space character to forward
                // for example, "b----a"  when head is 2 and tail is 5(end), no need to swap space in index 3 and 4, only swap non-space character 'a' from index 5 to 2
                ReverseStringBetweenNonSpace(sb, head, i - 1);

                // move index head to end of last word
                // for example, "b----a"  when head is 2 ,middle and tail is 5(end), after reverse, string is "b-a---" and head is 4
                head = i - (middle - head) + 1;
                //Console.WriteLine($"{sb.ToString()} head = {head} tail  = {tail} middle = {middle} i = {i}");
            }

            // means string is form with space
            if (head == 0) { return string.Empty; }

            sb.Remove(head - 1, (sb.Length - 1) - (head - 1) + 1);
            ReverseStringBetween(sb, 0, sb.Length - 1);
            return sb.ToString();
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

        public void ReverseStringBetweenNonSpace(StringBuilder sb, int from, int to)
        {
            char temp;
            for (; from < to; from++, to--)
            {
                if (!sb[to].Equals(' '))
                {
                    temp = sb[from];
                    sb[from] = sb[to];
                    sb[to] = temp;
                }
            }
        }

        //public string ReverseWords(string s)
        //{
        //    if (string.IsNullOrWhiteSpace(s))
        //    {
        //        return string.Empty;
        //    }

        //    StringBuilder sb = new StringBuilder(' ' + s);
        //    //sb.Append(new char[] { ' ' }, 0, 1);

        //    int head = 0;
        //    int tail = 0;
        //    int i = 0;
        //    int space = 0;
        //    int size = 0;
        //    int wordSize = 0;
        //    int jump = 0;
        //    int last = 0;
        //    while (i < sb.Length)
        //    {
        //        while (i < sb.Length && sb[i].Equals(' '))
        //        {
        //            i++;
        //            jump++;
        //        }

        //        while (i < sb.Length && !sb[i].Equals(' '))
        //        {
        //            i++;
        //            wordSize++;
        //        }
        //        space += wordSize > 0 ? 1 : 0;
        //        size += wordSize;
        //        last = tail;
        //        tail = i - 1;

        //        Console.WriteLine($"{sb.ToString()} length = {size} space = {space} wordsize = {wordSize} jump = {jump} head = {head} tail = {tail} i = {i}");
        //        ReverseStringBetween(sb, head, tail);

        //        //head = tail + 1 + wordSize;
        //        head = i - (jump) + 1;
        //        wordSize = 0;
        //        jump = 0;
        //        //head = i - jump + 1;
        //    }

        //    //ReverseStringBetween(sb.Remove(sb.Length - 1, 1), 0, sb.Length - 1);
        //    sb.Remove(size + space - 1, sb.Length - (size + space) + 1);
        //    ReverseStringBetween(sb, 0, sb.Length - 1);
        //    return sb.ToString();
        //}

        //public string ReverseWords(string s)
        //{
        //    if (string.IsNullOrWhiteSpace(s))
        //    {
        //        return string.Empty;
        //    }

        //    StringBuilder sb = new StringBuilder(s);
        //    sb.Append('_');

        //    int head = 0;
        //    int tail = 0;
        //    for (int i = 0; i < sb.Length; i++)
        //    {
        //        if (sb[i].Equals('_'))
        //        {
        //            if (head > tail)
        //            {
        //                ReverseStringBetween(sb, head, i - 1);
        //                break;
        //            }
        //            else
        //            {
        //                break;
        //            }
        //        }
        //        // this one is space, but last one is not, so i is head of a word
        //        if (sb[i].Equals(' ') && !sb[i + 1].Equals(' '))
        //        {
        //            head = i + 1;
        //            ReverseStringBetween(sb, head, tail);
        //        }
        //        // this one is not space, but last one is, so i is tail of a word
        //        else if (!sb[i].Equals(' ') && sb[i + 1].Equals(' '))
        //        {
        //            tail = i;
        //        }
        //        else if(sb[i].Equals(' '))
        //        {
        //            sb.Remove(i, 1);
        //        }
        //    }

        //    ReverseStringBetween(sb.Remove(sb.Length - 1, 1), 0, sb.Length - 1);
        //    return sb.ToString();
        //}

    }
}
