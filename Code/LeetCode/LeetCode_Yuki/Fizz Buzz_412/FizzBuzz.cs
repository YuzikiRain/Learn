using System;
using System.Collections.Generic;

namespace LeetCode_Yuki.Fizz_Buzz_412
{
    class FizzBuzz_412
    {
        public void Test()
        {
            var list = FizzBuzz(100);
            foreach(var number in list)
            {
                Console.WriteLine(number);
            }
        }

        public IList<string> FizzBuzz(int n)
        {
            string[] result = new string[n];
            int three = 3;
            int five = 5;
            int fifteen = 15;
            for(int i = 0; i < n; i++)
            {
                if (i + 1 == fifteen)
                {
                    result[i] = "FizzBuzz";
                    fifteen += 15;
                    three += 3;
                    five += 5;
                }
                else if (i + 1 == three)
                {
                    result[i] = "Fizz";
                    three += 3;
                }
                else if (i + 1 == five)
                {
                    result[i] = "Buzz";
                    five += 5;
                }
                else
                {
                    result[i] = (i + 1).ToString();
                }
            }

            return result;
        }
    }
}
