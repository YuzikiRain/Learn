using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode_Qinyu.CountingBits338
{
    class CountingBits338
    {
        public void Test()
        {
            foreach (var i in CountBits(13))
            {
                Console.Write($"{i}, ");
            }
        }

        public int[] CountBits(int num)
        {
            if (num == 0) { return new int[1] { 0 }; }

            int[] array = new int[num + 1];
            array[0] = 0;
            int i = 1;
            int n = 1;
            while (true)
            {
                while (n < (i * 2))
                {
                    array[n] = array[n - i] + 1;
                    // Console.WriteLine($"n={n} i={i}  array[{n}]={array[n]}  array[n-{i}]={array[n-i]}   {Convert.ToString(n, 2)}");
                    n++;
                    if (n >= num + 1) { return array; }
                }
                i *= 2;
            }

        }
    }
}
