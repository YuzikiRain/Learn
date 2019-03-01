using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode_Qinyu.ImplementStrStr_28
{
    class ImplementStrStr_28
    {
        public void Test()
        {
            //int result = StrStr("mississippi", "issipi");
            //int result = StrStr("hello", "abc");
            //int result = StrStr("hello", "hello world");
            //int result = StrStr("hello", null);
            //int result = StrStr("hello", "");
            int result = StrStr("hello", "llo");
            Console.WriteLine(result);
        }

        public int StrStr(string haystack, string needle)
        {
            if (string.IsNullOrEmpty(needle))
            {
                return 0;
            }
            else if (haystack.Length < needle.Length)
            {
                return -1;
            }

            int i = 0;
            int j = 0;
            bool isMatched = true;
            while ( i < haystack.Length - needle.Length + 1)
            {
                isMatched = true;
                while( j < needle.Length)
                {
                    if (haystack[i+j] != needle[j])
                    {
                        i++;
                        j = 0;
                        isMatched = false;
                        break;
                    }
                    else
                    {
                        j++;
                    }
                }
                if (isMatched)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
