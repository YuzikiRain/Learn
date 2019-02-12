using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode_Qinyu.Permutations46
{
    class Permutations
    {
        IList<IList<int>> results = new List<IList<int>>();

        public void Test()
        {
            var r = Permute(new int[] { 1, 2,3,4 });
            foreach(var i in r)
            {
                foreach (var j in i)
                {
                    Console.Write($"{j},");
                }
                Console.WriteLine();
            }
        }

        public IList<IList<int>> Permute(int[] nums)
        {
            AddElement(new List<int>(), new List<int>(nums));

            return results;
        }

        public void AddElement(List<int> result, List<int> nums)
        {
            for (var i = 0; i < nums.Count; i++)
            {
                var resultClone = new List<int>(result.ToArray());
                resultClone.Add(nums[i]);

                var listNew = new List<int>();
                for (var left = 0; left < i; left++)
                {
                    listNew.Add(nums[left]);
                }
                for (var right = i + 1; right < nums.Count; right++)
                {
                    listNew.Add(nums[right]);
                }

                if (listNew.Count == 0) { results.Add(resultClone); }
                else { AddElement(resultClone, listNew); }
            }
        }
    }
}
