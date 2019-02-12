using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode_Qinyu.SelfDividingNumbers
{
    class SelfDividingNumbers_728
    {

        public void Test()
        {
            //string[] test = new string[] { "Hello", "Alaska", "Dad", "Peace" };
            //string[] test = new string[] {"qz", "wq", "asdddafadsfa", "adfadfadfdassfawde"};
            foreach(var str in SelfDividingNumbers(1, 22))
            {
                Console.Write($"{str}  ");
            }
            //Console.ReadLine();
        }

        public IList<int> SelfDividingNumbers(int left, int right)
        {
            List<int> result = new List<int>();
            for(int i = left; i <= right; i++)
            {
                bool canDivide = true;
                int multi = 1;

                while (multi < i)
                {
                    int wei = (i % (multi*10)) / (multi);
                    if(wei == 0) { canDivide = false; break; }
                    if (i%wei != 0) { canDivide = false; break; }
                    //Console.WriteLine($"{i}%{wei} == 0");
                    multi *= 10;
                }
                if (canDivide) { result.Add(i);}
            }

            return result;
        }
    }
}
