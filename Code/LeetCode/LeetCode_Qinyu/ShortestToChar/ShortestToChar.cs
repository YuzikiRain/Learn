using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode_Qinyu.ShortestToChar
{
    class ShortestToChar_821
    {
        public void Test()
        {
            string test = "loveleetcode";
            char c = 'e';

            foreach (var element in ShortestToChar(test, c))
                { Console.Write($"{element}"); }

            //Console.ReadLine();
        }

        public int[] ShortestToChar(string S, char C)
        {
            List<int> array_index = new List<int>() { -10000};
            for (int index = 0; index < S.Length; index++)
            {
                if (S[index] == C)
                {
                    array_index.Add(index);
                }
            }
            array_index.Add(10000 + 10000);

            List<int> result = new List<int>();
            int i = 0;
            for (int index = 0; index < S.Length; index++)
            {
                int left = Math.Abs(index - array_index[i]);
                int right = Math.Abs(index - array_index[i + 1]);
                result.Add(left <= right ? left : right);
                Console.WriteLine($"{S[index]}, index = {index}, array_index[{i}] = {array_index[i]}, left = {left}, right = {right}, get {result.Last()}");
                if (index >= array_index[i + 1]){ i++; }
            }

            return result.ToArray();
        }
    }
}
