using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Offer39
{
    public void Test()
    {
        int[] values = new int[] { -1, 1, 1, 1, 2, 1};
        Console.WriteLine($"{MajorityElement(values)}");
    }

    public int MajorityElement(int[] nums)
    {
        // 假设全是大于0的数
        Dictionary<int, int> counts = new Dictionary<int, int>();
        for (int i = 0; i < nums.Length; i++)
        {
            int value = nums[i];
            if (counts.ContainsKey(value)) { counts[value] += 1; }
            else { counts[value] = 1; }
        }
        foreach (var key in counts.Keys)
        {
            if (counts[key] > nums.Length >> 1) { return key; }
        }
        return -1;
    }
}