using System;

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
            int m = haystack.Length;
            int n = needle.Length;
            if (m < n)
            {
                return -1;
            }

            int i = 0;
            int j = 0;
            while ( i < m - n + 1)
            {
                while( j < n)
                {
                    // not matched, reset j, move i to next position
                    if (haystack[i+j] != needle[j])
                    {
                        i++;
                        j = 0;
                        break;
                    }
                    else
                    {
                        j++;
                    }
                }
                // j >= needle.Length means all characters match
                if (j >= n)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
